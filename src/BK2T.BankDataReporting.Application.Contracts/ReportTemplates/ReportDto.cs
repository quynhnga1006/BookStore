using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public class ReportDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public Dictionary<string, object> Setting { get; set; }
    }
}
