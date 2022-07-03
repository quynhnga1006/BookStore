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
    public class ReportItemRepository : MongoDbRepository<BankDataReportingMongoDbContext, ReportItem, Guid>,
        IReportItemRepository
    {
        public ReportItemRepository(IMongoDbContextProvider<BankDataReportingMongoDbContext> dbContextProvider) : base(dbContextProvider) { }
        public async Task<(long total, List<BsonDocument> reportItems)> GetReportItemsByFilter(
            List<BsonElement> reportSettings,
            ReportType reportType,
            DateTime? reportDate,
            Guid? departmentId,
            Guid? userId,
            Dictionary<string, object> customParams,
            Dictionary<string, string> sortingParams,
            int skipCount = 0,
            int maxResultCount = 200)
        {
            var reportItemCollection = await GetCollectionAsync();
            var filters = new BsonArray
            {
                new BsonDocument(nameof(ReportItem.ReportType), reportType)
            };
            if (reportDate.HasValue)
            {
                reportDate = DateTime.SpecifyKind(reportDate.Value, DateTimeKind.Utc);
                filters.Add(new BsonDocument(nameof(ReportItem.DateOfData), reportDate.Value));
            }
            if (departmentId.HasValue) filters.Add(new BsonDocument(nameof(ReportItem.DepartmentId), departmentId.Value));
            if (userId.HasValue) filters.Add(new BsonDocument(nameof(ReportItem.UserId), userId.Value));

            var customerFilters = BuildCustomFilterDefinition(customParams);
            if (customerFilters != null && customerFilters.Any())
            {
                filters.AddRange(customerFilters);
            }

            var match = new BsonDocument
            {
                {
                    "$and",
                    filters
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

            var groupSettings = reportSettings
                .Where(x => x.Value.AsBsonDocument.TryGetValue("IsGroupBy", out _))
                .Where(x => x.Value.AsBsonDocument.GetValue("IsGroupBy").AsBoolean);

            if (groupSettings != null && groupSettings.Any())
            {
                var (group, projection) = BuildGroupBySettings(reportSettings);
                pipeline.Add(new BsonDocument("$group", group));
                pipeline.Add(new BsonDocument("$project", projection));
            }

            if (sortingParams != null && sortingParams.Any())
            {
                var sorter = BuildCustomSorterDefinition(sortingParams);
                pipeline.Add(new BsonDocument("$sort", sorter));
            }

            pipeline.Add(
                new BsonDocument(
                    "$facet",
                    new BsonDocument
                    {
                        { "reportItems", new BsonArray { new BsonDocument("$skip", skipCount), new BsonDocument("$limit", maxResultCount) } },
                        { "totalCount", new BsonArray { new BsonDocument("$count", "totalCount") } }
                    }
                ));

            var paginatedResults = await reportItemCollection.Aggregate<BsonDocument>(pipeline.ToArray()).ToListAsync();
            paginatedResults.FirstOrDefault().TryGetValue("totalCount", out var totalBsonValue);
            paginatedResults.FirstOrDefault().TryGetValue("reportItems", out var reportItemsAsBsonValue);
            var total = totalBsonValue.AsBsonArray.Select(x => x.AsBsonDocument.GetValue("totalCount").AsInt32).FirstOrDefault();
            var reportItems = reportItemsAsBsonValue.AsBsonArray.Select(x => x.AsBsonDocument).ToList();
            return (total, reportItems);
        }

        private static (BsonDocument, BsonDocument) BuildGroupBySettings(List<BsonElement> reportSettings)
        {
            var groupBySettings = reportSettings
                .Where(x => x.Value.AsBsonDocument.TryGetValue("IsGroupBy", out _))
                .Where(x => x.Value.AsBsonDocument.GetValue("IsGroupBy").AsBoolean);

            var notGroupBySettings = reportSettings.Except(groupBySettings);

            var groupIds = groupBySettings
                .ToDictionary(g => g.Name, g => $"${g.Name}")
                .ToBsonDocument();
            var groupBy = new BsonDocument("_id", groupIds);

            foreach (var setting in notGroupBySettings)
            {
                var bsonElement = new BsonElement(setting.Name, new BsonDocument("$first", $"${setting.Name}"));
                if (setting.Value.AsBsonDocument.GetValue("DataType").AsInt32 == (int)ReportItemDataType.Number)
                {
                    bsonElement = new BsonElement(setting.Name, new BsonDocument("$sum", $"${setting.Name}"));
                }
                groupBy.Add(bsonElement);
            }
            groupBy.Add(new BsonElement("DateOfData", new BsonDocument("$first", "$DateOfData")));

            var projections = groupBySettings
                .ToDictionary(x => x.Name, x => $"$_id.{x.Name}")
                .ToBsonDocument();
            var notGroupByProjections = notGroupBySettings.Select(x => new BsonElement(x.Name, $"${x.Name}"));
            projections.AddRange(notGroupByProjections);
            projections.Add(new BsonElement("DateOfData", "$DateOfData"));
            return (groupBy, projections);
        }

        private static BsonDocument BuildCustomSorterDefinition(Dictionary<string, string> sortingParams)
        {
            var sorter = new BsonDocument();
            foreach (var sorting in sortingParams)
            {
                if (sorting.Value.Equals("asc")) sorter = sorter.Add(sorting.Key, 1);
                else if (sorting.Value.Equals("desc")) sorter = sorter.Add(sorting.Key, -1);
            }
            return sorter;
        }

        private static BsonArray BuildCustomFilterDefinition(Dictionary<string, object> customParams)
        {
            if (customParams == null || !customParams.Any()) return null;
            var customFilters = new BsonArray();
            foreach (var paramValue in customParams)
            {
                if (paramValue.Key.Contains("_from"))
                {
                    var paramKey = paramValue.Key.RemovePostFix("_from");
                    customFilters.Add(new BsonDocument(
                            paramKey,
                            new BsonDocument("$gte", (DateTime)paramValue.Value)));
                    continue;
                }
                if (paramValue.Key.Contains("_to"))
                {
                    var paramKey = paramValue.Key.RemovePostFix("_to");
                    customFilters.Add(new BsonDocument(
                            paramKey,
                            new BsonDocument("$lte", (DateTime)paramValue.Value)));
                    continue;
                }
                if (paramValue.Key.Contains("_ct"))
                {
                    var paramKey = paramValue.Key.RemovePostFix("_ct");
                    customFilters.Add(new BsonDocument(
                            paramKey,
                            new BsonDocument("$regex", paramValue.Value?.ToString())));
                    continue;
                }
                customFilters.Add(new BsonDocument(
                            paramValue.Key,
                            new BsonDocument("$eq", (double)paramValue.Value)));
            }
            return customFilters;
        }
    }
}
