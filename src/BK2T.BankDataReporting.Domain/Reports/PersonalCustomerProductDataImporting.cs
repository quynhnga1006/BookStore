using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BK2T.BankDataReporting.Reports
{
    public class PersonalCustomerProductDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<ReportItem, Guid> _reportItemRepository;
        private readonly IRepository<PersonalCustomerProductItem, Guid> _personalCustomerProductItemsRepository;
        const int batchSize = 500;

        public PersonalCustomerProductDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateItemRepository,
            IRepository<ReportItem, Guid> reportItemRepository,
            IRepository<PersonalCustomerProductItem, Guid> personalCustomerProductItemRepository
            )
        {
            _reportTemplateRepository = reportTemplateItemRepository;
            _reportItemRepository = reportItemRepository;
            _personalCustomerProductItemsRepository = personalCustomerProductItemRepository;
        }
        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var personalCustomerItems = await GetPersonalCustomerProductItemsFromDataTableAsync(args, dataTable);
            await _personalCustomerProductItemsRepository.InsertManyAsync(personalCustomerItems);
        }
        private async Task<List<PersonalCustomerProductItem>> GetPersonalCustomerProductItemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }
            var personalCustomerItems = new List<PersonalCustomerProductItem>();
            var departmentId = new Guid();
            int total = (dataTable.Rows.Count / batchSize) + ((dataTable.Rows.Count % batchSize) > 0 ? 1 : 0);
            var i = 1;
            while (i <= total)
            {
                int startIndex = (i - 1) * batchSize;
                DataTable resultDatatable = dataTable.Rows.Cast<DataRow>()
                    .Skip(startIndex)
                    .Take(batchSize)
                    .CopyToDataTable();
                var cifs = new List<string>();
                foreach (DataRow row in resultDatatable.Rows)
                {
                    cifs.Add((string)row["SoCIF"]);
                }

                var loanReportItems = _reportItemRepository
                    .Where(rp => rp.DateOfData.Equals(args.DateOfData))
                    .Where(rp => rp.ReportType.Equals(ReportType.Loan))
                    .Where(rp => cifs.Contains(rp.CifNumber))
                    .ToList();

                var depositReportItems = _reportItemRepository
                    .Where(rp => rp.DateOfData.Equals(args.DateOfData))
                    .Where(rp => rp.ReportType.Equals(ReportType.Deposit))
                    .Where(rp => cifs.Contains(rp.CifNumber))
                    .ToList();

                foreach (DataRow row in resultDatatable.Rows)
                {
                    var childRow = new Dictionary<string, object>();
                    foreach (DataColumn col in resultDatatable.Columns)
                    {
                        var value = (row[col] == DBNull.Value) ? string.Empty : row[col];
                        var dataType = (ReportItemDataType)dataTypeDict.GetValueOrDefault(col.ColumnName);
                        var convertedValue = DataImportingJob.ConvertValueToStrongType(dataType, value);
                        childRow.Add(col.ColumnName, convertedValue);
                    }

                    var reportItem = loanReportItems
                        .Where(rp => rp.CifNumber.Equals(row["SocIF"]))
                        .GroupBy(rp => rp.DepartmentId)
                        .Select(rp => new
                        {
                            departmentId = rp.Key,
                            amount = rp.Sum(c => (double)c.ReportData["DuNoBQNamQuyDoi"]),
                        })
                        .OrderByDescending(rp => rp.amount)
                        .FirstOrDefault();

                    if (reportItem == null)
                    {
                        reportItem = depositReportItems
                            .Where(rp => rp.CifNumber.Equals(row["SocIF"]))
                            .GroupBy(rp => rp.DepartmentId)
                            .Select(rp => new
                            {
                                departmentId = rp.Key,
                                amount = rp.Sum(c => (double)c.ReportData["SoDuTienGuiBQNamQuyDoi"]),
                            })
                       .OrderByDescending(rp => rp.amount)
                       .FirstOrDefault();
                    }
                    departmentId = reportItem != null ? reportItem.departmentId : Guid.Empty;
                    personalCustomerItems.Add(new PersonalCustomerProductItem
                    {
                        DepartmentId = departmentId,
                        ReportFileId = args.ReportFileId,
                        ReportType = (int)args.ReportType,
                        DateOfData = args.DateOfData,
                        ReportData = new MongoDB.Bson.BsonDocument(childRow)
                    });
                }
                i++;
            }
            return personalCustomerItems;
        }
    }
}