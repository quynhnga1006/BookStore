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
    public class CollateralDataImporting : IDataImporting, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<ReportItem, Guid> _reportItemRepository;
        private readonly IRepository<CollateralItem, Guid> _collateralRepository;

        public CollateralDataImporting(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<ReportItem, Guid> reportItemRepository,
            IRepository<CollateralItem, Guid> collateralRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _reportItemRepository = reportItemRepository;
            _collateralRepository = collateralRepository;
        }
        public async Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var tsbdItems = await GetTsbdItemsFromDataTableAsync(args, dataTable);
            await _collateralRepository.InsertManyAsync(tsbdItems);
        }
        private async Task<List<CollateralItem>> GetTsbdItemsFromDataTableAsync(DataImportingArgs args, DataTable dataTable)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }

            var indexOfCIF = dataTable.Columns.IndexOf("SoCIF");

            var colatteralItems = new List<CollateralItem>();
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

                var reportItem = _reportItemRepository
                    .Where(rp => rp.ReportType.Equals(ReportType.Loan))
                    .Where(rp => rp.DateOfData.Equals(args.DateOfData))
                    .Where(rp => rp.CifNumber.Equals(row[indexOfCIF].ToString()))
                    .FirstOrDefault();

                var departmentId = reportItem != null ? reportItem.DepartmentId : Guid.Empty;

                colatteralItems.Add(new CollateralItem
                {
                    DepartmentId = departmentId,
                    ReportFileId = args.ReportFileId,
                    ReportType = (int)args.ReportType,
                    DateOfData = args.DateOfData,
                    ReportData = new MongoDB.Bson.BsonDocument(childRow)
                });
            }
            return colatteralItems;
        }

    }
}