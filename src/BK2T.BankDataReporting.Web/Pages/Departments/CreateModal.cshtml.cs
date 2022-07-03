using BK2T.BankDataReporting.Departments;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BK2T.BankDataReporting.Web.Pages.Departments
{
    public class CreateModalModel : BankDataReportingPageModel
    {
        private readonly IDepartmentAppService _departmentService;

        [BindProperty]
        public CreateDepartmentViewModel Department { get; set; }

        public CreateModalModel(IDepartmentAppService departmentService)
        {
            _departmentService = departmentService;
        }

        public void OnGet()
        {
            Department = new CreateDepartmentViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var departmentRequest = ObjectMapper.Map<CreateDepartmentViewModel, DepartmentRequestDto>(Department);
            await _departmentService.CreateAsync(departmentRequest);
            return NoContent();
        }

        public class CreateDepartmentViewModel
        {
            [Required]
            [Display(Name = "Departments:Code")]
            [RegularExpression("([0-9]*)", ErrorMessage = "Departments:CodeError")]
            [StringLength(DepartmentConst.CodeMaxLength)]
            public string Code { get; set; }

            [Required]
            [Display(Name = "Departments:Name")]
            [RegularExpression(@"^\S.*$", ErrorMessage = "Departments:NameError")]
            [StringLength(DepartmentConst.NameMaxLength)]
            public string Name { get; set; }

            [Display(Name = "Departments:OldCode")]
            public string OldCode { get; set; }

            [Display(Name = "Departments:CustomerSegments")]
            public string CustomerSegments { get; set; }
        }
    }
}
