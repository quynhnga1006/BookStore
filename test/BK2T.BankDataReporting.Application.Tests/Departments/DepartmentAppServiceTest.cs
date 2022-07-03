using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace BK2T.BankDataReporting.Departments
{
    [Collection(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class DepartmentAppServiceTest : BankDataReportingApplicationTestBase
    {
        private readonly IDepartmentAppService _departmentAppService;
        private readonly IRepository<Department, Guid> _departmentRepository;
        public DepartmentAppServiceTest()
        {
            _departmentAppService = GetRequiredService<IDepartmentAppService>();
            _departmentRepository = GetRequiredService<IRepository<Department, Guid>>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewDepartment()
        {
            // Arrange
            var departmentRequestDto = new DepartmentRequestDto
            {
                Code = "102140033",
                Name = "KeToan"
            };

            // Act
            var departmentDto = await _departmentAppService.CreateAsync(departmentRequestDto);

            //Assert
            departmentDto.Id.ShouldNotBe(Guid.Empty);
            departmentDto.Code.ShouldBe(departmentRequestDto.Code);
            departmentDto.Name.ShouldBe(departmentRequestDto.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateDepartment()
        {
            // Arrange
            var departmentRequestDto = new DepartmentRequestDto
            {
                Code = "102140033",
                Name = "KeToan"
            };
            var department = await _departmentRepository.FirstOrDefaultAsync();

            // Act
            var departmentDto = await _departmentAppService
                .UpdateAsync(department.Id, departmentRequestDto);

            //Assert
            departmentDto.Id.ShouldBe(department.Id);
            departmentDto.Code.ShouldBe(departmentRequestDto.Code);
            departmentDto.Name.ShouldBe(departmentRequestDto.Name);
        }

        [Fact]
        public async Task GetManualCapitalsAsync_ShouldGetManualCapitals()
        {
            //Arrange
            var department = await _departmentRepository.FirstOrDefaultAsync();
            var manualCapitalSearchDto = new ManualCapitalSearchDto
            {
                DepartmentId = department.Id,
                Year = "2022"
            };

            //Act
            var manualCapitals = await _departmentAppService.GetManualCapitalsAsync(manualCapitalSearchDto);

            //Assert
            manualCapitals.TotalCount.ShouldBe(5);
        }

        [Fact]
        public async Task GetManualCapitalAsync_ShouldGetManualCapital()
        {
            //Arrange
            var department = await _departmentRepository.FirstOrDefaultAsync();

            //Act
            var manualCapital = await _departmentAppService.GetManualCapitalAsync(department.Id, CustomerType.BigEnterpriseCustomer, "2022");

            //Assert
            manualCapital.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateManualCapital_ShouldUpdateManualCapital()
        {
            //Arrange
            var department = await _departmentRepository.FirstOrDefaultAsync();
            var monthsCapital = new List<double>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
            };
            var manualCapitalRequestDto = new ManualCapitalRequestDto
            {
                DepartmentId = department.Id,
                CustomerType = CustomerType.BigEnterpriseCustomer,
                Year = "2022",
                MonthsCapital = monthsCapital
            };

            //Act
            var manualCapital = await _departmentAppService.UpdateManualCapitalAsync(manualCapitalRequestDto);

            //Assert
            manualCapital.MonthsCapital[0].ShouldBeGreaterThan(0);
        }
        
        [Fact]
        public async Task GetSmallTargetPlansAsync_ShouldGetTargetPlans()
        {
            //Assert
            var department = await _departmentRepository.FirstOrDefaultAsync();
            var targetPlanSearchDto = new TargetPlanSearchDto
            {
                DepartmentId = department.Id,
                PlanType = PlanType.BigEnterpriseCreditBalance,
                Year = "2022"
            };

            //Act
            var targetPlans = await _departmentAppService.GetTargetPlansAsync(targetPlanSearchDto);

            //Asset
            targetPlans.TotalCount.ShouldBe(1);
        }

        [Fact]
        public async Task GetBigTargetPlansAsync_ShouldGetTargetPlans()
        {
            //Assert
            var department = await _departmentRepository.FirstOrDefaultAsync();
            var targetPlanSearchDto = new TargetPlanSearchDto
            {
                DepartmentId = department.Id,
                PlanType = PlanType.CreditBalance,
                Year = "2022"
            };

            //Act
            var targetPlans = await _departmentAppService.GetTargetPlansAsync(targetPlanSearchDto);

            //Asset
            targetPlans.TotalCount.ShouldBe(5);
        }
    }
}