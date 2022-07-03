using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace BK2T.BankDataReporting.TargetPlans
{
    public class TargetPlan : FullAuditedAggregateRoot<Guid>
    {
        public TargetPlan(Guid id, PlanType planType, UnitMeasure unitMeasure, List<TargetPlan> targetPlans) : base(id)
        {
            PlanType = planType;
            UnitMeasure = unitMeasure;
            YearsTarget = new Dictionary<string, double>();
            MonthsTarget = new Dictionary<string, List<double>>();
            TargetPlans = targetPlans;
        }
        public TargetPlan(Guid id, PlanType planType, UnitMeasure unitMeasure) : base(id)
        {
            PlanType = planType;
            UnitMeasure = unitMeasure;
            YearsTarget = new Dictionary<string, double>();
            MonthsTarget = new Dictionary<string, List<double>>();
        }
        public PlanType PlanType { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public IEnumerable<TargetPlan> TargetPlans { get; set; }
        public Dictionary<string, double> YearsTarget { get; set; }
        public Dictionary<string, List<double>> MonthsTarget { get; set; }
    }
}