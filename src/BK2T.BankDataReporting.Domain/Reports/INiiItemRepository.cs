using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace BK2T.BankDataReporting.Reports
{
    public interface INiiItemRepository : IRepository<NiiItem, Guid>
    {
        Task<(long total, List<BsonDocument> reportItems)> GetNiiItemsByGroupBySetting(
            List<BsonElement> groupBySettings,
            List<BsonElement> niiSettings,
            DateTime reportDate);
    }
}