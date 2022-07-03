using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BK2T.BankDataReporting.Reports
{
    public interface IReportAppService : IApplicationService
    {
        Task<PagedResultDto<Dictionary<string, string>>> GetMonthlyReportItemsByReportId(ReportSearchDto reportSearchDto);
        Task<PagedResultDto<Dictionary<string, string>>> GetReportItemsByReportId(ReportSearchDto reportSearchDto);
        Task<List<DatatableColumn>> GetColumnsByReportId(ReportColumnDto reportColumnDto);
    }
}
