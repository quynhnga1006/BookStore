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
    public class InsuranceDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<InsuranceItem, Guid> _insuranceRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;

        public InsuranceDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<InsuranceItem, Guid> insuranceRepository,
            IRepository<Department, Guid> departmentItemRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _insuranceRepository = insuranceRepository;
            _departmentItemRepository = departmentItemRepository;
        }

        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var insuranceItems = await GetInsuranceItemsFromDataTableAsync(args, dataTable);
            await _insuranceRepository.InsertManyAsync(insuranceItems);
        }

        private async Task<List<InsuranceItem>> GetInsuranceItemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }
            var departments = await _departmentItemRepository.GetListAsync();
            var insuranceItems = new List<InsuranceItem>();
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
                var departmentCode = "";
                if (args.ReportType == ReportType.LifeInsurance)
                {
                    departmentCode = childRow.GetValueOrDefault("MaPGD").ToString();
                }
                else if (args.ReportType == ReportType.NonLifeInsurance)
                {
                    departmentCode = childRow.GetValueOrDefault("MaPhong").ToString();
                }
                var departmentId = departments.FirstOrDefault(d => d.Code.Equals(departmentCode))?.Id;

                insuranceItems.Add(new InsuranceItem
                {
                    ReportFileId = args.ReportFileId,
                    DepartmentId = departmentId ?? Guid.Empty,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return insuranceItems;
        }
    }
}
