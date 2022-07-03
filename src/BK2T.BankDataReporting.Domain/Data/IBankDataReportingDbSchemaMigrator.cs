using System.Threading.Tasks;

namespace BK2T.BankDataReporting.Data
{
    public interface IBankDataReportingDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
