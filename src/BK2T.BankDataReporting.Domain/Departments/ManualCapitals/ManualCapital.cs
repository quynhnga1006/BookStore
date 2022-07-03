using BK2T.BankDataReporting.TargetPlans;
using System.Collections.Generic;

namespace BK2T.BankDataReporting.Departments.ManualCapitals
{
    public class ManualCapital
    {
        public CustomerType CustomerType { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public List<string> CustomerSegments { get; set; }
        public Dictionary<string, List<double>> MonthsCapital { get; set; }
        public ManualCapital(CustomerType customerType, UnitMeasure unitMeasure, List<string> customerSegments)
        {
            CustomerType = customerType;
            UnitMeasure = unitMeasure;
            CustomerSegments = customerSegments;
            MonthsCapital = new Dictionary<string, List<double>>();
        }
    }
}