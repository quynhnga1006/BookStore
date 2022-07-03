using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BK2T.BankDataReporting.Departments
{
    public class Department : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; private set; }
        public string Name { get; set; }
        public string OldCode { get; set; }
        public IEnumerable<string> CustomerSegments { get; set; }
        public IEnumerable<TargetPlan> TargetPlans { get; set; }
        public IEnumerable<ManualCapital> ManualCapitals { get; set; }

        internal Department() { }

        internal Department(
            Guid id,
            [NotNull] string code,
            [NotNull] string name,
            string oldCode,
            IEnumerable<string> customerSegments,
            IEnumerable<TargetPlan> targetPlans,
            IEnumerable<ManualCapital> manualCapitals)
            : base(id)
        {
            SetCode(code);
            Name = name;
            OldCode = oldCode;
            CustomerSegments = customerSegments;
            TargetPlans = targetPlans;
            ManualCapitals = manualCapitals;
        }

        
        internal Department(
            Guid id,
            [NotNull] string code,
            [NotNull] string name,
            [NotNull] string oldCode,
            List<string> customerSegments,
            List<TargetPlan> targetPlans,
            List<ManualCapital> manualCapitals
            )
            : base(id)
        {
            SetCode(code);
            Name = name;
            OldCode = oldCode;
            CustomerSegments = customerSegments;
            TargetPlans = targetPlans;
            ManualCapitals = manualCapitals;
        }

        internal Department ChangeCode([NotNull] string code)
        {
            SetCode(code);
            return this;
        }

        private void SetCode([NotNull] string code)
        {
            Code = Check.NotNullOrWhiteSpace(
                code,
                nameof(code),
                maxLength: DepartmentConst.CodeMaxLength
            );
        }

    }
}