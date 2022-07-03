using BK2T.BankDataReporting.ReportFiles;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public class ReportTemplate : FullAuditedAggregateRoot<Guid>
    {
        public ReportType ReportType { get; private set; }
        public BsonDocument Template { get; set; }
        public List<Report> Reports { get; set; }
        public ReportTemplate(Guid id, ReportType reportType, BsonDocument template) : base(id)
        {
            ReportType = reportType;
            Template = template;
            Reports = new List<Report>();
        }
    }
}
