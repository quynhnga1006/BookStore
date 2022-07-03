using BK2T.BankDataReporting.Departments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace BK2T.BankDataReporting.Web.Pages.Departments
{
    public class EditModalModel : BankDataReportingPageModel
    {
        [BindProperty]
        public UpdateDepartmentViewModel Department { get; set; }

        private readonly IDepartmentAppService _departmentService;

        public EditModalModel(IDepartmentAppService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task OnGetAsync(Guid id)
        {
            var departmentDto = await _departmentService.GetAsync(id);
            Department = ObjectMapper.Map<DepartmentDto, UpdateDepartmentViewModel>(departmentDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var departmentRequestDto = ObjectMapper.Map<UpdateDepartmentViewModel, DepartmentRequestDto>(Department);
            await _departmentService.UpdateAsync(Department.Id, departmentRequestDto);
            return NoContent();
        }

        public class UpdateDepartmentViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
