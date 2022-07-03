using BK2T.BankDataReporting.TargetPlans;
using System;

namespace BK2T.BankDataReporting.Departments
{
    public class TargetPlanSearchDto
    {
        public Guid DepartmentId { get; set; }
        public PlanType PlanType { get; set; }
        public string Year { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
    }
}