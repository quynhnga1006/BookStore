using BK2T.BankDataReporting.TargetPlans;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace BK2T.BankDataReporting.Departments
{
    public class TargetPlanDto : EntityDto<Guid>
    {
        public PlanType PlanType { get; set; }
        public UnitMeasure UnitMeasure { get; set; }
        public IEnumerable<TargetPlanDto> TargetPlans { get; set; }
        public double YearTarget { get; set; }
        public List<double> MonthTarget { get; set; }
    }
}