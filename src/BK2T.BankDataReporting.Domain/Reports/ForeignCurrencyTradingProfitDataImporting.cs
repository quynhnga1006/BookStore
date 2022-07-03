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
    public class ForeignCurrencyTradingProfitDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<ForeignCurrencyTradingProfitItem, Guid> _foreignCurrencyTradingProfitRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;

        public ForeignCurrencyTradingProfitDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<ForeignCurrencyTradingProfitItem, Guid> foreignCurrencyTradingProfitRepository,
            IRepository<Department, Guid> departmentItemRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _foreignCurrencyTradingProfitRepository = foreignCurrencyTradingProfitRepository;
            _departmentItemRepository = departmentItemRepository;
        }

        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var foreignCurrencyTradingProfitItems = await GetForeignCurrencyTradingProfitItemsFromDataTableAsync(args, dataTable);
            await _foreignCurrencyTradingProfitRepository.InsertManyAsync(foreignCurrencyTradingProfitItems);
        }

        private async Task<List<ForeignCurrencyTradingProfitItem>> GetForeignCurrencyTradingProfitItemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }
            var departments = await _departmentItemRepository.GetListAsync();
            var foreignCurrencyTradingProfitItems = new List<ForeignCurrencyTradingProfitItem>();
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

                var departmentCode = childRow.GetValueOrDefault("MaPhong").ToString();
                var departmentId = departments.FirstOrDefault(d => d.Code.Equals(departmentCode))?.Id;

                foreignCurrencyTradingProfitItems.Add(new ForeignCurrencyTradingProfitItem
                {
                    ReportFileId = args.ReportFileId,
                    DepartmentId = departmentId ?? Guid.Empty,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return foreignCurrencyTradingProfitItems;
        }
    }
}
