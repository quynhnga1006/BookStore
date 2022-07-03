using BK2T.BankDataReporting.ReportFiles;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace BK2T.BankDataReporting.ReportTemplates
{
    [Collection(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class ReportTemplateAppServiceTest : BankDataReportingApplicationTestBase
    {
        private readonly IReportTemplateAppService _reportTemplatesAppService;
        private readonly IRepository<ReportTemplate, Guid> _reportTemplatesRepository;

        public ReportTemplateAppServiceTest()
        {
            _reportTemplatesAppService = GetRequiredService<IReportTemplateAppService>();
            _reportTemplatesRepository = GetRequiredService<IRepository<ReportTemplate, Guid>>();
        }

        [Fact]
        public async Task AddReportToReportTemplateAsync_ShouldAddNewReportIntoReportTemplate()
        {
            // Arrange
            var reportTemplate = await _reportTemplatesRepository.FirstOrDefaultAsync(r => r.ReportType.Equals(ReportType.Loan));
            var reportCount = reportTemplate.Reports.Count;
            var setting = new SettingDto()
            {
                DataType = ReportItemDataType.String,
                IsVisible = true,
                IsQueryable = true
            };
            var createReportDto = new CreateEditReportDto
            {
                ReportType = ReportType.Loan,
                Name = "Báo cáo tiền gửi 2",
                Setting = new Dictionary<string, SettingDto>
                        {
                            {"Ngay", setting },
                            {"TenNguon", setting },
                            {"MaPGD", setting},
                            {"SoCIF", setting},
                            {"TenKhachHang", setting},
                            {"MaPhanKhuc", setting},
                            {"SoTaiKhoan", setting},
                            {"LoaiTien", setting},
                            {"MaCanBoTinDung", setting},
                            {"NgayMoTaiKhoan", setting},
                            {"NgayDaoHan", setting},
                            {"NgayTraNoGanNhat", setting},
                            {"NgayTraNoTiepTheo", setting},
                            {"TenNganhKinhTeCap3", setting},
                            {"MaNganhKinhTeCap3", setting},
                            {"LoaiKyHan", setting},
                            {"LaiSuatThuc", setting},
                            {"ThoiGianUuDaiLaiSuat", setting},
                            {"MaCTTD", setting },
                            {"TenCTTD", setting},
                            {"MaNhomNo", setting},
                            {"DuNoNgayQuyDoi", setting},
                            {"DuNoNgayNguyenTe", setting},
                            {"DuNoHomTruocQuyDoi", setting},
                            {"DuNoTuanTruocQuyDoi", setting},
                            {"DuNoCuoiThangTruocQuyDoi", setting},
                            {"DuNoCuoiNamTruocQuyDoi", setting},
                            {"DuNoBQThangQuyDoi", setting},
                            {"DuNoBQNamQuyDoi", setting},
                            {"DuNoBQNamTruocQuyDoi", setting},
                            {"DuNoBQThangTruocQuyDoi", setting},
                            {"GiaiNganThangQuyDoi", setting},
                            {"GiaiNganNamQuyDoi", setting},
                            {"SoTienTraNoGocTrongThangQuyDoi", setting},
                            {"SoTienTraNoGocTrongNamQuyDoi", setting},
                            {"SoTienTraLaiTrongThangQuyDoi", setting},
                            {"SoTienTraLaiTrongNamQuyDoi", setting},
                            {"MaChungLoai10", setting},
                            {"HanMucHopDongQuyDoi", setting},
                        }
            };

            // Act
            var reportTemplateDto = await _reportTemplatesAppService.AddReportToReportTemplateAsync(createReportDto);

            //Assert
            reportTemplateDto.Reports.Count.ShouldBe(reportCount + 1);
            reportTemplateDto.Reports.Last().Name.ShouldBe("Báo cáo tiền gửi 2");
            reportTemplateDto.Reports.Last().Setting.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateReportAsync_ShouldUpdateReport()
        {
            //Arrange
            var reportTemplate = await _reportTemplatesRepository.FirstOrDefaultAsync(r => r.ReportType.Equals(ReportType.Loan));
            var reportCount = reportTemplate.Reports.Count;
            var id = reportTemplate.Reports.FirstOrDefault(r => r.Name.Equals("Báo cáo tiền nợ 1")).Id;
            var newSetting = new SettingDto()
            {
                DataType = ReportItemDataType.String,
                IsVisible = false,
                IsQueryable = true
            };
            var editReportDto = new CreateEditReportDto
            {
                ReportType = ReportType.Loan,
                Name = "Báo cáo tiền gửi edit",
                Setting = new Dictionary<string, SettingDto>
                        {
                            {"Ngay", newSetting }, //update to false
                            {"TenNguon", newSetting },
                            {"MaPGD", newSetting},
                            {"SoCIF", newSetting},
                            {"TenKhachHang", newSetting},
                            {"MaPhanKhuc", newSetting},
                            {"SoTaiKhoan", newSetting},
                            {"LoaiTien", newSetting},
                            {"MaCanBoTinDung", newSetting},
                            {"NgayMoTaiKhoan", newSetting},
                            {"NgayDaoHan", newSetting},
                            {"NgayTraNoGanNhat", newSetting},
                            {"NgayTraNoTiepTheo", newSetting},
                            {"TenNganhKinhTeCap3", newSetting},
                            {"MaNganhKinhTeCap3", newSetting},
                            {"LoaiKyHan", newSetting},
                            {"LaiSuatThuc", newSetting},
                            {"ThoiGianUuDaiLaiSuat", newSetting},
                            {"MaCTTD", newSetting },
                            {"TenCTTD", newSetting},
                            {"MaNhomNo", newSetting},
                            {"DuNoNgayQuyDoi", newSetting},
                            {"DuNoNgayNguyenTe", newSetting},
                            {"DuNoHomTruocQuyDoi", newSetting},
                            {"DuNoTuanTruocQuyDoi", newSetting},
                            {"DuNoCuoiThangTruocQuyDoi", newSetting},
                            {"DuNoCuoiNamTruocQuyDoi", newSetting},
                            {"DuNoBQThangQuyDoi", newSetting},
                            {"DuNoBQNamQuyDoi", newSetting},
                            {"DuNoBQNamTruocQuyDoi", newSetting},
                            {"DuNoBQThangTruocQuyDoi", newSetting},
                            {"GiaiNganThangQuyDoi", newSetting},
                            {"GiaiNganNamQuyDoi", newSetting},
                            {"SoTienTraNoGocTrongThangQuyDoi", newSetting},
                            {"SoTienTraNoGocTrongNamQuyDoi", newSetting},
                            {"SoTienTraLaiTrongThangQuyDoi", newSetting},
                            {"SoTienTraLaiTrongNamQuyDoi", newSetting},
                            {"MaChungLoai10", newSetting},
                            {"HanMucHopDongQuyDoi", newSetting},
                        }
            };

            //Act
            var reportTemplateDto = await _reportTemplatesAppService.UpdateReportAsync(id, editReportDto);

            //Assert
            reportTemplateDto.Reports.Count.ShouldBe(reportCount);
            reportTemplateDto.Reports.FirstOrDefault(r => r.Id.Equals(id)).Name.ShouldBe("Báo cáo tiền gửi edit");
            var settingAttribute = (Dictionary<string,object>) reportTemplateDto.Reports.FirstOrDefault(r => r.Id.Equals(id)).Setting["Ngay"];
            settingAttribute["IsVisible"].ShouldBe(false);
        }

        [Fact]
        public async Task DeleteReportAsync_ShouldDeleteReport()
        {
            //Arrange
            var reportTemplate = await _reportTemplatesRepository.FirstOrDefaultAsync(r => r.ReportType.Equals(ReportType.Loan));
            var reportCount = reportTemplate.Reports.Count;
            var id = reportTemplate.Reports.FirstOrDefault(r => r.Name.Equals("Báo cáo tiền nợ 1")).Id;

            //Act
            await _reportTemplatesAppService.DeleteReportAsync(ReportType.Loan, id);

            //Assert
            _reportTemplatesRepository.FirstOrDefault(r => r.ReportType.Equals(ReportType.Loan)).Reports.Count.ShouldBe(reportCount - 1);
        }

        [Fact]
        public async Task GetReportTemplateAsync()
        {
            //Act
            var result = await _reportTemplatesAppService.GetReportTemplateAsync(ReportType.Loan);

            //Assert
            result.ReportType.ToString().ShouldBe("Loan");
            result.Template.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetListReportAsync()
        {
            //Act
            var result = await _reportTemplatesAppService.GetListReportAsync(ReportType.Loan);
            
            //Assert
            result.TotalCount.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetSettingVisibleAsync()
        {
            //Arrange
            var reportTemplate = await _reportTemplatesRepository.
                 FirstOrDefaultAsync(rp => rp.ReportType.Equals(ReportType.Loan));
            var report = reportTemplate.Reports.FirstOrDefault();

            //Act
            var result = await _reportTemplatesAppService.GetSettingVisibleAsync(ReportType.Loan, report.Id);

            //Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetSettingQueryabelAsync()
        {
            //Arrange
            var reportTemplate = await _reportTemplatesRepository.
                 FirstOrDefaultAsync(rp => rp.ReportType.Equals(ReportType.Loan));
            var report = reportTemplate.Reports.FirstOrDefault();

            //Act
            var result = await _reportTemplatesAppService.GetSettingQueryableAsync(ReportType.Loan, report.Id);

            //Assert
            result.ShouldNotBeNull();

            var queryableSetting = (Dictionary<string, object>)result.FirstOrDefault().Value;
            queryableSetting["IsQueryable"].ShouldBe(true);
        }

        [Fact]
        public async Task GetReportAsync()
        {
            //Arrange
            var reportTemplate = await _reportTemplatesRepository.
                 FirstOrDefaultAsync(rp => rp.ReportType.Equals(ReportType.Loan));
            var report = reportTemplate.Reports.FirstOrDefault();

            //Act
            var result = await _reportTemplatesAppService.GetReportAsync(ReportType.Loan, report.Id);

            //Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetTemplatesOfReportTemplateAsync()
        {
            //Act
            var result = await _reportTemplatesAppService.GetTemplatesOfReportTemplateAsync(ReportType.Loan);

            //Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetKeysOfReportTemplateAsync()
        {
            //Act
            var result = await _reportTemplatesAppService.GetKeysOfReportTemlateAsync(ReportType.Loan);

            //Assert
            result.ShouldNotBeNull();
        }
    }
}
