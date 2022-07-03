using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BK2T.BankDataReporting.Web.Pages.ManualCapitals
{
    public class EditModalModel : BankDataReportingPageModel
    {
        private readonly IDepartmentAppService _departmentAppService;
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid DepartmentId { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public CustomerType CustomerType { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Year { get; set; }

        [BindProperty]
        public UpdateManualCapitalViewModel ManualCapital { get; set; }

        public List<string> Label { get; set; }

        public EditModalModel(IDepartmentAppService departmentAppService)
        {
            _departmentAppService = departmentAppService;
        }
        public async Task OnGetAsync()
        {
            ManualCapital = new();
            var manualCapital = await _departmentAppService.GetManualCapitalAsync(DepartmentId, CustomerType, Year);

            ManualCapital.MonthsCapital = new List<double>();
            for (int i = 0; i < manualCapital.MonthsCapital.Count; i++)
            {
                ManualCapital.MonthsCapital.Add(manualCapital.MonthsCapital[i]);
            }
            ManualCapital.DepartmentId = DepartmentId;
            ManualCapital.CustomerType = CustomerType;
            ManualCapital.UnitMeasure = manualCapital.UnitMeasure;
            ManualCapital.Year = Year;

            Label = new List<string>();
            for (int i = 1; i <= ManualCapital.MonthsCapital.Count; i++)
            {
                Label.Add(L["ManualCapitals:MonthCapital:00004", i]);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var manualCapitalRequestDto = ObjectMapper.Map<UpdateManualCapitalViewModel, ManualCapitalRequestDto>(ManualCapital);
            await _departmentAppService.UpdateManualCapitalAsync(manualCapitalRequestDto);
            return NoContent();
        }
        public class UpdateManualCapitalViewModel
        {
            [HiddenInput]
            public Guid DepartmentId { get; set; }

            [HiddenInput]
            public CustomerType CustomerType { get; set; }

            [HiddenInput]
            public UnitMeasure UnitMeasure { get; set; }

            [HiddenInput]
            public string Year { get; set; }

            public List<double> MonthsCapital { get; set; }
        }
    }
}