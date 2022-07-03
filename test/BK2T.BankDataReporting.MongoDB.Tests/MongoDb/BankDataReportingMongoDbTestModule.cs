using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace BK2T.BankDataReporting.MongoDB
{
    [DependsOn(
        typeof(BankDataReportingTestBaseModule),
        typeof(BankDataReportingMongoDbModule)
        )]
    public class BankDataReportingMongoDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var stringArray = BankDataReportingMongoDbFixture.ConnectionString.Split('?');
                        var connectionString = stringArray[0].EnsureEndsWith('/')  +
                                                   "Db_" +
                                               Guid.NewGuid().ToString("N") + "/?" + stringArray[1];

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}
