using System.ComponentModel.DataAnnotations;

namespace BK2T.BankDataReporting.TargetPlans
{
    public enum UnitMeasure
    {
        [Display(Name = "TargetPlans:UnitMeasure:0")]
        Billion,

        [Display(Name = "TargetPlans:UnitMeasure:1")]
        Million,

        [Display(Name = "TargetPlans:UnitMeasure:2")]
        NumberOfCustomer,
    }
}