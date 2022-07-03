using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace BK2T.BankDataReporting.Reports
{
    public class RetailDevelopmentCustomerDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<RetailDevelopmentCustomerItem, Guid> _retailDevelopmentCustomerRepository;
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<ReportItem, Guid> _reportItemRepository;
        private readonly IRepository<Department, Guid> _departmentRepository;
        const int batchSize = 500;

        public RetailDevelopmentCustomerDataImporting(
            IRepository<RetailDevelopmentCustomerItem, Guid> retailDevelopmentCustomerRepository,
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<ReportItem, Guid> reportItemRepository,
            IRepository<Department, Guid> departmentRepository
            )
        {
            _retailDevelopmentCustomerRepository = retailDevelopmentCustomerRepository;
            _reportTemplateRepository = reportTemplateRepository;
            _reportItemRepository = reportItemRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var retailDevelopmentCustomerItems = await GetRetailDevelopmentCustomerItemsFromDataTableAsync(args, dataTable);
            await _retailDevelopmentCustomerRepository.InsertManyAsync(retailDevelopmentCustomerItems);
        }

        private async Task<List<RetailDevelopmentCustomerItem>> GetRetailDevelopmentCustomerItemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }
            var departments = await _departmentRepository.GetListAsync();
            var retailDevelopmentCustomerItems = new List<RetailDevelopmentCustomerItem>();
            Guid? departmentId = new Guid();
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
                    cifs.Add((string)row["CIF"]);
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
                    var departmentCode = childRow["MaPhong"].ToString();
                    if (!string.IsNullOrEmpty(departmentCode))
                    {
                        departmentId = departments.FirstOrDefault(d => d.Code.Equals(departmentCode))?.Id;
                    }
                    else
                    {
                        var reportItem = loanReportItems
                            .Where(rp => rp.CifNumber.Equals(row["CIF"]))
                            .LastOrDefault();
                        if(reportItem == null)
                        {
                            reportItem = depositReportItems
                                .Where(rp => rp.CifNumber.Equals(row["CIF"]))
                                .LastOrDefault();
                        }
                        departmentId = reportItem != null ? reportItem.DepartmentId : Guid.Empty;
                    }
                    retailDevelopmentCustomerItems.Add(new RetailDevelopmentCustomerItem
                    {
                        DepartmentId = departmentId != null ? (Guid)departmentId : Guid.Empty,
                        ReportFileId = args.ReportFileId,
                        ReportType = (int)args.ReportType,
                        DateOfData = args.DateOfData,
                        ReportData = new MongoDB.Bson.BsonDocument(childRow)
                    });
                }
                i++;
            }
            return retailDevelopmentCustomerItems;
        }
    }
}
