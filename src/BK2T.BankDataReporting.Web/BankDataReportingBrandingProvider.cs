using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace BK2T.BankDataReporting.Web
{
    [Dependency(ReplaceServices = true)]
    public class BankDataReportingBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "BankDataReporting";
    }
}
