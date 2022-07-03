using MongoDB.Bson;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public class Report : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public BsonDocument Setting { get; set; }
        private Report()
        {
            Setting = new BsonDocument();
        }
        public Report(Guid id, string name, BsonDocument setting) : base (id)
        {
            Name = name;
            Setting = setting;
        }
    }
}
