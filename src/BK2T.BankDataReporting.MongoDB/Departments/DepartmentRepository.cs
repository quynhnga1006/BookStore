using BK2T.BankDataReporting.MongoDB;
using BK2T.BankDataReporting.Departments;
using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace BK2T.BankDataReporting.Departments
{
    public class DepartmentRepository : MongoDbRepository<BankDataReportingMongoDbContext, Department, Guid>,
        IDepartmentRepository
    {
        public DepartmentRepository(IMongoDbContextProvider<BankDataReportingMongoDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}