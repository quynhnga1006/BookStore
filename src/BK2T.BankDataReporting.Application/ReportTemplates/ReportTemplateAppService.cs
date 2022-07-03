using BK2T.BankDataReporting.Permissions;
using BK2T.BankDataReporting.ReportFiles;
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

namespace BK2T.BankDataReporting.ReportTemplates
{
    [Authorize(BankDataReportingPermissions.ReportTemplates.Default)]
    public class ReportTemplateAppService : ApplicationService, IReportTemplateAppService
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplatesRepository;

        public ReportTemplateAppService(IRepository<ReportTemplate, Guid> reportTemplatesRepository)
        {
            _reportTemplatesRepository = reportTemplatesRepository;
        }

        [Authorize(BankDataReportingPermissions.ReportTemplates.Update)]
        [HttpPost]
        public async Task<ReportTemplateDto> AddReportToReportTemplateAsync(CreateEditReportDto input)
        {
            var reportTemplate = await _reportTemplatesRepository.
                FirstOrDefaultAsync(rp => rp.ReportType.Equals(input.ReportType));
            reportTemplate.Reports.Add(new Report(GuidGenerator.Create(), input.Name, input.Setting.ToBsonDocument()));
            await _reportTemplatesRepository.UpdateAsync(reportTemplate);
            return ObjectMapper.Map<ReportTemplate, ReportTemplateDto>(reportTemplate);
        }

        [HttpPost]
        public async Task<ReportTemplateDto> UpdateReportAsync(Guid reportId, CreateEditReportDto input)
        {
            var reportTemplate = await _reportTemplatesRepository.
                FirstOrDefaultAsync(rp => rp.ReportType.Equals(input.ReportType));
            var report = reportTemplate.Reports.Find(s => s.Id.Equals(reportId));
            report.Name = input.Name;
            report.Setting = input.Setting.ToBsonDocument();
            await _reportTemplatesRepository.UpdateAsync(reportTemplate);
            return ObjectMapper.Map<ReportTemplate, ReportTemplateDto>(reportTemplate);
        }

        [HttpGet]
        public async Task<Dictionary<string, object>> GetSettingVisibleAsync(ReportType reportType, Guid reportId)
        {
            var reportTemplate = await _reportTemplatesRepository.
                FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            var report = reportTemplate.Reports.Find(s => s.Id.Equals(reportId));
            if (report == null) return new();
            Dictionary<string, object> visibleSetting = new();
            foreach (var setting in report.Setting)
            {
                var settingAttribute = setting.Value.AsBsonDocument;
                if (settingAttribute["IsVisible"].AsBoolean)
                {
                    visibleSetting.Add(setting.Name,
                        settingAttribute["Label"].AsString);
                }
            }
            return visibleSetting;
        }

        [HttpGet]
        public async Task<Dictionary<string, object>> GetSettingQueryableAsync(ReportType reportType, Guid reportId)
        {
            var reportTemplate = await _reportTemplatesRepository.
                FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            var report = reportTemplate.Reports.Find(s => s.Id.Equals(reportId));
            if (report == null) return new();
            Dictionary<string, object> visibleSetting = new();
            foreach (var setting in report.Setting)
            {
                var settingAttribute = setting.Value.AsBsonDocument;
                if (settingAttribute["IsVisible"].AsBoolean && settingAttribute["IsQueryable"].AsBoolean)
                {
                    Dictionary<string, object> attribute = new();
                    attribute.Add("IsQueryable", settingAttribute["IsQueryable"].AsBoolean);
                    attribute.Add("DataType", settingAttribute["DataType"].AsInt32);
                    attribute.Add("Label", settingAttribute["Label"].AsString);
                    visibleSetting.Add(setting.Name, attribute);
                }
            }
            return visibleSetting;
        }

        [HttpGet]
        public async Task<PagedResultDto<string>> GetKeysOfReportTemlateAsync(ReportType reportType)
        {
            var reportTemplate = await _reportTemplatesRepository
                .FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            if (reportTemplate == null) return new();
            var templateDictionary = reportTemplate.Template.ToDictionary();
            List<string> listKeys = templateDictionary.Keys.ToList();
            return new PagedResultDto<string>(listKeys.Count, listKeys);
        }

        [HttpGet]
        public async Task<PagedResultDto<TemplateDto>> GetTemplatesOfReportTemplateAsync(ReportType reportType)
        {
            var reportTemplate = await _reportTemplatesRepository.
                FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            var templateDictionary = reportTemplate.Template.ToDictionary();
            List<TemplateDto> listTemplates = new();
            foreach (KeyValuePair<string, object> keyValuePair in templateDictionary)
            {
                Dictionary<string, object> attribute = (Dictionary<string, object>)keyValuePair.Value;
                listTemplates.Add(new TemplateDto
                {
                    Key = keyValuePair.Key,
                    Label = attribute["Label"].ToString(),
                    DataType = (ReportItemDataType)attribute["DataType"]
                });
            }
            return new PagedResultDto<TemplateDto>(listTemplates.Count, listTemplates);
        }
        [HttpGet]
        public async Task<ReportTemplateDto> GetReportTemplateAsync(ReportType reportType)
        {
            var reportTemplate = await _reportTemplatesRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            var reportTemplateDto = ObjectMapper.Map<ReportTemplate, ReportTemplateDto>(reportTemplate);
            return reportTemplateDto;
        }

        [HttpGet]
        public async Task<ReportDto> GetReportAsync(ReportType reportType, Guid reportId)
        {
            var reportTemplate = await _reportTemplatesRepository.
                FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            var report = reportTemplate.Reports.Find(s => s.Id.Equals(reportId));
            return ObjectMapper.Map<Report, ReportDto>(report);
        }

        [HttpDelete]
        public async Task DeleteReportAsync(ReportType reportType, Guid reportId)
        {
            var reportTemplate = await _reportTemplatesRepository.
                FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            var report = reportTemplate.Reports.Find(s => s.Id.Equals(reportId));
            reportTemplate.Reports.Remove(report);
            await _reportTemplatesRepository.UpdateAsync(reportTemplate);
        }

        public async Task<PagedResultDto<ReportDto>> GetListReportAsync(ReportType reportType)
        {
            var reportTemplate = await _reportTemplatesRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(reportType));
            var reportTemplateDto = ObjectMapper.Map<ReportTemplate, ReportTemplateDto>(reportTemplate);
            return new PagedResultDto<ReportDto>(reportTemplateDto.Reports.Count, (IReadOnlyList<ReportDto>)reportTemplateDto.Reports);
        }
    }
}