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
    public class ProvisionDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<ProvisionItem, Guid> _provisionRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;

        public ProvisionDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<ProvisionItem, Guid> provisionRepository,
            IRepository<Department, Guid> departmentItemRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _provisionRepository = provisionRepository;
            _departmentItemRepository = departmentItemRepository;
        }
        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var provisionItems = await GetProvisionItemsFromDataTableAsync(args, dataTable);
            await _provisionRepository.InsertManyAsync(provisionItems);
        }
        private async Task<List<ProvisionItem>> GetProvisionItemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }
            var departments = await _departmentItemRepository.GetListAsync();
            var provisionItems = new List<ProvisionItem>();
            foreach (DataRow row in dataTable.Rows)
            {
                var childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    var value = (row[col] == DBNull.Value) ? string.Empty : row[col];
                    var dataType = (ReportItemDataType)dataTypeDict.GetValueOrDefault(col.ColumnName);
                    var convertedValue = DataImportingJob.ConvertValueToStrongType(dataType, value);
                    childRow.Add(col.ColumnName, convertedValue);
                }

                var departmentCode = ApplyDepartmentImportRule(childRow, departments);
                var departmentId = departments.FirstOrDefault(d => d.Code.Equals(departmentCode))?.Id;

                provisionItems.Add(new ProvisionItem
                {
                    ReportFileId = args.ReportFileId,
                    DepartmentId = departmentId ?? Guid.Empty,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return provisionItems;
        }

        protected string ApplyDepartmentImportRule(Dictionary<string, object> dataItem, List<Department> allDepartments)
        {
            var oldCode = dataItem.GetValueOrDefault("PhongKinhDoanh").ToString();
            if (string.IsNullOrEmpty(oldCode)) return "";
            var department = allDepartments.FirstOrDefault(x => x.OldCode.Equals(oldCode));
            return department?.Code;
        }
    }
}
