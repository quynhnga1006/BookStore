using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.Permissions;
using BK2T.BankDataReporting.TargetPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BK2T.BankDataReporting.Departments
{
    [Authorize(BankDataReportingPermissions.Departments.Default)]
    public class DepartmentAppService
        : CrudAppService<
            Department, DepartmentDto,
            Guid,
            PagedAndSortedResultRequestDto,
            DepartmentRequestDto>
        , IDepartmentAppService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly DepartmentManager _departmentManager;

        public DepartmentAppService(
            IDepartmentRepository departmentRepository,
            DepartmentManager departmentManager)
            : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
            _departmentManager = departmentManager;
        }

        public async Task<PagedResultDto<TargetPlanDto>> GetTargetPlansAsync(TargetPlanSearchDto targetPlanSearchDto)
        {
            var department = await _departmentRepository.GetAsync(targetPlanSearchDto.DepartmentId);
            var targetPlansDto = new List<TargetPlanDto>();
            foreach (var bigTargetPlan in department.TargetPlans)
            {
                if (bigTargetPlan.PlanType == targetPlanSearchDto.PlanType)
                {
                    var targetPlanDto = ManualMappingFromTargetPlanToTargetPlanDto(bigTargetPlan, targetPlanSearchDto.Year);
                    targetPlansDto.Add(targetPlanDto);
                    foreach (var targetPlan in bigTargetPlan.TargetPlans)
                    {
                        targetPlanDto = ManualMappingFromTargetPlanToTargetPlanDto(targetPlan, targetPlanSearchDto.Year);
                        targetPlansDto.Add(targetPlanDto);
                    }
                    break;
                }
                foreach (var targetPlan in bigTargetPlan.TargetPlans)
                {
                    if (targetPlan.PlanType == targetPlanSearchDto.PlanType)
                    {
                        var targetPlanDto = ManualMappingFromTargetPlanToTargetPlanDto(targetPlan, targetPlanSearchDto.Year);
                        targetPlansDto.Add(targetPlanDto);
                        return new PagedResultDto<TargetPlanDto>(targetPlansDto.Count, targetPlansDto); ;
                    }
                }
            }
            return new PagedResultDto<TargetPlanDto>(targetPlansDto.Count, targetPlansDto);
        }

        public async Task<TargetPlanDto> GetTargetPlanAsync(Guid departmentId, PlanType planType, string year)
        {
            var department = await _departmentRepository.GetAsync(departmentId);
            var targetPlanDto = new TargetPlanDto();
            foreach (var bigTargetPlan in department.TargetPlans)
            {
                if(bigTargetPlan.PlanType == planType)
                {
                    targetPlanDto = ManualMappingFromTargetPlanToTargetPlanDto(bigTargetPlan, year);
                    return targetPlanDto;
                }
                foreach (var smallTargetPlan in bigTargetPlan.TargetPlans)
                {
                    if (smallTargetPlan.PlanType == planType)
                    {
                        targetPlanDto = ManualMappingFromTargetPlanToTargetPlanDto(smallTargetPlan, year);
                         return targetPlanDto;
                    }
                }
            }
            return targetPlanDto;
        }

        [HttpPost]
        public async Task UpdateTargetPlanAsync(TargetPlanRequestDto input)
        {
            var department = await _departmentRepository.GetAsync(input.DepartmentId);
            bool check = false;
            foreach (var bigTargetPlan in department.TargetPlans)
            {
                var targetMonths = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                bigTargetPlan.YearsTarget.TryGetValue(input.Year, out var yearTarget);
                bigTargetPlan.MonthsTarget.TryGetValue(input.Year, out var monthsTarget);
                if (monthsTarget == null)
                {
                    monthsTarget = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                }
                if (check)
                {
                    break;
                }

                if (bigTargetPlan.PlanType.Equals(input.PlanType))
                {
                    if (bigTargetPlan.YearsTarget.ContainsKey(input.Year)
                        && bigTargetPlan.MonthsTarget.ContainsKey(input.Year))
                    {
                        yearTarget -= bigTargetPlan.YearsTarget[input.Year];
                        bigTargetPlan.YearsTarget[input.Year] = input.YearTarget;
                        yearTarget += bigTargetPlan.YearsTarget[input.Year];

                        for (int i = 0; i < 12; ++i)
                        {
                            monthsTarget[i] -= bigTargetPlan.MonthsTarget[input.Year][i];
                            bigTargetPlan.MonthsTarget[input.Year][i] = input.MonthTarget[i];
                            monthsTarget[i] += bigTargetPlan.MonthsTarget[input.Year][i];
                        }
                    }
                    else
                    {
                        bigTargetPlan.YearsTarget.Add(input.Year, input.YearTarget);
                        yearTarget += bigTargetPlan.YearsTarget[input.Year];

                        bigTargetPlan.MonthsTarget.Add(input.Year, input.MonthTarget);
                        for (int i = 0; i < 12; ++i)
                        {
                            monthsTarget[i] += bigTargetPlan.MonthsTarget[input.Year][i];
                        }
                    }
                    bigTargetPlan.MonthsTarget[input.Year] = monthsTarget;
                    bigTargetPlan.YearsTarget[input.Year] = yearTarget;
                    check = true;
                    break;
                }

                foreach (var smallTargetPlan in bigTargetPlan.TargetPlans)
                {                  
                    if (smallTargetPlan.PlanType.Equals(input.PlanType))
                    {
                        if (smallTargetPlan.YearsTarget.ContainsKey(input.Year)
                            && smallTargetPlan.MonthsTarget.ContainsKey(input.Year))
                        {
                            yearTarget -= smallTargetPlan.YearsTarget[input.Year];
                            smallTargetPlan.YearsTarget[input.Year] = input.YearTarget;
                            yearTarget += smallTargetPlan.YearsTarget[input.Year];

                            for (int i = 0; i < 12; ++i)
                            {
                                monthsTarget[i] -= smallTargetPlan.MonthsTarget[input.Year][i];
                                smallTargetPlan.MonthsTarget[input.Year][i] = input.MonthTarget[i];
                                monthsTarget[i] += smallTargetPlan.MonthsTarget[input.Year][i];
                            }
                        }
                        else
                        {
                            smallTargetPlan.YearsTarget.Add(input.Year, input.YearTarget);
                            yearTarget += smallTargetPlan.YearsTarget[input.Year];

                            smallTargetPlan.MonthsTarget.Add(input.Year, input.MonthTarget);
                            for (int i = 0; i < 12; ++i)
                            {
                                monthsTarget[i] += smallTargetPlan.MonthsTarget[input.Year][i];
                            }
                        }                       
                        bigTargetPlan.MonthsTarget[input.Year] = monthsTarget;
                        bigTargetPlan.YearsTarget[input.Year] = yearTarget;
                        check = true;
                        break;
                    }
                }            
            }
            await _departmentRepository.UpdateAsync(department);
            return;
        }

        public async Task<PagedResultDto<ManualCapitalDto>> GetManualCapitalsAsync(ManualCapitalSearchDto manualCapitalSearchDto)
        {
            var department = await _departmentRepository.GetAsync(manualCapitalSearchDto.DepartmentId);
            var manualCapitals = new List<ManualCapitalDto>();
            foreach(var manualCapital in department.ManualCapitals)
            {
                var manualCapitalDto = ManualMappingFromManualCapitalToTargetManualCapitalDto(manualCapital, manualCapitalSearchDto.Year);
                manualCapitals.Add(manualCapitalDto);
            };
            return new PagedResultDto<ManualCapitalDto>(manualCapitals.Count,manualCapitals);
        }

        public async Task<ManualCapitalDto> GetManualCapitalAsync(Guid departmentId, CustomerType customerType, string year)
        {
            var department = await _departmentRepository.GetAsync(departmentId);
            foreach(var manualCapital in department.ManualCapitals)
            {
                if(customerType == manualCapital.CustomerType)
                {
                    return ManualMappingFromManualCapitalToTargetManualCapitalDto(manualCapital, year);

                }
            }
            return new ManualCapitalDto();
        }

        public async Task<ManualCapitalDto> UpdateManualCapitalAsync(ManualCapitalRequestDto manualCapitalRequestDto)
        {
            var department = await _departmentRepository.GetAsync(manualCapitalRequestDto.DepartmentId);
            foreach(var manualCapital in department.ManualCapitals)
            {
                if(manualCapital.CustomerType == manualCapitalRequestDto.CustomerType)
                {
                    if(manualCapital.MonthsCapital.TryGetValue(manualCapitalRequestDto.Year, out var _))
                    {
                        manualCapital.MonthsCapital[manualCapitalRequestDto.Year] = manualCapitalRequestDto.MonthsCapital;
                    }
                    manualCapital.MonthsCapital.TryAdd(manualCapitalRequestDto.Year, manualCapitalRequestDto.MonthsCapital);
                    await _departmentRepository.UpdateAsync(department);
                    return ManualMappingFromManualCapitalToTargetManualCapitalDto(manualCapital, manualCapitalRequestDto.Year);
                }
            }
            return new ManualCapitalDto();
        }

        public override Task<PagedResultDto<DepartmentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            input.MaxResultCount = 1000;
            return base.GetListAsync(input);
        }

        [Authorize(BankDataReportingPermissions.Departments.Create)]
        public override async Task<DepartmentDto> CreateAsync(DepartmentRequestDto input)
        {
            var department = await _departmentManager.CreateAsync(input.Code, input.Name,
                                                                input.OldCode, input.CustomerSegments);
            await _departmentRepository.InsertAsync(department);
            return ObjectMapper.Map<Department, DepartmentDto>(department);
        }

        [Authorize(BankDataReportingPermissions.Departments.Update)]
        public override async Task<DepartmentDto> UpdateAsync(Guid id, DepartmentRequestDto input)
        {
            var department = await _departmentRepository.GetAsync(id);
            if (department.Code != input.Code)
            {
                await _departmentManager.ChangeCodeAsync(department, input.Code);
            }

            department.Name = input.Name;
            department.OldCode = input.OldCode;
            department.CustomerSegments = input.CustomerSegments;

            await _departmentRepository.UpdateAsync(department);
            return ObjectMapper.Map<Department, DepartmentDto>(department);
        }

        private static TargetPlanDto ManualMappingFromTargetPlanToTargetPlanDto(TargetPlan targetPlan, string year)
        {
            var targetPlanDto = new TargetPlanDto()
            {
                Id = targetPlan.Id,
                PlanType = targetPlan.PlanType,
                UnitMeasure = targetPlan.UnitMeasure
            };
            if (targetPlan.YearsTarget.TryGetValue(year, out var yearTarget)
                && targetPlan.MonthsTarget.TryGetValue(year, out var monthTarget))
            {
                targetPlanDto.YearTarget = yearTarget;
                targetPlanDto.MonthTarget = monthTarget;
            }
            else
            {
                targetPlanDto.YearTarget = 0;
                targetPlanDto.MonthTarget = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            return targetPlanDto;
        }

        private static ManualCapitalDto ManualMappingFromManualCapitalToTargetManualCapitalDto(ManualCapital manualCapital, string year)
        {
            var manualCapitalDto = new ManualCapitalDto()
            {
                CustomerType = manualCapital.CustomerType,
                UnitMeasure = manualCapital.UnitMeasure
            };
            if (manualCapital.MonthsCapital.TryGetValue(year, out var monthCapital))
            {
                manualCapitalDto.MonthsCapital = monthCapital;
            }
            else
            {
                manualCapitalDto.MonthsCapital = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            manualCapitalDto.Year = Convert.ToInt32(year);
            return manualCapitalDto;
        }
    }
}