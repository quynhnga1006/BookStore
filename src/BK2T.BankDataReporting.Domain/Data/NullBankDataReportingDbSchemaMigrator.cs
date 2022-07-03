using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace BK2T.BankDataReporting.Data
{
    /* This is used if database provider does't define
     * IBankDataReportingDbSchemaMigrator implementation.
     */
    public class NullBankDataReportingDbSchemaMigrator : IBankDataReportingDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}