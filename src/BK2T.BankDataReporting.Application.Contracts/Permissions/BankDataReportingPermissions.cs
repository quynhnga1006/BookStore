namespace BK2T.BankDataReporting.Permissions
{
    public static class BankDataReportingPermissions
    {
        public const string GroupName = "BankDataReporting";

        public static class Departments
        {
            public const string Default = GroupName + ".Departments";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class ReportFiles
        {
            public const string Default = GroupName + ".ReportFiles";
            public const string Create = Default + ".Create";
            public const string Delete = Default + ".Delete";
        }

        public static class ReportTemplates
        {
            public const string Default = GroupName + ".ReportTemplates";
            public const string Update = Default + ".Update";
            public const string Add = Default + ".Add";
            public const string Delete = Default + ".Delete";
            public const string Menu = Default + ".Menu";
        }

        public static class Reports
        {
            public const string Default = GroupName + ".Reports";
            public const string GetAll = Default + ".GetAll";
            public const string GetByOwnDepartment = Default + ".GetByOwnDepartment";
            public const string FilterByDepartment = Default + ".FilterByDepartment";
            public const string FilterByUser = Default + ".FilterByUser";
        }
    }
}