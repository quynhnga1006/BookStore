using BK2T.BankDataReporting.MongoDB;
using BK2T.BankDataReporting.ReportFiles;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace BK2T.BankDataReporting.Reports
{
    public class NiiItemRepository : MongoDbRepository<BankDataReportingMongoDbContext, NiiItem, Guid>,
        INiiItemRepository
    {
        public NiiItemRepository(IMongoDbContextProvider<BankDataReportingMongoDbContext> dbContextProvider) : base(dbContextProvider) { }
        public async Task<(long total, List<BsonDocument> reportItems)> GetNiiItemsByGroupBySetting(
            List<BsonElement> groupBySettings,
            List<BsonElement> niiSettings,
            DateTime reportDate)
        {
            var niiItemCollection = await GetCollectionAsync();

            var filterByMonth = new BsonArray {
                new BsonDocument(
                    "MonthOfData",
                    new BsonDocument("$eq", reportDate.Month - 1)),
                new BsonDocument(
                    "YearOfData",
                    new BsonDocument("$eq", reportDate.Year))
            };

            var match = new BsonDocument
            {
                {
                    "$and",
                    filterByMonth
                }
            };

            var pipeline = new List<BsonDocument>
            {
                new BsonDocument {
                    {
                        "$match",
                        match
                    }
                }
            };

            if (groupBySettings != null && groupBySettings.Any())
            {
                var (group, projection) = BuildGroupBySettings(groupBySettings, niiSettings);
                pipeline.Add(new BsonDocument("$group", group));
                pipeline.Add(new BsonDocument("$project", projection));
            }

            var results = await niiItemCollection.Aggregate<BsonDocument>(pipeline.ToArray()).ToListAsync();

            return (results.Count, results);
        }
        private static (BsonDocument, BsonDocument) BuildGroupBySettings(List<BsonElement> groupBySettings, List<BsonElement> niiSettings)
        {
            var groupIds = groupBySettings
                .ToDictionary(g => g.Name, g => $"${g.Name}")
                .ToBsonDocument();
            var groupBy = new BsonDocument("_id", groupIds);

            foreach (var setting in niiSettings)
            {
                var bsonElement = new BsonElement(setting.Name, new BsonDocument("$first", $"${setting.Name}"));
                if (setting.Value.AsBsonDocument.GetValue("DataType").AsInt32 == (int)ReportItemDataType.Number)
                {
                    bsonElement = new BsonElement(setting.Name, new BsonDocument("$sum", $"${setting.Name}"));
                }
                groupBy.Add(bsonElement);
            }


            var projections = groupBySettings
                .ToDictionary(x => x.Name, x => $"$_id.{x.Name}")
                .ToBsonDocument();
            var notGroupByProjections = niiSettings.Select(x => new BsonElement(x.Name, $"${x.Name}"));
            projections.AddRange(notGroupByProjections);

            return (groupBy, projections);
        }
    }
}