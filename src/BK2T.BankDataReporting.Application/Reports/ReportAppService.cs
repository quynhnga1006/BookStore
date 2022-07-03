using BK2T.BankDataReporting.Permissions;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace BK2T.BankDataReporting.Reports
{
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IReportTemplateAppService _reportTemplateService;
        private readonly IReportItemRepository _reportItemRepository;
        private readonly INiiItemRepository _niiItemRepository;
        private readonly IRepository<CollateralItem, Guid> _collateralItemRepository;
        private readonly IRepository<ProvisionItem, Guid> _provisionRepository;
        private readonly IRepository<DebtDueCustomerItem, Guid> _debtDueCustomerItemRepository;
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _identityUserRepository;

        public ReportAppService(
            IReportTemplateAppService reportTemplateService,
            IReportItemRepository reportItemRepository,
            INiiItemRepository niiItemRepository,
            IRepository<CollateralItem, Guid> collateralItemRepository,
            IRepository<ProvisionItem, Guid> provisionRepository,
            IRepository<DebtDueCustomerItem, Guid> debtDueCustomerItemRepository,
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            ICurrentUser currentUser,
            IIdentityUserRepository identityUserRepository
            )
        {
            _reportTemplateService = reportTemplateService;
            _reportItemRepository = reportItemRepository;
            _collateralItemRepository = collateralItemRepository;
            _provisionRepository = provisionRepository;
            _debtDueCustomerItemRepository = debtDueCustomerItemRepository;
            _reportTemplateRepository = reportTemplateRepository;
            _currentUser = currentUser;
            _identityUserRepository = identityUserRepository;
            _niiItemRepository = niiItemRepository;
        }

        public async Task<List<DatatableColumn>> GetColumnsByReportId(ReportColumnDto reportColumnDto)
        {
            var reportSettings = await _reportTemplateService
                 .GetSettingVisibleAsync(reportColumnDto.ReportType, reportColumnDto.ReportId);

            var columns = reportSettings.Select(x => new DatatableColumn { Title = x.Value.As<string>(), Data = x.Key.As<string>() }).ToList();
            return columns;
        }

        [HttpPost]
        public async Task<PagedResultDto<Dictionary<string, string>>> GetReportItemsByReportId(ReportSearchDto reportSearchDto)
        {
            if (!await AuthorizationService.IsGrantedAsync(BankDataReportingPermissions.Reports.GetAll))
            {
                if (await AuthorizationService.IsGrantedAsync(BankDataReportingPermissions.Reports.GetByOwnDepartment))
                {
                    var extraProp = _identityUserRepository.GetAsync((Guid)_currentUser.Id).Result.ExtraProperties;
                    reportSearchDto.DepartmentId = (Guid)extraProp["DepartmentId"];
                }
                else if (await AuthorizationService.IsGrantedAsync(BankDataReportingPermissions.Reports.Default))
                {
                    reportSearchDto.UserId = _currentUser.Id;
                }
            }
            var reportTemplate = await _reportTemplateRepository.FirstOrDefaultAsync(x => x.ReportType.Equals(reportSearchDto.ReportType));
            if (reportTemplate == null) return new();
            var report = reportTemplate.Reports.FirstOrDefault(x => x.Id.Equals(reportSearchDto.ReportId));
            if (report == null) return new();

            var visibleSettings = report.Setting
                .Where(x => x.Value.AsBsonDocument.TryGetValue("IsVisible", out _))
                .Where(x => x.Value.AsBsonDocument.GetValue("IsVisible").AsBoolean)
                .ToList();

            var niiSettings = visibleSettings
                .Where(x => x.Value.AsBsonDocument.TryGetValue("IsNII", out _))
                .Where(x => x.Value.AsBsonDocument.GetValue("IsNII").AsBoolean)
                .ToList();

            var notNiiSettings = visibleSettings.Except(niiSettings).ToList();

            var groupBySettings = report.Setting
                .Where(x => x.Value.AsBsonDocument.TryGetValue("IsGroupBy", out _))
                .Where(x => x.Value.AsBsonDocument.GetValue("IsGroupBy").AsBoolean)
                .ToList();

            long total = 0;
            var reportItems = new List<BsonDocument>();
            (total, reportItems) = await _reportItemRepository
                .GetReportItemsByFilter(
                    notNiiSettings,
                    reportSearchDto.ReportType,
                    reportSearchDto.ReportDate,
                    reportSearchDto.DepartmentId,
                    reportSearchDto.UserId,
                    reportSearchDto.CustomParams,
                    reportSearchDto.SortingParams,
                    reportSearchDto.SkipCount,
                    reportSearchDto.MaxResultCount);

            var data = new List<Dictionary<string, string>>();

            long niiTotal = 0;
            var niiItems = new List<BsonDocument>();
            if(niiSettings != null && niiSettings.Any())
            {
                (niiTotal, niiItems) = await _niiItemRepository.GetNiiItemsByGroupBySetting(groupBySettings, niiSettings, reportSearchDto.ReportDate.Value);
                if(groupBySettings != null && groupBySettings.Any())
                {
                    reportItems.ForEach(item =>
                    {
                        var itemDicts = item.ToDictionary();

                        var niiItem = niiItems
                            .WhereIf(
                                itemDicts.TryGetValue("DepartmentId", out var departmentCode),
                                n => n.GetValue("DepartmentId").Equals(departmentCode.ToString()))
                            .WhereIf(
                                itemDicts.TryGetValue("SoCIF", out var CIFNumber),
                                n => n.GetValue("SoCIF").Equals(CIFNumber.ToString()))
                            .FirstOrDefault();

                        var dataItem = notNiiSettings.ToDictionary(k => k.Name, k => itemDicts.GetValueOrDefault(k.Name)?.ToString());
                        niiSettings.ForEach(setting =>
                            dataItem.Add(setting.Name, niiItem?.GetValue(setting.Name)?.ToString()));
                        data.Add(dataItem);
                    });
                }
                else
                {
                    reportItems.ForEach(item =>
                    {
                        var itemDicts = item.ToDictionary();

                        var niiItem = niiItems
                            .WhereIf(
                                itemDicts.TryGetValue("SoTaiKhoan", out var accountNumber),
                                n => n.GetValue("SoTaiKhoan").Equals(accountNumber.ToString()))
                            .FirstOrDefault();

                        var dataItem = notNiiSettings.ToDictionary(k => k.Name, k => itemDicts.GetValueOrDefault(k.Name)?.ToString());
                        niiSettings.ForEach(setting =>
                            dataItem.Add(setting.Name, niiItem?.GetValue(setting.Name)?.ToString()));
                        data.Add(dataItem);
                    });
                }
                return new PagedResultDto<Dictionary<string, string>>(total, data);
            }
            
            reportItems.ForEach(item =>
            {
                var itemDicts = item.ToDictionary();
                var dataItem = notNiiSettings.ToDictionary(k => k.Name, k => itemDicts.GetValueOrDefault(k.Name)?.ToString());
                data.Add(dataItem);
            });
            return new PagedResultDto<Dictionary<string, string>>(total, data);
        }

        [HttpPost]
        public async Task<PagedResultDto<Dictionary<string, string>>> GetMonthlyReportItemsByReportId(ReportSearchDto reportSearchDto)
        {
            switch (reportSearchDto.ReportType)
            {
                case ReportType.Deposit:
                case ReportType.Loan:
                case ReportType.NII:
                    return null;
                case ReportType.Collateral:
                    return await GetCollateralItemsByReportId(reportSearchDto);
                case ReportType.Provision:
                    return await GetProvisionItemsByReportId(reportSearchDto);
                case ReportType.DebtDueCustomer:
                    return await GetDebtDueCustomerByDebtDueCustomerId(reportSearchDto);
                default:
                    return new();
            }
        }
        private async Task<PagedResultDto<Dictionary<string, string>>> GetCollateralItemsByReportId(ReportSearchDto reportSearchDto)
        {
            var reportTemplate = await _reportTemplateRepository.FirstOrDefaultAsync(x => x.ReportType.Equals(reportSearchDto.ReportType));
            if (reportTemplate == null) return new();
            var report = reportTemplate.Reports.FirstOrDefault(x => x.Id.Equals(reportSearchDto.ReportId));
            if (report == null) return new();
            if (reportSearchDto.ReportDate.HasValue)
            {
                reportSearchDto.ReportDate = DateTime.SpecifyKind(reportSearchDto.ReportDate.Value, DateTimeKind.Utc);
            }

            var data = new List<Dictionary<string, string>>();
            var collateralVisibleSettings = report.Setting
                .Where(x => x.Value.AsBsonDocument.TryGetValue("IsVisible", out _))
                .Where(x => x.Value.AsBsonDocument.GetValue("IsVisible").AsBoolean)
                .Select(x => x.Name)
                .ToList();

            var reportQueryable = _collateralItemRepository
                .WhereIf(reportSearchDto.ReportDate.HasValue, r => r.DateOfData.Equals(reportSearchDto.ReportDate.Value))
                .WhereIf(reportSearchDto.DepartmentId.HasValue, r => r.DepartmentId == reportSearchDto.DepartmentId.Value);

            var total = reportQueryable.LongCount();
            var collaterals = await AsyncExecuter.ToListAsync(
                reportQueryable
                    .Skip(reportSearchDto.SkipCount)
                    .Take(reportSearchDto.MaxResultCount)
            );
            collaterals.ForEach(item =>
            {
                var itemDicts = item.ReportData.ToDictionary();
                var dataItem = collateralVisibleSettings.ToDictionary(k => k, k => itemDicts.GetValueOrDefault(k)?.ToString());
                data.Add(dataItem);
            });

            return new PagedResultDto<Dictionary<string, string>>(total, data);
        }

        private async Task<PagedResultDto<Dictionary<string, string>>> GetProvisionItemsByReportId(ReportSearchDto reportSearchDto)
        {
            var visibleSettingDict = await _reportTemplateService.GetSettingVisibleAsync(reportSearchDto.ReportType, reportSearchDto.ReportId);
            if (visibleSettingDict == null) return new();
            if (reportSearchDto.ReportDate.HasValue)
            {
                reportSearchDto.ReportDate = DateTime.SpecifyKind(reportSearchDto.ReportDate.Value, DateTimeKind.Utc);
            }

            var reportQueryable = _provisionRepository
                                    .WhereIf(reportSearchDto.ReportDate.HasValue, r => r.DateOfData.Equals(reportSearchDto.ReportDate.Value))
                                    .WhereIf(reportSearchDto.DepartmentId.HasValue, r => r.DepartmentId == reportSearchDto.DepartmentId.Value);

            var total = reportQueryable.LongCount();
            var provisions = await AsyncExecuter.ToListAsync(
                reportQueryable
                    .Skip(reportSearchDto.SkipCount)
                    .Take(reportSearchDto.MaxResultCount)
            );
            var data = new List<Dictionary<string, string>>();

            provisions.ForEach(item =>
            {
                var itemDicts = item.ReportData.ToDictionary();
                var dataItem = visibleSettingDict.ToDictionary(k => k.Key, k => itemDicts.GetValueOrDefault(k.Key)?.ToString());
                data.Add(dataItem);
            });

            return new PagedResultDto<Dictionary<string, string>>(reportQueryable.LongCount(), data);
        }

        [HttpPost]
        public async Task<PagedResultDto<Dictionary<string, string>>> GetDebtDueCustomerByDebtDueCustomerId(ReportSearchDto reportSearchDto)
        {
            var reportTemplate = await _reportTemplateRepository.FirstOrDefaultAsync(x => x.ReportType.Equals(reportSearchDto.ReportType));
            if (reportTemplate == null) return new();
            var report = reportTemplate.Reports.FirstOrDefault(x => x.Id.Equals(reportSearchDto.ReportId));
            if (report == null) return new();
            if (reportSearchDto.ReportDate.HasValue)
            {
                reportSearchDto.ReportDate = DateTime.SpecifyKind(reportSearchDto.ReportDate.Value, DateTimeKind.Utc);
            }
            var visibleSettings = report.Setting
                .Where(x => x.Value.AsBsonDocument.TryGetValue("IsVisible", out _))
                .Where(x => x.Value.AsBsonDocument.GetValue("IsVisible").AsBoolean)
                .Select(x => x.Name)
                .ToList();
            var reportQueryable = _debtDueCustomerItemRepository
                .WhereIf(reportSearchDto.DepartmentId.HasValue, r => r.DepartmentId.Equals(reportSearchDto.DepartmentId.Value))
                .WhereIf(reportSearchDto.ReportDate.HasValue, r => r.DateOfData.Equals(reportSearchDto.ReportDate.Value))
                .WhereIf(reportSearchDto.UserId.HasValue, r => r.UserId.Equals(reportSearchDto.UserId.Value));

            var total = reportQueryable.LongCount();
            var debtDueCustomers = await AsyncExecuter.ToListAsync(
                reportQueryable
                    .Skip(reportSearchDto.SkipCount)
                    .Take(reportSearchDto.MaxResultCount)
            );
            var data = new List<Dictionary<string, string>>();
            debtDueCustomers.ForEach(item =>
            {
                var itemDicts = item.ReportData.ToDictionary();
                var dataItem = visibleSettings.ToDictionary(k => k, k => itemDicts.GetValueOrDefault(k)?.ToString());
                data.Add(dataItem);
            });
            return new PagedResultDto<Dictionary<string, string>>(total, data);
        }
    }
}