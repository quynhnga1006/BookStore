using BK2T.BankDataReporting.ReportFiles;
using System;

namespace BK2T.BankDataReporting.Reports
{
    public class DataImportingArgs
    {
        public Guid ReportFileId { get; set; }
        public string ReportFileName { get; set; }
        public DateTime DateOfData { get; set; }
        public ReportType ReportType { get; set; }
    }
}
