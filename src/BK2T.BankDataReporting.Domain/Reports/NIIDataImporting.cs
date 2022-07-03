using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using MongoDB.Bson;
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
    public class NIIDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly INiiItemRepository _niiItemRepository;
        private readonly IReportItemRepository _reportItemRepository;

        public NIIDataImporting(IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<Department, Guid> departmentItemRepository,
            IIdentityUserRepository identityUserRepository,
            INiiItemRepository niiItemRepository,
            IReportItemRepository reportItemRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _departmentItemRepository = departmentItemRepository;
            _identityUserRepository = identityUserRepository;
            _niiItemRepository = niiItemRepository;
            _reportItemRepository = reportItemRepository;
        }
        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var niiItems = await GetNiiItemsFromDataTableAsync(args, dataTable);
            await _niiItemRepository.InsertManyAsync(niiItems);
        }
        private async Task<List<NiiItem>> GetNiiItemsFromDataTableAsync(DataImportingArgs args, DataTable data)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var departments = await _departmentItemRepository.GetListAsync();
            var users = await _identityUserRepository.GetListAsync();
            var userDicts = users.ToDictionary(u => u.UserName, u => u.Id);
            var dataTypeDict = new Dictionary<string, int>();
            var cumulativeCols = new Dictionary<string, string>();
            if (template != null)
            {
                dataTypeDict = template.Template
                    .Where(t => t.Value.AsBsonDocument.TryGetValue("DataType", out _))
                    .ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
                cumulativeCols = template.Template
                    .Where(t => t.Value.AsBsonDocument.TryGetValue("CumulativeFrom", out _))
                    .ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("CumulativeFrom").AsString);
            }

            var firstDateInLastMonth = new DateTime(args.DateOfData.Year, args.DateOfData.Month != 1 ? args.DateOfData.Month - 1 : 1, 1);
            var niiItemsInLastMonth = _niiItemRepository
                .Where(d => d.DateOfData >= firstDateInLastMonth)
                .ToList();


            var niiItems = new List<NiiItem>();
            foreach (DataRow row in data.Rows)
            {
                var childRow = new Dictionary<string, object>();
                foreach (DataColumn col in data.Columns)
                {
                    var value = (row[col] == DBNull.Value) ? string.Empty : row[col];
                    var dataType = (ReportItemDataType)dataTypeDict.GetValueOrDefault(col.ColumnName);
                    var convertedValue = DataImportingJob.ConvertValueToStrongType(dataType, value);
                    childRow.Add(col.ColumnName, convertedValue);
                }

                childRow.TryGetValue("SoTaiKhoan", out var accountNumber);
                var lastNiiItem = niiItemsInLastMonth
                    .Where(x => x.AccountNumber.Equals(accountNumber))
                    .Select(x => x.ReportData)
                    .LastOrDefault();

                foreach (var col in cumulativeCols)
                {
                    BsonValue lastCumulativeValue = 0.0;
                    if (lastNiiItem != null) lastNiiItem.TryGetValue(col.Key, out lastCumulativeValue);
                    childRow.TryGetValue(col.Value, out var newValue);
                    childRow.Add(col.Key, (double)newValue + (double)lastCumulativeValue);
                }

                var reportItems = _reportItemRepository
                                    .Where(rp => rp.ReportType.Equals(ReportType.Loan))
                                    .Where(rp => rp.DateOfData.Equals(args.DateOfData))
                                    .Where(rp => rp.AccountNumber.Equals(row["SoTaiKhoan"]))
                                    .FirstOrDefault();
                if (reportItems == null)
                {
                    reportItems = _reportItemRepository
                                    .Where(rp => rp.ReportType.Equals(ReportType.Deposit))
                                    .Where(rp => rp.DateOfData.Equals(args.DateOfData))
                                    .Where(rp => rp.AccountNumber.Equals(row["SoTaiKhoan"]))
                                    .FirstOrDefault();
                }

                var departmentId = reportItems != null ? reportItems.DepartmentId : Guid.Empty;
                var userId = reportItems != null ? reportItems.UserId : Guid.Empty;

                niiItems.Add(new NiiItem
                {
                    DepartmentId = departmentId,
                    UserId = userId,
                    ReportFileId = args.ReportFileId,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    MonthOfData = args.DateOfData.Month,
                    YearOfData = args.DateOfData.Year,
                    AccountNumber = accountNumber?.ToString(),
                    ReportData = new BsonDocument(childRow)
                });
            }
            return niiItems;
        }
    }
}