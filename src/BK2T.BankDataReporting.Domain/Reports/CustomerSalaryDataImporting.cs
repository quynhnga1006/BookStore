using BK2T.BankDataReporting.Departments;
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
    public class CustomerSalaryDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<Department, Guid> _departmentRepository;
        private readonly IRepository<CustomerSalaryItem, Guid> _customerSalaryRepository;

        public CustomerSalaryDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<Department, Guid> departmentRepository,
            IRepository<CustomerSalaryItem, Guid> customerSalaryRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _departmentRepository = departmentRepository;
            _customerSalaryRepository = customerSalaryRepository;
        }
        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var customerSalaryItems = await GetCustomerSalaryItemsFromDataTableAsync(args, dataTable);
            await _customerSalaryRepository.InsertManyAsync(customerSalaryItems);
        }
        private async Task<List<CustomerSalaryItem>> GetCustomerSalaryItemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }

            var customerSalaryItems = new List<CustomerSalaryItem>();
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

                var allDepartments = await _departmentRepository.GetListAsync();
                var (receiverDepartmentId, paidDepartmentId) = GetDepartmentIdOfImportData(childRow, allDepartments);

                customerSalaryItems.Add(new CustomerSalaryItem
                {
                    ReceiverDepartmentId = receiverDepartmentId,
                    PaidDepartmentId = paidDepartmentId,
                    ReportFileId = args.ReportFileId,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return customerSalaryItems;
        }
        protected static (Guid, Guid) GetDepartmentIdOfImportData(Dictionary<string, object> dataItem, List<Department> allDepartments)
        {
            var receiverCode = dataItem.GetValueOrDefault("MaPhongQuanLyTKCBNVNhanLuong");
            var paidCode = dataItem.GetValueOrDefault("MaPhongQLKHDNChiLuong");
            var receiverDepartmentId = Guid.Empty;
            var paidDepartmentId = Guid.Empty;
            if (receiverCode != null)
            {
                var receiverDepartment = allDepartments.Where(dp => dp.Code.Equals(receiverCode)).FirstOrDefault();
                receiverDepartmentId = receiverDepartment.Id;
            }
            if (paidCode != null)
            {
                var paidDepartment = allDepartments.Where(dp => dp.Code.Equals(paidCode)).FirstOrDefault();
                paidDepartmentId = paidDepartment.Id;
            }

            return (receiverDepartmentId, paidDepartmentId);
        }
    }
}
