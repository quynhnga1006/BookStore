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
    public class CorporateCustomerDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<CorporateCustomerItem, Guid> _corCustomerRepository;
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<ReportItem, Guid> _reportItemRepository;

        public CorporateCustomerDataImporting(
            IRepository<CorporateCustomerItem, Guid> corCustomerRepository,
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<ReportItem, Guid> reportItemRepository
            )
        {
            _corCustomerRepository = corCustomerRepository;
            _reportTemplateRepository = reportTemplateRepository;
            _reportItemRepository = reportItemRepository;
        }
        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            await GetCorporateCustomerItems(args, dataTable);
        }

        public async Task GetCorporateCustomerItems(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }

            var indexOfCIF = dataTable.Columns.IndexOf("SoCIF");
            var corCustomerItems = new List<CorporateCustomerItem>();


            var skipCount = 0;
            var takeCount = 500;
            while (skipCount < dataTable.Rows.Count)
            {
                DataTable takeRows = dataTable.AsEnumerable().Skip(skipCount).Take(takeCount).CopyToDataTable();

                var arrsCifNumber = takeRows.AsEnumerable().Select(r => r.Field<string>("SoCIF")).ToList();

                var reportItems = _reportItemRepository
                    .Where(rp => arrsCifNumber.Contains(rp.CifNumber)).ToList();

                foreach (DataRow row in takeRows.Rows)
                {
                    var childRow = new Dictionary<string, object>();
                    foreach (DataColumn col in takeRows.Columns)
                    {
                        var value = (row[col] == DBNull.Value) ? string.Empty : row[col];
                        var dataType = (ReportItemDataType)dataTypeDict.GetValueOrDefault(col.ColumnName);
                        var convertedValue = DataImportingJob.ConvertValueToStrongType(dataType, value);
                        childRow.Add(col.ColumnName, convertedValue);
                    }
                    var reportItem = reportItems
                        .Where(rp => rp.ReportType == (int)ReportType.Loan)
                        .Where(rp => rp.CifNumber.Equals(row[indexOfCIF].ToString()))
                        .GroupBy(rp => rp.DepartmentId)
                        .Select(rp => new
                        {
                            departmentId = rp.Key,
                            sum = rp.Sum(c => (double)c.ReportData["DuNoBQNamQuyDoi"])
                        })
                        .OrderBy(rp => rp.sum)
                        .LastOrDefault(); 

                    if (reportItem == null)
                    {
                        reportItem = reportItems
                            .Where(rp => rp.ReportType == (int)ReportType.Deposit)
                            .Where(rp => rp.CifNumber.Equals(row[indexOfCIF].ToString()))
                            .GroupBy(rp => rp.DepartmentId)
                            .Select(rp => new
                            {
                                departmentId = rp.Key,
                                sum = rp.Sum(c => (double)c.ReportData["SoDuTienGuiBQNamQuyDoi"])
                            })
                            .OrderBy(rp => rp.sum)
                            .LastOrDefault();
                    }

                    if (reportItem == null)
                    {
                        continue;
                    }

                    var departmentId = reportItem.departmentId;
                    corCustomerItems.Add(new CorporateCustomerItem
                    {
                        DepartmentId = departmentId,
                        ReportFileId = args.ReportFileId,
                        ReportType = (int)args.ReportType,
                        DateOfData = args.DateOfData,
                        ReportData = new MongoDB.Bson.BsonDocument(childRow)
                    });
                }

                await _corCustomerRepository.InsertManyAsync(corCustomerItems);

                corCustomerItems.Clear();
                reportItems.Clear();
                arrsCifNumber.Clear();

                skipCount += takeCount;
            }
        }

    }
}
