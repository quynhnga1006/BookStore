using BK2T.BankDataReporting.ReportFiles;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public class SettingDto
    {
        public string Label { get; set; }
        public bool IsVisible { get; set; }
        public bool IsQueryable { get; set; }
        public ReportItemDataType DataType { get; set; }
        public bool IsGroupBy { get; set; }
    }
    public class NIISettingDto : SettingDto
    {
        public bool IsNII { get; private set; } = true;
    }
}