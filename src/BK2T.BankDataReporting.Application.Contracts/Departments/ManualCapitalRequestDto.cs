using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using System;
using System.Collections.Generic;

namespace BK2T.BankDataReporting.Departments
{
    public class ManualCapitalRequestDto
    {
        public Guid DepartmentId { get; set; }
        public CustomerType CustomerType { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public string Year { get; set; }
        public List<double> MonthsCapital { get; set; }
    }
}