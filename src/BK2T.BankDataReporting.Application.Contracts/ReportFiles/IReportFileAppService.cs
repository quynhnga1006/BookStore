using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BK2T.BankDataReporting.ReportFiles
{
    public interface IReportFileAppService
        : ICrudAppService<
            ReportFileDto,
            Guid,
            PagedAndSortedResultRequestDto,
            ReportFileRequestDto>
    {
    }
}
