using BK2T.BankDataReporting.ReportFiles;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public class TemplateDto
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public ReportItemDataType DataType { get; set; }
    }
}
