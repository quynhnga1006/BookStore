using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BK2T.BankDataReporting.Departments
{
    public interface IDepartmentAppService
        : ICrudAppService<
            DepartmentDto,
            Guid,
            PagedAndSortedResultRequestDto,
            DepartmentRequestDto>
    {
        Task<PagedResultDto<TargetPlanDto>> GetTargetPlansAsync(TargetPlanSearchDto targetPlanSearchDto);
        Task UpdateTargetPlanAsync(TargetPlanRequestDto input);
        Task<TargetPlanDto> GetTargetPlanAsync(Guid departmentId, PlanType planType, string year);
        Task<PagedResultDto<ManualCapitalDto>> GetManualCapitalsAsync(ManualCapitalSearchDto manualCapitalSearchDto);
        Task<ManualCapitalDto> GetManualCapitalAsync(Guid departmentId, CustomerType customerType, string year);
        Task<ManualCapitalDto> UpdateManualCapitalAsync(ManualCapitalRequestDto manualCapitalRequestDto);
    }
}