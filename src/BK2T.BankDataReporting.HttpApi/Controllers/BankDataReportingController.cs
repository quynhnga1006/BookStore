using BK2T.BankDataReporting.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace BK2T.BankDataReporting.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class BankDataReportingController : AbpController
    {
        protected BankDataReportingController()
        {
            LocalizationResource = typeof(BankDataReportingResource);
        }
    }
}