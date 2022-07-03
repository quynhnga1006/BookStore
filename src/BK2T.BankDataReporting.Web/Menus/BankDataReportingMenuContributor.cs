using System.Threading.Tasks;
using BK2T.BankDataReporting.Localization;
using BK2T.BankDataReporting.Permissions;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using BK2T.BankDataReporting.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace BK2T.BankDataReporting.Web.Menus
{
    public class BankDataReportingMenuContributor : IMenuContributor
    {
        private readonly IReportTemplateAppService _reportTemplateAppService;
        public BankDataReportingMenuContributor(IServiceCollection service)
        {
            _reportTemplateAppService = service.GetRequiredService<IReportTemplateAppService>();
        }
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<BankDataReportingResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    BankDataReportingMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "fas fa-home",
                    order: 0
                )
            );
            if (await context.IsGrantedAsync(BankDataReportingPermissions.Reports.Default))
            {
                context.Menu.Items.Insert(
                    1,
                    GetMonthlyReportItemsApplicationMenuItem(ReportType.Provision, l).Result
                );
                context.Menu.Items.Insert(
                    1,
                    GetMonthlyReportItemsApplicationMenuItem(ReportType.Collateral, l).Result
                );
                context.Menu.Items.Insert(
                    1,
                    GetMonthlyReportItemsApplicationMenuItem(ReportType.DebtDueCustomer, l).Result
                );
                context.Menu.Items.Insert(
                    1,
                    GetApplicationMenuItem(ReportType.Deposit, l).Result
                    );
                context.Menu.Items.Insert(
                    1,
                    GetApplicationMenuItem(ReportType.Loan, l).Result
                    );
            }

            if (await context.IsGrantedAsync(BankDataReportingPermissions.ReportTemplates.Menu))
            {
                context.Menu.Items.Insert(
                    1,
                    new ApplicationMenuItem(
                        BankDataReportingMenus.ReportTemplates,
                        l["Menu:ReportTemplates"],
                        icon: "fa fa-file-o",
                        url: "/ReportTemplates",
                        order: 1
                        )
                    );
            }

            if (await context.IsGrantedAsync(BankDataReportingPermissions.ReportFiles.Default))
            {
                context.Menu.Items.Insert(
                1,
                new ApplicationMenuItem(
                    BankDataReportingMenus.ReportFiles,
                    l["Menu:ReportFiles"],
                    icon: "far fa-file-alt",
                    url: "/ReportFiles",
                    order: 1
                )
                );
            }
            if (await context.IsGrantedAsync(BankDataReportingPermissions.Departments.Default))
            {
                context.Menu.Items.Insert(
                1,
                new ApplicationMenuItem(
                    BankDataReportingMenus.Departments,
                    l["Menu:Departments"],
                    icon: "far fa-building",
                    url: "/Departments",
                    order: 1
                )
            );
            }
            context.Menu.Items.Insert(
                1,
                new ApplicationMenuItem(
                    BankDataReportingMenus.TargetPlans,
                    l["Menu:TargetPlans"],
                    url: "/TargetPlans",
                    order: 1
                ));
            context.Menu.Items.Insert(
                1,
                new ApplicationMenuItem(
                    BankDataReportingMenus.TargetPlans,
                    l["Menu:ManualCapitals"],
                    url: "/ManualCapitals",
                    order: 1
                ));
            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 0);
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 1);
        }
        private async Task<ApplicationMenuItem> GetApplicationMenuItem(ReportType reportType, IStringLocalizer l)
        {
            var report = await _reportTemplateAppService.GetListReportAsync(reportType);
            var nameMenu = $"Reports.{reportType}";
            var displayNameMenu = EnumExtensions.GetDisplayName(reportType);
            var appMenuItem = new ApplicationMenuItem(
                name: nameMenu,
                icon: "fa fa-money",
                displayName: l[displayNameMenu]
                );
            foreach (var item in report.Items)
            {
                appMenuItem.AddItem(new ApplicationMenuItem(
                    name: item.Name,
                    displayName: item.Name,
                    url: "/Reports?reportType=" + reportType + "&reportId=" + item.Id
                    ));
            }
            return appMenuItem;
        }

        private async Task<ApplicationMenuItem> GetMonthlyReportItemsApplicationMenuItem(ReportType reportType, IStringLocalizer l)
        {
            var report = await _reportTemplateAppService.GetListReportAsync(reportType);
            var nameMenu = $"Reports.{reportType}";
            var displayNameMenu = EnumExtensions.GetDisplayName(reportType);
            var appMenuItem = new ApplicationMenuItem(
                name: nameMenu,
                icon: "fa fa-money",
                displayName: l[displayNameMenu]
                );
            foreach (var item in report.Items)
            {
                appMenuItem.AddItem(new ApplicationMenuItem(
                    name: item.Name,
                    displayName: item.Name,
                    url: "/MonthlyReports?reportType=" + reportType + "&reportId=" + item.Id
                    ));
            }
            return appMenuItem;
        }
    }
}