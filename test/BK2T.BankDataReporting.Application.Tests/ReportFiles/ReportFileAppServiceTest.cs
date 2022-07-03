using Microsoft.AspNetCore.Http;
using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace BK2T.BankDataReporting.ReportFiles
{
    [Collection(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class ReportFileAppServiceTest : BankDataReportingApplicationTestBase
    {
        private readonly IReportFileAppService _reportFileAppService;
        private readonly IRepository<ReportFile, Guid> _reportFileRepository;

        public ReportFileAppServiceTest()
        {
            _reportFileAppService = GetRequiredService<IReportFileAppService>();
            _reportFileRepository = GetRequiredService<IRepository<ReportFile, Guid>>();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewReportFile()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "TGNGAY_NEW190322.xlsx")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
            
            var reportFileRequestDto = new ReportFileRequestDto
            {
                ReportDate = new DateTime(2010, 8, 18),
                ReportType = ReportType.Deposit,
                FileData = file
            };

            // Act
            var reportFileDto = await _reportFileAppService.CreateAsync(reportFileRequestDto);

            //Assert
            reportFileDto.Id.ShouldNotBe(Guid.Empty);
            reportFileDto.ReportDate.ShouldBe(reportFileRequestDto.ReportDate);
            reportFileDto.ReportType.ShouldBe(reportFileRequestDto.ReportType);
            reportFileDto.FileData.ShouldBe(reportFileDto.FileData);
        }
    }
}
