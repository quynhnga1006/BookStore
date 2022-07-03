using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BK2T.BankDataReporting.Reports
{
    public class NiiItem : CreationAuditedAggregateRoot<Guid>
    {
        public Guid? DepartmentId { get; set; }
        public Guid? UserId { get; set; }
        public Guid ReportFileId { get; set; }
        public int ReportType { get; set; }
        public DateTime DateOfData { get; set; }
        public int MonthOfData { get; set; }
        public int YearOfData { get; set; }
        public string AccountNumber { get; set; }
        [BsonExtraElements]
        public BsonDocument ReportData { get; set; }
    }
}