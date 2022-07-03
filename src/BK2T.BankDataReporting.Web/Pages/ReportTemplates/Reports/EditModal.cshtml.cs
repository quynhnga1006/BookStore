using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using Microsoft.AspNetCore.Mvc;

namespace BK2T.BankDataReporting.Web.Pages.ReportTemplates.Reports
{
    public class EditModalModel : BankDataReportingPageModel
    {
        private readonly IReportTemplateAppService _reportTemplateAppService;
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public ReportType ReportType { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        [BindProperty]
        public EditReportViewModel Report { get; set; }
        public async void OnGet()
        {
            Report = new EditReportViewModel();
            var report = await _reportTemplateAppService.GetReportAsync(ReportType, Id);
            Report.Name = report.Name;
            foreach (var item in report.Setting)
            {
                Dictionary<string, object> settingAttribute = (Dictionary<string, object>)item.Value;
                if (settingAttribute.ContainsKey("IsNII"))
                {
                    Report.NIISettings.Add(new SettingViewModel
                    {
                        Name = item.Key,
                        IsVisible = (bool)settingAttribute["IsVisible"],
                        IsQueryable = (bool)settingAttribute["IsQueryable"],
                        DataType = (ReportItemDataType)settingAttribute["DataType"],
                        Label = settingAttribute["Label"] as string,
                    });
                    continue;
                }
                Report.AllSettings.Add(new SettingViewModel
                {
                    Name = item.Key,
                    IsVisible = (bool)settingAttribute["IsVisible"],
                    IsQueryable = (bool)settingAttribute["IsQueryable"],
                    IsGroupBy = (bool)settingAttribute["IsGroupBy"],
                    DataType = (ReportItemDataType)settingAttribute["DataType"],
                    Label = settingAttribute["Label"] as string
                });
            }
        }
        public async Task<IActionResult> OnPost()
        {
            Dictionary<string, SettingDto> settingDict = new();
            foreach (var setting in Report.AllSettings)
            {
                settingDict.Add(setting.Name, new SettingDto()
                {
                    Label = setting.Label,
                    IsVisible = setting.IsVisible,
                    IsQueryable = setting.IsQueryable,
                    IsGroupBy = setting.IsGroupBy,
                    DataType = setting.DataType
                });
            }
            foreach (var setting in Report.NIISettings)
            {
                settingDict.Add(setting.Name, new NIISettingDto()
                {
                    Label = setting.Label,
                    IsVisible = setting.IsVisible,
                    IsQueryable = setting.IsQueryable,
                    DataType = setting.DataType,
                });
            }
            await _reportTemplateAppService.UpdateReportAsync(Id, new CreateEditReportDto { ReportType = ReportType, Name = Report.Name, Setting = settingDict });
            return NoContent();
        }
        public EditModalModel(IReportTemplateAppService reportTemplateAppService)
        {
            _reportTemplateAppService = reportTemplateAppService;
        }
        public class EditReportViewModel
        {
            [Required]
            [Display(Name = "Reports:Name")]
            public string Name { get; set; }
            public List<SettingViewModel> AllSettings { get; set; }
            public List<SettingViewModel> NIISettings { get; set; }
            public EditReportViewModel()
            {
                AllSettings = new List<SettingViewModel>();
                NIISettings = new List<SettingViewModel>();
            }
        }
        public class SettingViewModel
        {
            [HiddenInput]
            public string Name { get; set; }
            [Display(Name = "Settings:IsVisible")]
            public bool IsVisible { get; set; }
            [Display(Name = "Settings:IsQueryable")]
            public bool IsQueryable { get; set; }
            [Display(Name = "Settings:IsGroupBy")]
            public bool IsGroupBy { get; set; }
            [Display]
            public ReportItemDataType DataType { get; set; }
            [HiddenInput]
            public string Label { get; set; }
        }
    }
}
