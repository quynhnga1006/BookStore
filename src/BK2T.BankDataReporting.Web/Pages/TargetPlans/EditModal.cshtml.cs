using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.TargetPlans;
using Microsoft.AspNetCore.Mvc;

namespace BK2T.BankDataReporting.Web.Pages.TargetPlans
{
    public class EditModalModel : BankDataReportingPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid DepartmentId { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet =true)]
        public PlanType PlanType { get; set; }
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string Year { get; set; }
        [BindProperty]
        public UpdateTargetPlanViewModel TargetPlan { get; set; }

        public List<string> Label { get; set; }

        private readonly IDepartmentAppService _departmentService;
        public EditModalModel(IDepartmentAppService departmentService)
        {
            _departmentService = departmentService;
        }
        public async void OnGetAsync()
        {
            TargetPlan = new UpdateTargetPlanViewModel();
            var targetPlan = await _departmentService.GetTargetPlanAsync(DepartmentId, PlanType, Year);
            TargetPlan.YearTarget = targetPlan.YearTarget;

            TargetPlan.MonthTargets = new List<double>();
            for (int i = 0; i < targetPlan.MonthTarget.Count; i++)
            {
                TargetPlan.MonthTargets.Add(targetPlan.MonthTarget[i]);
            }
            TargetPlan.DepartmentId = DepartmentId;
            TargetPlan.Year = Year;
            TargetPlan.UnitMeasure = targetPlan.UnitMeasure;
            TargetPlan.PlanType = targetPlan.PlanType;
            
            Label = new List<string>();
            for(int i =1; i<=TargetPlan.MonthTargets.Count; i++)
            {
                Label.Add(L["TargetPlans:MonthTarget:00003", i]);
            }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var targetPlan = new TargetPlanRequestDto();
            targetPlan.DepartmentId = TargetPlan.DepartmentId;
            targetPlan.PlanType = TargetPlan.PlanType;
            targetPlan.UnitMeasure = TargetPlan.UnitMeasure;
            targetPlan.Year = TargetPlan.Year;
            targetPlan.YearTarget = TargetPlan.YearTarget;
            targetPlan.MonthTarget = new List<double>();
            for(int i =0; i< TargetPlan.MonthTargets.Count; i++)
            {
                targetPlan.MonthTarget.Add(TargetPlan.MonthTargets[i]);
            }
            await _departmentService.UpdateTargetPlanAsync(targetPlan);
            return NoContent();
        }
        public class UpdateTargetPlanViewModel
        {
            [HiddenInput]
            public Guid DepartmentId { get; set; }

            [HiddenInput]
            public PlanType PlanType { get; set; }

            [HiddenInput]
            public UnitMeasure UnitMeasure { get; set; }

            [HiddenInput]
            public string Year { get; set; }
            
            [Display(Name ="YearTarget")]         
            public double YearTarget { get; set; }

            public List<double> MonthTargets { get; set; }           
        }
    }
}
