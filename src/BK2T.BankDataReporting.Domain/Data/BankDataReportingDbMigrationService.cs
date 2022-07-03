using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace BK2T.BankDataReporting.Data
{
    public class BankDataReportingDbMigrationService : ITransientDependency
    {
        public ILogger<BankDataReportingDbMigrationService> Logger { get; set; }

        private readonly IDataSeeder _dataSeeder;
        private readonly IEnumerable<IBankDataReportingDbSchemaMigrator> _dbSchemaMigrators;

        public BankDataReportingDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IBankDataReportingDbSchemaMigrator> dbSchemaMigrators)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;

            Logger = NullLogger<BankDataReportingDbMigrationService>.Instance;
        }

        public async Task MigrateAsync()
        {
            Logger.LogInformation("Started database migrations...");

            await MigrateDatabaseSchemaAsync();
            await SeedDataAsync();

            Logger.LogInformation($"Successfully completed host database migrations.");
            Logger.LogInformation("Successfully completed all database migrations.");
            Logger.LogInformation("You can safely end this process...");
        }

        private async Task MigrateDatabaseSchemaAsync()
        {
            Logger.LogInformation(
                $"Migrating schema for host database...");

            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
        }

        private async Task SeedDataAsync()
        {
            Logger.LogInformation($"Executing host database seed...");

            await _dataSeeder.SeedAsync(new DataSeedContext(null)
                .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
                .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, IdentityDataSeedContributor.AdminPasswordDefaultValue)
            );
        }
    }
}