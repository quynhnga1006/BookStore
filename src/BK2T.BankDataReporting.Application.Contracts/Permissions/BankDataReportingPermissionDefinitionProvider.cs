using BK2T.BankDataReporting.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BK2T.BankDataReporting.Permissions
{
    public class BankDataReportingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(BankDataReportingPermissions.GroupName);
            var departmentPermission = myGroup.AddPermission(BankDataReportingPermissions.Departments.Default, L("Permission:Departments"));
            departmentPermission.AddChild(BankDataReportingPermissions.Departments.Create, L("Permission:Create"));
            departmentPermission.AddChild(BankDataReportingPermissions.Departments.Update, L("Permission:Update"));
            departmentPermission.AddChild(BankDataReportingPermissions.Departments.Delete, L("Permission:Delete"));

            var reportFilesPermission = myGroup.AddPermission(BankDataReportingPermissions.ReportFiles.Default, L("Permission:ReportFiles"));
            reportFilesPermission.AddChild(BankDataReportingPermissions.ReportFiles.Create, L("Permission:Create"));
            reportFilesPermission.AddChild(BankDataReportingPermissions.ReportFiles.Delete, L("Permission:Delete"));

            var reportTemplatePermission = myGroup.AddPermission(BankDataReportingPermissions.ReportTemplates.Default, L("Permission:ReportTemplates"));
            reportTemplatePermission.AddChild(BankDataReportingPermissions.ReportTemplates.Menu, L("Permission:Menu"));
            reportTemplatePermission.AddChild(BankDataReportingPermissions.ReportTemplates.Add, L("Permission:Create"));
            reportTemplatePermission.AddChild(BankDataReportingPermissions.ReportTemplates.Update, L("Permission:Update"));
            reportTemplatePermission.AddChild(BankDataReportingPermissions.ReportTemplates.Delete, L("Permission:Delete"));

            var reportPermission = myGroup.AddPermission(BankDataReportingPermissions.Reports.Default, L("Permission:Reports"));
            reportPermission.AddChild(BankDataReportingPermissions.Reports.GetAll, L("Permission:GetAll"));
            reportPermission.AddChild(BankDataReportingPermissions.Reports.GetByOwnDepartment, L("Permission:GetByOwnDepartment"));
            reportPermission.AddChild(BankDataReportingPermissions.Reports.FilterByDepartment, L("Permission:FilterByDepartment"));
            reportPermission.AddChild(BankDataReportingPermissions.Reports.FilterByUser, L("Permission:FilterByUser"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BankDataReportingResource>(name);
        }
    }
}