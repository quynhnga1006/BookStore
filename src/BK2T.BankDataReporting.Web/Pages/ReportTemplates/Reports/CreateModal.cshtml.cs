using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using Microsoft.AspNetCore.Mvc;

namespace BK2T.BankDataReporting.Web.Pages.ReportTemplates.Reports
{
    public class CreateModalModel : BankDataReportingPageModel
    {
        private readonly IReportTemplateAppService _reportTemplateAppService;
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public ReportType ReportType { get; set; }
        [BindProperty]
        public CreateReportViewModel Report { get; set; }
        public async void OnGet()
        {
            Report = new CreateReportViewModel();
            var templates = await _reportTemplateAppService.GetTemplatesOfReportTemplateAsync(ReportType);
            foreach (var template in templates.Items)
            {
                Report.AllSettings.Add(new SettingViewModel
                {
                    Name = template.Key,
                    IsVisible = false,
                    IsQueryable = false,
                    IsGroupBy = false,
                    Label = template.Label,
                    DataType = template.DataType
                });
            }
            var NIITemplates = await _reportTemplateAppService.GetTemplatesOfReportTemplateAsync(ReportType.NII);
            foreach (var niiTemplate in NIITemplates.Items)
            {
                Report.NIISettings.Add(new SettingViewModel
                {
                    Name = niiTemplate.Key,
                    IsVisible = false,
                    IsQueryable = false,
                    Label = niiTemplate.Label,
                    DataType = niiTemplate.DataType
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
            await _reportTemplateAppService.AddReportToReportTemplateAsync(new CreateEditReportDto { ReportType = ReportType, Name = Report.Name, Setting = settingDict });
            return NoContent();
        }
        public CreateModalModel(IReportTemplateAppService reportTemplateAppService)
        {
            _reportTemplateAppService = reportTemplateAppService;
        }
        public class CreateReportViewModel
        {
            [Required]
            [Display(Name = "Reports:Name")]
            public string Name { get; set; }
            public List<SettingViewModel> AllSettings { get; set; }
            public List<SettingViewModel> NIISettings { get; set; }
            public CreateReportViewModel()
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
