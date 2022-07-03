using BK2T.BankDataReporting.Files;
using BK2T.BankDataReporting.Permissions;
using BK2T.BankDataReporting.Reports;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace BK2T.BankDataReporting.ReportFiles
{
    [Authorize(BankDataReportingPermissions.ReportFiles.Default)]
    public class ReportFileAppService
        : CrudAppService<
            ReportFile, 
            ReportFileDto,
            Guid,
            PagedAndSortedResultRequestDto,
            ReportFileRequestDto>
        , IReportFileAppService
    {
        private readonly IRepository<ReportFile, Guid> _reportFileRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly FileAppService _fileAppService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IIdentityUserRepository _identityUserRepository;

        public ReportFileAppService(
            IRepository<ReportFile, Guid> reportFileRepository,
            IGuidGenerator guidGenerator,
            FileAppService fileAppService,
            IBackgroundJobManager backgroundJobManager,
            IIdentityUserRepository identityUserRepository)
            : base(reportFileRepository)
        {
            _reportFileRepository = reportFileRepository;
            _guidGenerator = guidGenerator;
            _fileAppService = fileAppService;
            _identityUserRepository = identityUserRepository;
            _backgroundJobManager = backgroundJobManager;
        }

        public override async Task<PagedResultDto<ReportFileDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var reportFilesQueryable = _reportFileRepository.Skip(input.SkipCount).Take(input.MaxResultCount);
            var reportFiles = await _reportFileRepository.AsyncExecuter.ToListAsync(reportFilesQueryable);
            if (!reportFiles.Any()) return new PagedResultDto<ReportFileDto>(0, new List<ReportFileDto>());

            var users = await _identityUserRepository.GetListAsync();
            var userDicts = users.ToDictionary(k => k.Id, v => v.UserName);

            var reportFileDtos = ObjectMapper.Map<List<ReportFile>, List<ReportFileDto>>(reportFiles);
            reportFileDtos.ForEach(reportFileDto => reportFileDto.CreatedByUsername = userDicts.GetValueOrDefault(reportFileDto.CreatedByUserId));

            return new PagedResultDto<ReportFileDto>()
            {
                TotalCount = _reportFileRepository.LongCount(),
                Items = reportFileDtos
            };
        }

        [Authorize(BankDataReportingPermissions.ReportFiles.Create)]
        public override async Task<ReportFileDto> CreateAsync(ReportFileRequestDto input)
        {
            var fileName = await _fileAppService.UploadFileAsync(input.FileData);
            var reportFile = new ReportFile(
                _guidGenerator.Create(),
                input.ReportDate,
                input.ReportType,
                ReportFileStatus.Uploaded,
                fileName
            );
            await _reportFileRepository.InsertAsync(reportFile);

            var dataImportingArgs = new DataImportingArgs
            {
                DateOfData = reportFile.ReportDate,
                ReportFileId = reportFile.Id,
                ReportFileName = fileName,
                ReportType = reportFile.ReportType
            };
            await _backgroundJobManager.EnqueueAsync(args: dataImportingArgs);
            var reportFileResponse = ObjectMapper.Map<ReportFile, ReportFileDto>(reportFile);
            return reportFileResponse;
        }

        [Authorize(BankDataReportingPermissions.ReportFiles.Delete)]
        public override Task DeleteAsync(Guid id)
        {
            var reportFile = _reportFileRepository.GetAsync(id);
            _fileAppService.DeleteFileAsync(reportFile.Result.FileData);
            return _reportFileRepository.DeleteAsync(id);
        }
    }
}
