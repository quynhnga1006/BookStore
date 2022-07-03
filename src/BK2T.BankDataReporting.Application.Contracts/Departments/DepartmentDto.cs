using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace BK2T.BankDataReporting.Departments
{
    public class DepartmentDto : EntityDto<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string OldCode { get; set; }
        public IEnumerable<string> CustomerSegments { get; set; }
    }
}
