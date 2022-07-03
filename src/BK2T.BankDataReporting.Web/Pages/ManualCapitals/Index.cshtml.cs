using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.Departments.ManualCapitals;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace BK2T.BankDataReporting.Web.Pages.ManualCapitals
{
    public class IndexModel : BankDataReportingPageModel
    {
        private readonly IDepartmentAppService _departmentService;

        public ManualCapitalViewModel ViewModel { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Years { get; set; }
        public IndexModel(
            IDepartmentAppService departmentService
            )
        {
            _departmentService = departmentService;
        }
        public virtual async Task OnGetAsync()
        {
            var departments = await _departmentService.GetListAsync(new PagedAndSortedResultRequestDto { MaxResultCount = 1000 });

            Departments = new List<SelectListItem>();
            foreach (var item in departments.Items)
            {
                Departments.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString(),
                });
            }

            Years = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = "2022",
                    Text = "2022",
                    Selected = true
                },
                new SelectListItem
                {
                    Value = "2023",
                    Text = "2023"
                },
                new SelectListItem
                {
                    Value = "2024",
                    Text = "2024"
                },
                new SelectListItem
                {
                    Value = "2025",
                    Text = "2025"
                },
                new SelectListItem
                {
                    Value = "2026",
                    Text = "2026"
                },
                new SelectListItem
                {
                    Value = "2027",
                    Text = "2027"
                }
            };
        }
        public class ManualCapitalViewModel
        {

            [SelectItems(nameof(Departments))]
            [Display(Name = "Departments")]
            public Guid? DepartmentId { get; set; }

            [Display(Name = "ManualCapital:CustomerType")]
            public CustomerType CustomerType { get; set; }

            [SelectItems(nameof(Years))]
            [Display(Name = "Years")]
            public string Years { get; set; }
        }
    }
}