using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BK2T.BankDataReporting.ReportFiles
{
    public class ReportFileDto : EntityDto<Guid>
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ReportDate { get; set; }
        public ReportType ReportType { get; set; }
        public string ReportTypeCode { get; set; }
        public string FileData { get; set; }
        public string ReportStatusCode { get; set; }
        public string CreatedByUsername { get; set; }
        public Guid CreatedByUserId { get; set; }
    }
}
