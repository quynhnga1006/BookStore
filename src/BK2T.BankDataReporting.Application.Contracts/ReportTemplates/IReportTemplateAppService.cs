using BK2T.BankDataReporting.ReportFiles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BK2T.BankDataReporting.ReportTemplates
{
    public interface IReportTemplateAppService : IApplicationService
    {
        public Task<ReportTemplateDto> AddReportToReportTemplateAsync(CreateEditReportDto input);
        public Task<ReportTemplateDto> UpdateReportAsync(Guid reportId, CreateEditReportDto input);
        public Task<ReportTemplateDto> GetReportTemplateAsync(ReportType reportType);
        public Task<PagedResultDto<string>> GetKeysOfReportTemlateAsync(ReportType reportType);
        public Task<Dictionary<string, object>> GetSettingVisibleAsync(ReportType reportType, Guid reportId);
        public Task<Dictionary<string, object>> GetSettingQueryableAsync(ReportType reportType, Guid reportId);
        public Task<ReportDto> GetReportAsync(ReportType reportType, Guid reportId);
        public Task DeleteReportAsync(ReportType reportType, Guid reportId);
        public Task<PagedResultDto<ReportDto>> GetListReportAsync(ReportType reportType);
        public Task<PagedResultDto<TemplateDto>> GetTemplatesOfReportTemplateAsync(ReportType reportType);
    }
}