using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BK2T.BankDataReporting.ReportFiles;

namespace BK2T.BankDataReporting.Reports
{
    public class ReportSearchDto
    {
        [Required]
        public Guid ReportId { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
        public DateTime? ReportDate { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? UserId { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
        public string Sorting { private get; set; }
        public Dictionary<string, string> SortingParams
        {
            get
            {
                if (string.IsNullOrEmpty(Sorting?.Trim())) return null;
                var sortParams = Sorting.Split(" ");
                if (sortParams.Length < 2) return null;
                return new Dictionary<string, string>() { { sortParams[0], sortParams[1] } };
            }
        }
        public Dictionary<string, object> CustomParams { get; set; }
    }
}