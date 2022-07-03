using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace BK2T.BankDataReporting
{
    public class BankDataReportingTestDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Department, Guid> _departmentsRepository;
        private readonly IRepository<ReportTemplate, Guid> _reportTemplatesRepository;
        private readonly DepartmentManager _departmentManager;
        private readonly IGuidGenerator _guidGenerator;

        public BankDataReportingTestDataSeedContributor(
            IRepository<Department, Guid> departmentsRepository,
            IRepository<ReportTemplate, Guid> reportTemplatesRepository,
            DepartmentManager departmentManager,
            IGuidGenerator guidGenerator
            )
        {
            _departmentsRepository = departmentsRepository;
            _reportTemplatesRepository = reportTemplatesRepository;
            _departmentManager = departmentManager;
            _guidGenerator = guidGenerator;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedDepartmentAsync();
            await SeedReportIntoReportTemplate();
        }
        public async Task SeedReportIntoReportTemplate()
        {
            var loanTemplate = await _reportTemplatesRepository.FindAsync(r => r.ReportType.Equals(ReportType.Loan));
            loanTemplate.Reports.Add(new Report(
                _guidGenerator.Create(),
                "Báo cáo tiền nợ 1",
                new BsonDocument
                        {
                            {"Ngay", new BsonDocument {
                                    { "Label", "Label" },
                                    { "IsVisible",true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } } },
                            {"TenNguon", new BsonDocument {
                                    { "Label", "Tên Nguồn" },
                                    { "IsVisible",true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } } },
                            {"MaPGD", new BsonDocument {
                                    { "Label", "Mã PGD" },
                                    { "IsVisible",true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } } },
                            {"SoCIF", new BsonDocument {
                                    { "Label", "Số CIF" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } } },
                            {"TenKhachHang", new BsonDocument {
                                    { "Label", "Tên Khách Hàng" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } } },
                            {"MaPhanKhuc", new BsonDocument {
                                    { "Label", "Mã Phân Khúc" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"SoTaiKhoan", new BsonDocument {
                                    { "Label", "Số Tài Khoản" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"LoaiTien", new BsonDocument {
                                    { "Label", "Loại Tiền" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"MaCanBoTinDung", new BsonDocument {
                                    { "Label", "Mã Cán Bộ Tín Dụng" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"NgayMoTaiKhoan", new BsonDocument {
                                    { "Label", "Ngày mở tài khoản" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"NgayDaoHan", new BsonDocument {
                                    { "Label", "Ngày đáo hạn" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"NgayTraNoGanNhat", new BsonDocument {
                                    { "Label", "Ngày trả nợ gần nhất" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"NgayTraNoTiepTheo", new BsonDocument {
                                    { "Label", "Ngày trả nợ tiếp theo" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"TenNganhKinhTeCap3", new BsonDocument {
                                    { "Label", "Tên ngành kinh tế cấp 3" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"MaNganhKinhTeCap3", new BsonDocument {
                                    { "Label", "Mã ngành kinh tế cấp 3" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"LoaiKyHan", new BsonDocument {
                                    { "Label", "Loại Kỳ Hạn" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"LaiSuatThuc", new BsonDocument {
                                    { "Label", "Lãi Suất Thực" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"ThoiGianUuDaiLaiSuat", new BsonDocument { 
                                    { "Label", "Thời gian ưu đã lãi suất" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"MaCTTD", new BsonDocument { 
                                    { "Label", "Mã CTTD" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } } },
                            {"TenCTTD", new BsonDocument { 
                                    { "Label", "Tên CTTD" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"MaNhomNo", new BsonDocument { 
                                    { "Label", "Mã Nhóm Nợ" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoNgayQuyDoi", new BsonDocument { 
                                    { "Label", "Dư nợ ngày quy đổi" }, 
                                    { "IsVisible", true },
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoNgayNguyenTe", new BsonDocument { 
                                    { "Label", "Dư nợ ngày nguyên tệ" }, 
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoHomTruocQuyDoi", new BsonDocument {
                                    { "Label", "Dư nợ hôm trước quy đổi" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoTuanTruocQuyDoi", new BsonDocument { 
                                    { "Label", "Dư nợ tuần trước quy đổi" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoCuoiThangTruocQuyDoi", new BsonDocument { 
                                    { "Label", "Dư nợ cuối tháng trước quy đổi" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoCuoiNamTruocQuyDoi", new BsonDocument { 
                                    { "Label", "Dư nợ cuối năm trước quy đổi" }, 
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoBQThangQuyDoi", new BsonDocument {
                                    { "Label", "Dư nợ BQ tháng quy đổi" },
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoBQNamQuyDoi", new BsonDocument {
                                    { "Label", "Dư nợ BQ năm quy đổi" }, 
                                    { "IsVisible", true },
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoBQNamTruocQuyDoi", new BsonDocument { 
                                    { "Label", "Dư nợ BQ năm trước quy đổi" },
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"DuNoBQThangTruocQuyDoi", new BsonDocument { 
                                    { "Label", "Dư nợ BQ tháng trước quy đổi" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"GiaiNganThangQuyDoi", new BsonDocument {
                                    { "Label", "Giải ngân tháng quy đổi" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"GiaiNganNamQuyDoi", new BsonDocument { 
                                    { "Label", "Giải ngân năm quy đổi" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"SoTienTraNoGocTrongThangQuyDoi", new BsonDocument { 
                                    { "Label", "Số tiền trả nợ gốc trong tháng quy đổi" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"SoTienTraNoGocTrongNamQuyDoi", new BsonDocument {
                                    { "Label", "Số tiền trả nợ gốc trong năm quy đổi" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"SoTienTraLaiTrongThangQuyDoi", new BsonDocument {
                                    { "Label", "Số tiền trả lãi trong tháng quy đổi" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"SoTienTraLaiTrongNamQuyDoi", new BsonDocument { 
                                    { "Label", "Số tiền trả lãi trong năm quy đổi" }, 
                                    { "IsVisible", true }, 
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                            {"MaChungLoai10", new BsonDocument { 
                                    { "Label", "Mã chủng loại 10" }, 
                                    { "IsVisible", true },
                                    { "IsQueryable", true },
                                    { "DataType", ReportItemDataType.Date } }},
                            {"HanMucHopDongQuyDoi", new BsonDocument { 
                                    { "Label", "Hạn mức hợp đồng quy đổi" },
                                    { "IsVisible", true },
                                    { "IsQueryable", true }, 
                                    { "DataType", ReportItemDataType.Date } }},
                        }
                ));
            await _reportTemplatesRepository.UpdateAsync(loanTemplate);
        }
        public async Task SeedDepartmentAsync()
        {
            await _departmentsRepository.InsertAsync(await
                _departmentManager.CreateAsync("001002003", "Data Collecting", "1002", new List<string> { "00" }));
        }
    }
}