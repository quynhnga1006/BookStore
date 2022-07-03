using BK2T.BankDataReporting.TargetPlans;
using System;
using System.Collections.Generic;

namespace BK2T.BankDataReporting.Departments
{
    public class TargetPlanRequestDto
    {
        public Guid DepartmentId { get; set; }
        public PlanType PlanType { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public double YearTarget { get; set; }
        public List<double> MonthTarget { get; set; }
        public string Year { get; set; }
    }
}
