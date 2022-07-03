using System.ComponentModel.DataAnnotations;

namespace BK2T.BankDataReporting.TargetPlans
{
    public enum PlanType
    {
        [Display(Name = "TargetPlans:PlanType:0")]
        CreditBalance,

        [Display(Name = "TargetPlans:PlanType:1")]
        BigEnterpriseCreditBalance,

        [Display(Name = "TargetPlans:PlanType:2")]
        MediumEnterpriseCreditBalance,

        [Display(Name = "TargetPlans:PlanType:3")]
        FDIEnterpriseCreditBalance,

        [Display(Name = "TargetPlans:PlanType:4")]
        PersonalCreditBalance,

        [Display(Name = "TargetPlans:PlanType:5")]
        Capital,

        [Display(Name = "TargetPlans:PlanType:6")]
        BigEnterpriseCapital,

        [Display(Name = "TargetPlans:PlanType:7")]
        MediumEnterpriseCapital,

        [Display(Name = "TargetPlans:PlanType:8")]
        FDIEnterpriseCapital,

        [Display(Name = "TargetPlans:PlanType:9")]
        PersonalCreditCapital,

        [Display(Name = "TargetPlans:PlanType:10")]
        FinancialCapital,

        [Display(Name = "TargetPlans:PlanType:11")]
        CASACapital,

        [Display(Name = "TargetPlans:PlanType:12")]
        BigEnterpriseCASACapital,

        [Display(Name = "TargetPlans:PlanType:13")]
        MediumEnterpriseCASACapital,

        [Display(Name = "TargetPlans:PlanType:14")]
        FDIEnterpriseCASACapital,

        [Display(Name = "TargetPlans:PlanType:15")]
        PersonalCreditCASACapital,

        [Display(Name = "TargetPlans:PlanType:16")]
        FinancialCASACapital,

        [Display(Name = "TargetPlans:PlanType:17")]
        NII,

        [Display(Name = "TargetPlans:PlanType:18")]
        CreditNII,

        [Display(Name = "TargetPlans:PlanType:19")]
        CapitalNII,

        [Display(Name = "TargetPlans:PlanType:20")]
        ServiceFee,

        [Display(Name = "TargetPlans:PlanType:21")]
        ProfitOfTradeMoney,

        [Display(Name = "TargetPlans:PlanType:22")]
        TotalProfit,

        [Display(Name = "TargetPlans:PlanType:23")]
        InsuranceSale,

        [Display(Name = "TargetPlans:PlanType:24")]
        LifeInsuranceSale,

        [Display(Name = "TargetPlans:PlanType:25")]
        NonLifeInsuranceSale,

        [Display(Name = "TargetPlans:PlanType:26")]
        CustomerGoal,

        [Display(Name = "TargetPlans:PlanType:27")]
        EnterpriseCustomerGoal_small,

        [Display(Name = "TargetPlans:PlanType:28")]
        EnterpriseCustomerGoal_big,

        [Display(Name = "TargetPlans:PlanType:29")]
        ExcellenceCustomerGoal,

        [Display(Name = "TargetPlans:PlanType:30")]
        ExcellenceCustomerGoal_5,

        [Display(Name = "TargetPlans:PlanType:31")]
        PersonalCustomerGoal,

        [Display(Name = "TargetPlans:PlanType:32")]
        CrossSellingGoal,

        [Display(Name = "TargetPlans:PlanType:33")]
        IPayCustomer,

        [Display(Name = "TargetPlans:PlanType:34")]
        EFastCustomer,

        [Display(Name = "TargetPlans:PlanType:35")]
        KTBLCustomer,

        [Display(Name = "TargetPlans:PlanType:36")]
        EnterpriseCustomerPaidSalary,

        [Display(Name = "TargetPlans:PlanType:37")]
        StaffRecieveSalary,

        [Display(Name = "TargetPlans:PlanType:38")]
        ActiveCredit,

        [Display(Name = "TargetPlans:PlanType:39")]
        AcceptCreditUnit,

    }
}