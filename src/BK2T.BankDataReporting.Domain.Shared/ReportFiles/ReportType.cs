using System.ComponentModel.DataAnnotations;

namespace BK2T.BankDataReporting.ReportFiles
{
    public enum ReportType
    {
        [Display(Name = "ReportFiles:ReportType:Deposit")]
        Deposit,

        [Display(Name = "ReportFiles:ReportType:Loan")]
        Loan,

        [Display(Name = "ReportFiles:ReportType:NII")]
        NII,

        [Display(Name = "ReportFiles:ReportType:DebtDueCustomer")]
        DebtDueCustomer,

        [Display(Name = "ReportFiles:ReportType:Collateral")]
        Collateral,

        [Display(Name = "ReportFiles:ReportType:Provision")]
        Provision,

        [Display(Name = "ReportFiles:ReportType:CustomerSalary")]
        CustomerSalary,

        [Display(Name = "ReportFiles:ReportType:LifeInsurance")]
        LifeInsurance,

        [Display(Name = "ReportFiles:ReportType:NonLifeInsurance")]
        NonLifeInsurance,

        [Display(Name = "ReportFiles:ReportType:IPayCustomer")]
        IpayCustomer,

        [Display(Name = "ReportFiles:ReportType:PersonalCustomerProduct")]
        PersonalCustomerProduct,

        [Display(Name = "ReportFiles:ReportType:ForeignCurrencyTradingProfit")]
        ForeignCurrencyTradingProfit,

        [Display(Name = "ReportFiles:ReportType:EFastCustomerItem")]
        EFastCustomerItem,

        [Display(Name = "ReportFiles:ReportType:CorporateCustomer")]
        CorporateCustomer,

        [Display(Name = "ReportFiles:ReportType:CreditCard")]
        CreditCard,

        [Display(Name = "ReportFiles:ReportType:CardAcceptingUnit")]
        CardAcceptingUnit,

        [Display(Name = "ReportFiles:ReportType:RetailDevelopmentCustomer")]
        RetailDevelopmentCustomer
    }
}