using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BK2T.BankDataReporting.ReportFiles
{
    public class ReportFile : FullAuditedAggregateRoot<Guid>
    {
        public DateTime ReportDate { get; set; }
        public ReportType ReportType { get; set; }
        public ReportFileStatus ReportFileStatus { get; set; }
        public string FileData { get; set; }

        private ReportFile() { }

        public ReportFile(
            Guid id,
            [NotNull] DateTime reportDate,
            [NotNull] ReportType reportType,
            [NotNull] ReportFileStatus reportFileStatus,
            [NotNull] string fileData)
            : base(id)
        {
            ReportDate = reportDate;
            ReportType = reportType;
            ReportFileStatus = reportFileStatus;
            FileData = fileData;
        }
    }
}
