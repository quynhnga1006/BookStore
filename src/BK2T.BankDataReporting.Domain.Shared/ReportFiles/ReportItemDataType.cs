using System.ComponentModel.DataAnnotations;

namespace BK2T.BankDataReporting.ReportFiles
{
    public enum ReportItemDataType
    {
        [Display(Name = "Settings:String")]
        String,
        [Display(Name = "Settings:Number")]
        Number,
        [Display(Name = "Settings:Date")]
        Date,
        [Display(Name = "Settings:DateRange")]
        DateRange,
        [Display(Name = "Settings:Boolean")]
        Boolean
    }
}
