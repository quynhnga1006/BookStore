using Volo.Abp;

namespace BK2T.BankDataReporting.Departments
{
    public class DepartmentAlreadyExistsException : BusinessException
    {
        public DepartmentAlreadyExistsException(string code)
            : base(BankDataReportingDomainErrorCodes.DepartmentAlreadyExists)
        {
            WithData("code", code);
        }
    }
}
