using System.Collections.Generic;

namespace BK2T.BankDataReporting.Departments
{
    public class DepartmentRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string OldCode { get; set; }
        public IEnumerable<string> CustomerSegments { get; set; }
    }
}
