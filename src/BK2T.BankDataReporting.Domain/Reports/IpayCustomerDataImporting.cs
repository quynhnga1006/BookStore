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
using Volo.Abp.Identity;

namespace BK2T.BankDataReporting.Reports
{
    public class IpayCustomerDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;
        private readonly IRepository<IPayCustomerItem, Guid> _ipayNumberOfCustomersRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public IpayCustomerDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<Department, Guid> departmentItemRepository,
            IRepository<IPayCustomerItem, Guid> ipayNumberOfCustomersRepository,
            IIdentityUserRepository identityUserRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _departmentItemRepository = departmentItemRepository;
            _ipayNumberOfCustomersRepository = ipayNumberOfCustomersRepository;
            _identityUserRepository = identityUserRepository;
        }

        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable data)
        {
            var ipayNumberOfCustomersItems = await GetIpayNumberOfCustomersItemsFromDataTable(args, data);
            await _ipayNumberOfCustomersRepository.InsertManyAsync(ipayNumberOfCustomersItems);
        }

        public async Task<List<IPayCustomerItem>> GetIpayNumberOfCustomersItemsFromDataTable(DataImportingArgs args, DataTable dataTable)
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
            var ipayNumberOfCustomerItems = new List<IPayCustomerItem>();
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
                ipayNumberOfCustomerItems.Add(new IPayCustomerItem
                {
                    ReportFileId = args.ReportFileId,
                    UserId = userDicts.GetValueOrDefault(username),
                    DepartmentId = departmentId ?? Guid.Empty,            
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return ipayNumberOfCustomerItems;
        }

        protected (string, string) ApplyDepartmentImportRule(Dictionary<string, object> dataItem, List<Department> allDepartments, ReportType reportType)
        {
            var username = dataItem.GetValueOrDefault("UserADCanBo").ToString();
            var code = dataItem.GetValueOrDefault("MaPhong").ToString();
            dataItem["UserADCanBo"] = username.ToString().ToLower();
            var department = allDepartments.FirstOrDefault(x => x.Code.Equals(code));
            return (dataItem["UserADCanBo"].ToString(), department?.Code);
        }
    }
}
