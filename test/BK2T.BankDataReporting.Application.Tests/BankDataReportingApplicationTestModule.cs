using Volo.Abp.Modularity;

namespace BK2T.BankDataReporting
{
    [DependsOn(
        typeof(BankDataReportingApplicationModule),
        typeof(BankDataReportingDomainTestModule)
        )]
    public class BankDataReportingApplicationTestModule : AbpModule
    {

    }
}