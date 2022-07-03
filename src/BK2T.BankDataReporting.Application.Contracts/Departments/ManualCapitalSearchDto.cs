using BK2T.BankDataReporting.Departments.ManualCapitals;
using System;

namespace BK2T.BankDataReporting.Departments
{
    public class ManualCapitalSearchDto
    {
        public Guid DepartmentId { get; set; }
        public string Year { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
    }
}