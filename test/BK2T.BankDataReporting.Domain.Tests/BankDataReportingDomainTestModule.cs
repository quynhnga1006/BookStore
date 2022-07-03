using BK2T.BankDataReporting.MongoDB;
using Volo.Abp.Modularity;

namespace BK2T.BankDataReporting
{
    [DependsOn(
        typeof(BankDataReportingMongoDbTestModule)
        )]
    public class BankDataReportingDomainTestModule : AbpModule
    {

    }
}