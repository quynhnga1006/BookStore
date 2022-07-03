using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.Permissions;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace BK2T.BankDataReporting.Web.Pages.Collaterals
{
    public class IndexModel : BankDataReportingPageModel
    {
        public ReportViewModel ViewModel { get; set; }
        public ReportDto Report { get; set; }
        public Guid ReportId { get; set; }
        public int ReportType { get; set; }
        public bool IsDisplay { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
        public List<SelectListItem> UserList { get; set; }

        private readonly IDepartmentAppService _departmentService;
        private readonly IReportTemplateAppService _reportTemplateAppService;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ICurrentUser _currentUser;

        public IndexModel(
            IDepartmentAppService departmentService,
            IIdentityUserRepository identityUserRepository,
            IReportTemplateAppService reportTemplateAppService,
            ICurrentUser currentUser
            )
        {
            _departmentService = departmentService;
            _reportTemplateAppService = reportTemplateAppService;
            _identityUserRepository = identityUserRepository;
            _currentUser = currentUser;
        }

        public virtual async Task OnGetAsync(ReportType reportType, Guid reportId)
        {
            ReportType = (int)reportType;
            ReportId = reportId;
            Report = await _reportTemplateAppService.GetReportAsync(reportType, reportId);
            if (Report == null) return;

            IsDisplay = CheckDisplayFilterByUser(reportType);

            var departmentList = await _departmentService.GetListAsync(new PagedAndSortedResultRequestDto { MaxResultCount = 1000 });
            var userList = await _identityUserRepository.GetListAsync();

            DepartmentList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text =  L["Select all department"], Selected = true}
            };

            foreach (var item in departmentList.Items)
            {
                DepartmentList.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name.ToString()
                });
            }

            if (!await AuthorizationService.IsGrantedAsync(BankDataReportingPermissions.Reports.GetAll))
            {
                if (await AuthorizationService.IsGrantedAsync(BankDataReportingPermissions.Reports.GetByOwnDepartment))
                {
                    var extraProp = _identityUserRepository.GetAsync((Guid)_currentUser.Id).Result.ExtraProperties;
                    userList = userList.Where(x => !x.ExtraProperties.IsNullOrEmpty() &&
                                        x.ExtraProperties["DepartmentId"].Equals(extraProp["DepartmentId"])).ToList();
                }
            }
            UserList = new List<SelectListItem>
            {
                 new SelectListItem { Value = "", Text =  L["Select all user"], Selected = true}
            };
            foreach (var item in userList)
            {
                UserList.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name?? item.Id.ToString()
                });
            }
        }

        private static bool CheckDisplayFilterByUser(ReportType reportType)
        {
            switch (reportType)
            {
                case BankDataReporting.ReportFiles.ReportType.Provision:
                case BankDataReporting.ReportFiles.ReportType.Collateral:
                    return false;
                case BankDataReporting.ReportFiles.ReportType.DebtDueCustomer:
                    return true;
                default:
                    return false;
            }
        }

        public class ReportViewModel
        {
            [DataType(DataType.Date)]
            [Display(Name = "ReportFiles:ReportDate")]
            public DateTime? ReportDate { get; set; }

            [SelectItems(nameof(DepartmentList))]
            [Display(Name = "Departments")]
            public Guid? DepartmentId { get; set; }

            [SelectItems(nameof(UserList))]
            [Display(Name = "Users")]
            public Guid? UserId { get; set; }
        }

        public class ComponentModel
        {
            [HiddenInput]
            public string Key { get; set; }
            [HiddenInput]
            public string Label { get; set; }
            public ReportItemDataType Type { get; set; }

            [DataType(DataType.Date)]
            public DateTime? DateTime { get; set; }
            [DataType(DataType.Text)]
            public string Text { get; set; }
        }
    }
}