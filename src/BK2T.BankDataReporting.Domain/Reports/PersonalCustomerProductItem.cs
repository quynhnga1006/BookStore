using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BK2T.BankDataReporting.Reports
{
    public class PersonalCustomerProductItem : CreationAuditedAggregateRoot<Guid>
    {
        public Guid ReportFileId { get; set; }
        public Guid? DepartmentId { get; set; }
        public int ReportType { get; set; }
        public DateTime DateOfData { get; set; }
        [BsonExtraElements]
        public BsonDocument ReportData { get; set; }
    }
}
