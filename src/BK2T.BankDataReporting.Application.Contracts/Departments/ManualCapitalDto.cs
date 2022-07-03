using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using System.Collections.Generic;

namespace BK2T.BankDataReporting.Departments
{
    public class ManualCapitalDto
    {
        public CustomerType CustomerType { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public int Year { get; set; }
        public List<double> MonthsCapital { get; set; }
    }
}