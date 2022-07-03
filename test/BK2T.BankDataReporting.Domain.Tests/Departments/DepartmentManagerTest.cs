using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Xunit;

namespace BK2T.BankDataReporting.Departments
{
    [Collection(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class DepartmentManagerTest : BankDataReportingDomainTestBase
    {
        private readonly IGuidGenerator _guidGenerator;
        private IDepartmentRepository _mockDepartmentRepository;

        public DepartmentManagerTest()
        {
            _guidGenerator = ServiceProvider.GetRequiredService<IGuidGenerator>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _mockDepartmentRepository = Substitute.For<IDepartmentRepository>();
        }

        [Fact]
        public async Task Create_NewCode_ReturnsNewDepartment()
        {
            // Arrange
            var code = "newCode";
            var name = "new Name";
            var oldCode = "oldCode";
            var segments = new List<string> { "00" };

            _mockDepartmentRepository
                .AnyAsync(Arg.Any<Expression<Func<Department, bool>>>(), default)
                .Returns(false);

            var departmentManager = new DepartmentManager(_mockDepartmentRepository, _guidGenerator);

            // Act
            var department = await departmentManager.CreateAsync(code, name, oldCode, segments);

            //Assert
            department.Id.ShouldNotBe(Guid.Empty);
            department.Code.ShouldBe(code);
            department.Name.ShouldBe(name);
            department.OldCode.ShouldBe(oldCode);
            department.CustomerSegments.ShouldBe(segments);
        }

        [Fact]
        public async Task Create_ExistedCode_ThrowsDepartmentAlreadyExistsException()
        {
            // Arrange
            var code = "newCode";
            var name = "new Name";
            var oldCode = "oldCode";
            var segments = new List<string> { "00" };

            _mockDepartmentRepository
                .AnyAsync(Arg.Any<Expression<Func<Department, bool>>>(), default)
                .Returns(true);

            var departmentManager = new DepartmentManager(_mockDepartmentRepository, _guidGenerator);

            // Act & Assert
            await Assert.ThrowsAsync<DepartmentAlreadyExistsException>(async () =>
            {
                await departmentManager.CreateAsync(code, name, oldCode, segments);
            });
        }

        [Fact]
        public async Task ChangeCode_NewCode_Success()
        {
            // Arrange
            var newCode = "newCode";
            var code = "001002003";
            var name = "Tong Hop";
            var oldCode = "oldCode";
            var segments = new List<string> { "00" };
            var targetPlans = new List<TargetPlan>
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.CreditNII, UnitMeasure.Million)
            };
            var manualCapitals = new List<ManualCapital>
            {
                new ManualCapital(CustomerType.BigEnterpriseCustomer, UnitMeasure.Billion, new List<string>())
            };
            var department = new Department(Guid.NewGuid(), code, name, oldCode, segments, targetPlans, manualCapitals);

            _mockDepartmentRepository
                .FirstOrDefaultAsync(Arg.Any<Expression<Func<Department, bool>>>(), default)
                .Returns(null as Department);

            var departmentManager = new DepartmentManager(_mockDepartmentRepository, _guidGenerator);

            // Act
            await departmentManager.ChangeCodeAsync(department, newCode);

            //Assert
            department.Code.ShouldBe(newCode);
            department.Name.ShouldBe(name);
            department.OldCode.ShouldBe(oldCode);
            department.CustomerSegments.ShouldBe(segments);
        }

        [Fact]
        public async Task ChangeCode_ExistedCode_ThrowsDepartmentAlreadyExistsException()
        {
            // Arrange
            var existedCode = "existedCode";
            var code = "001002003";
            var name = "Tong Hop";
            var oldCode = "oldCode";
            var targetPlans = new List<TargetPlan>
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.CreditNII, UnitMeasure.Million)
            };
            var manualCapitals = new List<ManualCapital>
            {
                new ManualCapital(CustomerType.BigEnterpriseCustomer, UnitMeasure.Billion, new List<string>())
            };
            var segments = new List<string> { "00" };
            var department = new Department(Guid.NewGuid(), code, name, oldCode, segments, targetPlans, manualCapitals);
            var existedDepartment = new Department(Guid.NewGuid(), existedCode, name, oldCode, segments, targetPlans, manualCapitals);

            _mockDepartmentRepository
                .FirstOrDefaultAsync(Arg.Any<Expression<Func<Department, bool>>>(), default)
                .Returns(existedDepartment);

            var departmentManager = new DepartmentManager(_mockDepartmentRepository, _guidGenerator);

            // Act & Assert
            await Assert.ThrowsAsync<DepartmentAlreadyExistsException>(async () =>
            {
                await departmentManager.ChangeCodeAsync(department, existedCode);
            });
        }
    }
}
