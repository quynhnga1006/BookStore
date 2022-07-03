using BK2T.BankDataReporting.Localization;
using Volo.Abp.Application.Services;

namespace BK2T.BankDataReporting
{
    public abstract class BankDataReportingAppService : ApplicationService
    {
        protected BankDataReportingAppService()
        {
            LocalizationResource = typeof(BankDataReportingResource);
        }
    }
}
