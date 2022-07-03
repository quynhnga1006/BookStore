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
using Volo.Abp.Identity;

namespace BK2T.BankDataReporting.Reports
{
    public class DebtDueCustomerDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;
        private readonly IRepository<DebtDueCustomerItem, Guid> _debtDueCustomerItemRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public DebtDueCustomerDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<Department, Guid> departmentItemRepository,
            IRepository<DebtDueCustomerItem, Guid> debtDueCustomerItemRepository,
            IIdentityUserRepository identityUserRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _departmentItemRepository = departmentItemRepository;
            _debtDueCustomerItemRepository = debtDueCustomerItemRepository;
            _identityUserRepository = identityUserRepository;
        }

        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable data)
        {
            var debtDueCustomerItems = await GetDebtDueCustomerItemsFromDataTable(args, data);
            await _debtDueCustomerItemRepository.InsertManyAsync(debtDueCustomerItems);
        }

        // handle if report type is debt due customers
        public async Task<List<DebtDueCustomerItem>> GetDebtDueCustomerItemsFromDataTable(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }
            var departments = await _departmentItemRepository.GetListAsync();
            var users = await _identityUserRepository.GetListAsync();
            var userDicts = users.ToDictionary(u => u.UserName, u => u.Id);
            var debtDueCustomerItems = new List<DebtDueCustomerItem>();
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
                var (username, departmentCode) = ApplyDepartmentImportRule(childRow, departments, args.ReportType);
                var departmentId = departments.FirstOrDefault(d => d.Code.Equals(departmentCode))?.Id;
                debtDueCustomerItems.Add(new DebtDueCustomerItem
                {
                    UserId = userDicts.GetValueOrDefault(username),
                    DepartmentId = departmentId ?? Guid.Empty,
                    ReportFileId = args.ReportFileId,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return debtDueCustomerItems;
        }

        // TODO: Ugly hack hard code the header for the rule. Need to do refactor!
        protected (string, string) ApplyDepartmentImportRule(Dictionary<string, object> dataItem, List<Department> allDepartments, ReportType reportType)
        {
            var username = dataItem.GetValueOrDefault("CanBoQLTaiKhoan");
            if (username == null) username = "";
            var departmentCode = "";
            dataItem["CanBoQLTaiKhoan"] = username.ToString().ToLower();
            var oldCode = dataItem.GetValueOrDefault("MaDonVi").ToString();
            var departments = allDepartments.Where(x => x.OldCode.Equals(oldCode));
            if (string.IsNullOrEmpty(oldCode) || !departments.Any()) return (dataItem["CanBoQLTaiKhoan"].ToString(), departmentCode.ToString());

            if (oldCode == "48098")
            {
                var customerSegment = dataItem.GetValueOrDefault("PhanKhuc").ToString();
                customerSegment = string.IsNullOrEmpty(customerSegment) ? customerSegment : customerSegment.Substring(0, 2);
                departmentCode = BuildDepartmentCode(departments, customerSegment, reportType);
                return (dataItem["CanBoQLTaiKhoan"].ToString(), departmentCode.ToString());
            }
            var department = departments.FirstOrDefault();
            if (departmentCode.ToString().Equals(department.Code)) return (dataItem["CanBoQLTaiKhoan"].ToString(), departmentCode.ToString());
            departmentCode = department.Code;
            return (dataItem["CanBoQLTaiKhoan"].ToString(), departmentCode.ToString());
        }

        // TODO: Ugly hack hard code the header for the rule. Need to do refactor!
        private string BuildDepartmentCode(IEnumerable<Department> departments, string customerSegment, ReportType reportType)
        {
            var department = departments.FirstOrDefault(d => d.CustomerSegments.Contains(customerSegment));
            if (department != null) return department.Code;
            return "048003000";
        }
    }
}