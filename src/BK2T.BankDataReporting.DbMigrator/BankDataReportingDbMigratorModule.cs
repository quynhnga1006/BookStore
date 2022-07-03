using BK2T.BankDataReporting.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace BK2T.BankDataReporting.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BankDataReportingMongoDbModule),
        typeof(BankDataReportingApplicationContractsModule)
        )]
    public class BankDataReportingDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
