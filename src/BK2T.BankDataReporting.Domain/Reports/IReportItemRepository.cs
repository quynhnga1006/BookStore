using BK2T.BankDataReporting.ReportFiles;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace BK2T.BankDataReporting.Reports
{
    public interface IReportItemRepository : IRepository<ReportItem, Guid>
    {
        Task<(long total, List<BsonDocument> reportItems)> GetReportItemsByFilter(
            List<BsonElement> reportSettings,
            ReportType reportType,
            DateTime? reportDate,
            Guid? departmentId,
            Guid? userId,
            Dictionary<string, object> customParams,
            Dictionary<string, string> sortingParams,
            int skipCount = 0,
            int maxResultCount = 200);
    }
}