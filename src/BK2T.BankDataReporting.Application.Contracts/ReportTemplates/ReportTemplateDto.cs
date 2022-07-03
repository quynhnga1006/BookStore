using BK2T.BankDataReporting.ReportFiles;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public class ReportTemplateDto : EntityDto<Guid>
    {
        public ReportType ReportType { get; set; }
        public Dictionary<string, object> Template { get; set; }
        public IList<ReportDto> Reports { get; set; }
    }
}
