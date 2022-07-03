using System.ComponentModel.DataAnnotations;

namespace BK2T.BankDataReporting.ReportFiles
{
    public enum ReportFileStatus
    {
        [Display(Name = "ReportFiles:Status:Uploaded")]
        Uploaded,

        [Display(Name = "ReportFiles:Status:Imported")]
        Imported,
    }
}
