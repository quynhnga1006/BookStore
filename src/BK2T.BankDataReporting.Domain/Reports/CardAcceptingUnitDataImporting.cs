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
    public class CardAcceptingUnitDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<CardAcceptingUnitItem, Guid> _cardAcceptingUnitRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public CardAcceptingUnitDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<CardAcceptingUnitItem, Guid> cardAcceptingUnitRepository,
            IRepository<Department, Guid> departmentItemRepository,
            IIdentityUserRepository identityUserRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _cardAcceptingUnitRepository = cardAcceptingUnitRepository;
            _departmentItemRepository = departmentItemRepository;
            _identityUserRepository = identityUserRepository;
        }
        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var cardAcceptingUnitItems = await GetCardAcceptingUnittemsFromDataTableAsync(args, dataTable);
            await _cardAcceptingUnitRepository.InsertManyAsync(cardAcceptingUnitItems);
        }

        private async Task<List<CardAcceptingUnitItem>> GetCardAcceptingUnittemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
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
            var cardAcceptingUnitItemItems = new List<CardAcceptingUnitItem>();
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
                var username = childRow.GetValueOrDefault("STAFFID").ToString();
                if (username == null) username = "";
                childRow["STAFFID"] = username.ToString().ToLower();
                var departmentId = departments.FirstOrDefault(d => d.Code.Equals(childRow["MaPhong"]))?.Id;
                cardAcceptingUnitItemItems.Add(new CardAcceptingUnitItem
                {
                    ReportFileId = args.ReportFileId,
                    UserId = userDicts.GetValueOrDefault(username),
                    DepartmentId = departmentId ?? Guid.Empty,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return cardAcceptingUnitItemItems;
        }
    }
}