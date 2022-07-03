using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Xunit;

namespace BK2T.BankDataReporting.Reports
{
    [Collection(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class ReportAppServiceTest : BankDataReportingApplicationTestBase
    {
        private readonly IReportAppService _reportAppService;
        private readonly IReportItemRepository _reportItemsRepository;
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;

        public ReportAppServiceTest()
        {
            _reportAppService = GetRequiredService<IReportAppService>();
            _reportItemsRepository = GetRequiredService<IReportItemRepository>();
            _reportTemplateRepository = GetRequiredService<IRepository<ReportTemplate, Guid>>();
        }

        [Fact]
        public async Task GetColumnByReportIdAsync_ShouldGetColumn()
        {
            //Arrange
            var loanTemplate = await _reportTemplateRepository.FirstOrDefaultAsync(r => r.ReportType == ReportType.Loan);
            var reportId = loanTemplate.Reports.FirstOrDefault(r => r.Name.Equals("Báo cáo tiền nợ 1")).Id;

            //Act
            var result = await _reportAppService.GetColumnsByReportId(new ReportColumnDto
            {
                ReportId = reportId,
                ReportType = ReportType.Loan
            });

            //Assert
            result.Count.ShouldBeGreaterThanOrEqualTo(0);
        }
    }
}
