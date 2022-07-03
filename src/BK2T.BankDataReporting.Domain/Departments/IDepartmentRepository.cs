using System;
using Volo.Abp.Domain.Repositories;

namespace BK2T.BankDataReporting.Departments
{
    public interface IDepartmentRepository : IRepository<Department, Guid>
    {
    }
}