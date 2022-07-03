using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.Reports;
using BK2T.BankDataReporting.ReportTemplates;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace BK2T.BankDataReporting.MongoDB
{
    [ConnectionStringName("Default")]
    public class BankDataReportingMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<Department> Departments => Collection<Department>();
        public IMongoCollection<ReportFile> ReportFiles => Collection<ReportFile>();
        public IMongoCollection<ReportItem> ReportItems => Collection<ReportItem>();
        public IMongoCollection<ReportTemplate> ReportTemplates => Collection<ReportTemplate>();
        public IMongoCollection<NiiItem> NIIItems => Collection<NiiItem>();
        public IMongoCollection<CollateralItem> CollateralItems => Collection<CollateralItem>();
        public IMongoCollection<ProvisionItem> ProvisionItems => Collection<ProvisionItem>();
        public IMongoCollection<CustomerSalaryItem> CustomerSalaryItems => Collection<CustomerSalaryItem>();
        public IMongoCollection<InsuranceItem> InsuranceItems => Collection<InsuranceItem>();
        public IMongoCollection<DebtDueCustomerItem> DebtDueCustomerItems => Collection<DebtDueCustomerItem>();
        public IMongoCollection<IPayCustomerItem> IPayCustomerItems => Collection<IPayCustomerItem>();
        public IMongoCollection<PersonalCustomerProductItem> PersonalCustomerProductItems => Collection<PersonalCustomerProductItem>();
        public IMongoCollection<ForeignCurrencyTradingProfitItem> ForeignCurrencyTradingProfitItems => Collection<ForeignCurrencyTradingProfitItem>();
        public IMongoCollection<CorporateCustomerItem> CorporateCustomerItems => Collection<CorporateCustomerItem>();
        public IMongoCollection<CreditCardItem> CreditCardItems => Collection<CreditCardItem>();
        public IMongoCollection<RetailDevelopmentCustomerItem> RetailDevelopmentCustomerItems => Collection<RetailDevelopmentCustomerItem>();
        public IMongoCollection<EFastCustomerItem> EFastCustomerItems => Collection<EFastCustomerItem>();
        public IMongoCollection<CardAcceptingUnitItem> CardAcceptingUnitItems => Collection<CardAcceptingUnitItem>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<Department>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "Departments";
            });

            modelBuilder.Entity<ReportFile>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "ReportFiles";
            });

            modelBuilder.Entity<ReportTemplate>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "ReportTemplates";
            });

            modelBuilder.Entity<ReportItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "ReportItems";
            });

            modelBuilder.Entity<NiiItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "NiiItems";
            });

            modelBuilder.Entity<DebtDueCustomerItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "DebtDueCustomerItems";
            });

            modelBuilder.Entity<CollateralItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "CollateralItems";
            });

            modelBuilder.Entity<ProvisionItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "ProvisionItems";
            });

            modelBuilder.Entity<CustomerSalaryItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "CustomerSalaryItem";
            });

            modelBuilder.Entity<InsuranceItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "InsuranceItems";
            });

            modelBuilder.Entity<IPayCustomerItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "IPayCustomerItems";
            });
            modelBuilder.Entity<PersonalCustomerProductItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "PersonalCustomerProductItems";
            });

            modelBuilder.Entity<ForeignCurrencyTradingProfitItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "ForeignCurrencyTradingProfitItems";
            });

            modelBuilder.Entity<EFastCustomerItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "EFastCustomerItems";
            });

            modelBuilder.Entity<CorporateCustomerItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "CorporateCustomerItems";
            });

            modelBuilder.Entity<CreditCardItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "CreditCardItems";
            });

            modelBuilder.Entity<CardAcceptingUnitItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "CardAcceptingUnitItems";
            });
            modelBuilder.Entity<RetailDevelopmentCustomerItem>(b =>
            {
                b.CollectionName = BankDataReportingConsts.DbTablePrefix + "RetailDevelopmentCustomerItems";
            });
        }
    }
}