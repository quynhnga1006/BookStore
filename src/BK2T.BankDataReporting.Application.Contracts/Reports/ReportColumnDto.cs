using BK2T.BankDataReporting.ReportFiles;
using System;
using System.ComponentModel.DataAnnotations;

namespace BK2T.BankDataReporting.Reports
{
    public class ReportColumnDto
    {
        [Required]
        public Guid ReportId { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
    }
}
