using BK2T.BankDataReporting.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BK2T.BankDataReporting.Web.Pages
{
    public abstract class BankDataReportingPageModel : AbpPageModel
    {
        protected BankDataReportingPageModel()
        {
            LocalizationResourceType = typeof(BankDataReportingResource);
        }
    }
}