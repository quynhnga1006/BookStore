using BK2T.BankDataReporting.ReportFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public class CreateEditReportDto
    {
        public ReportType ReportType { get; set; }
        public string Name { get; set; }
        public Dictionary<string, SettingDto> Setting { get; set; }
    }
}