using System.ComponentModel.DataAnnotations;

namespace BK2T.BankDataReporting.Departments.ManualCapitals
{
    public enum CustomerType
    {
        [Display(Name = "ManualCapitals:CustomerType:0")]
        BigEnterpriseCustomer,

        [Display(Name = "ManualCapitals:CustomerType:1")]
        MediumEnterpriseCustomer,

        [Display(Name = "ManualCapitals:CustomerType:2")]
        FDIEnterpriseCustomer,

        [Display(Name = "ManualCapitals:CustomerType:3")]
        PersonalCustomer,

        [Display(Name = "ManualCapitals:CustomerType:4")]
        FinancialCustomer,
    }
}