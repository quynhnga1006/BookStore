using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace BK2T.BankDataReporting
{
    public class BankDataReportingSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IGuidGenerator _guidGenerator;

        public BankDataReportingSeederContributor(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IGuidGenerator guidGenerator)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _guidGenerator = guidGenerator;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedDepositTemplateAsync();
            await SeedLoanTemplateAsync();
            await SeedNIITemplateAsync();
            await SeedCollateralTemplateAsync();
            await SeedProvisionTemplateAsync();
            await SeedDebtDueCustomerItemsTemplateAsync();
            await SeedCustomerSalaryAsync();
            await SeedLifeInsuranceItemsTemplateAsync();
            await SeedNonLifeInsuranceItemsTemplateAsync();
            await SeedPersonalCustomerProductTemplateAsync();
            await SeedIpayCustomerTemplateAsync();
            await SeedForeignCurrencyTradingProfitTemplateAsync();
            await SeedEFastCustomerItemTemplateAsync();
            await SeedRetailDevelopmentCustomenrTemplateAsync();
            await SeedCardAcceptingUnitItemTemplateAsync();
        }

        public async Task SeedCardAcceptingUnitItemTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.CardAcceptingUnit))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.CardAcceptingUnit,
                new BsonDocument
                {
                    {
                        "MID",
                        new BsonDocument
                        {
                            {"Label", "MID" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MaPhong",
                        new BsonDocument
                        {
                            {"Label", "MaPhong" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MIDNAME",
                        new BsonDocument
                        {
                            {"Label", "MIDNAME" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MIDNAMEVOUCHER",
                        new BsonDocument
                        {
                            {"Label", "MIDNAMEVOUCHER" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MIDADDRESS",
                        new BsonDocument
                        {
                            {"Label", "MIDADDRESS" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MIDOWNER",
                        new BsonDocument
                        {
                            {"Label", "MIDOWNER" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MIDOWNERTEL",
                        new BsonDocument
                        {
                            {"Label", "MIDOWNERTEL" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "BRANCHNO",
                        new BsonDocument
                        {
                            {"Label", "BRANCHNO" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "BRANCH",
                        new BsonDocument
                        {
                            {"Label", "BRANCH" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "BRANCHNAME",
                        new BsonDocument
                        {
                            {"Label", "BRANCHNAME" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "FINACIALPROFILE",
                        new BsonDocument
                        {
                            {"Label", "FINACIALPROFILE" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MCC1",
                        new BsonDocument
                        {
                            {"Label", "MCC1" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "BUSSINESSTYPE",
                        new BsonDocument
                        {
                            {"Label", "BUSSINESSTYPE" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "CORP_ID",
                        new BsonDocument
                        {
                            {"Label", "CORP_ID" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "TID",
                        new BsonDocument
                        {
                            {"Label", "TID" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "PARENTTID",
                        new BsonDocument
                        {
                            {"Label", "PARENTTID" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "NAMELATIN",
                        new BsonDocument
                        {
                            {"Label", "NAMELATIN" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "DEVICETYPE",
                        new BsonDocument
                        {
                            {"Label", "DEVICETYPE" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "SERIAL",
                        new BsonDocument
                        {
                            {"Label", "SERIAL" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "ADDRESSLINE1",
                        new BsonDocument
                        {
                            {"Label", "ADDRESSLINE1" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "ADDRESSLINE2",
                        new BsonDocument
                        {
                            {"Label", "ADDRESSLINE2" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },                   
                    {
                        "INACTIVE",
                        new BsonDocument
                        {
                            {"Label", "INACTIVE" },
                            {"DataType", ReportItemDataType.Boolean }
                        }
                    },
                    {
                        "CREATEDATE",
                        new BsonDocument
                        {
                            {"Label", "CREATEDATE" },
                            {"DataType", ReportItemDataType.Date }
                        }
                    },
                    {
                        "SYNC_CMS_DATE",
                        new BsonDocument
                        {
                            {"Label", "SYNC_CMS_DATE" },
                            {"DataType", ReportItemDataType.Date }
                        }
                    },
                    {
                        "LAST_APPROVED",
                        new BsonDocument
                        {
                            {"Label", "LAST_APPROVED" },
                            {"DataType", ReportItemDataType.Date }
                        }
                    },
                    {
                        "TID_OPTION3",
                        new BsonDocument
                        {
                            {"Label", "TID_OPTION3" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "SAMPLETID",
                        new BsonDocument
                        {
                            {"Label", "SAMPLETID" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "STAFFID",
                        new BsonDocument
                        {
                            {"Label", "STAFFID" },
                            {"DataType", ReportItemDataType.String }
                        }
                    }
                }
                ));
        }

        public async Task SeedEFastCustomerItemTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.EFastCustomerItem))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.EFastCustomerItem,
                new BsonDocument
                {
                    {
                        "SoCIFDN",
                        new BsonDocument
                        {
                            {"Label", "Số CIF DN" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "TenDN",
                        new BsonDocument
                        {
                            {"Label", "Tên DN" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                    {
                        "MaPhong",
                        new BsonDocument
                        {
                            {"Label", "Mã phòng" },
                            {"DataType", ReportItemDataType.String }
                        }
                    },
                }
                ));
        }

        public async Task SeedPersonalCustomerProductTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.PersonalCustomerProduct))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.PersonalCustomerProduct,
                    new BsonDocument
                    {
                        {
                            "MaCN",
                            new BsonDocument
                            {
                                {"Label", "Mã CN" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "Ngay",
                            new BsonDocument
                            {
                                {"Label", "Ngày" },
                                {"DataType", ReportItemDataType.Date },
                            }
                        },
                        {
                            "SoCIF",
                            new BsonDocument
                            {
                                {"Label", "CIF" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "MaPhanKhuc",
                            new BsonDocument
                            {
                                {"Label", "Mã phân khúc" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "Cochovay",
                            new BsonDocument
                            {
                                {"Label", "1. Cờ cho vay" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoCASA",
                            new BsonDocument
                            {
                                {"Label", "2. Cờ CASA" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoFD",
                            new BsonDocument
                            {
                                {"Label", "3. Cờ FD" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoTheTinDungActive",
                            new BsonDocument
                            {
                                {"Label", "4. Cờ thẻ tín dụng active" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoTheGNQT12Thang",
                            new BsonDocument
                            {
                                {"Label", "5. Cờ thẻ GNQT 12 tháng" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoTheGNND12Thang",
                            new BsonDocument
                            {
                                {"Label", "6. Cờ thẻ GNNĐ 12 tháng" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoIPay",
                            new BsonDocument
                            {
                                {"Label", "7. Cờ iPay" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoIPayMobile",
                            new BsonDocument
                            {
                                {"Label", "7. Cờ iPay mobile" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoBDSD",
                            new BsonDocument
                            {
                                {"Label", "8. Cờ BĐSD" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoBaoHiemNhanTho",
                            new BsonDocument
                            {
                                {"Label", "9. Cờ Bảo hiểm nhân thọ" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoBaoHiemPhiNhanTho",
                            new BsonDocument
                            {
                                {"Label", "10. Cờ Bảo hiểm phi nhân thọ" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoBaoLanh_TTTM",
                            new BsonDocument
                            {
                                {"Label", "11. Cờ Bảo lãnh/ TTTM" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoMuaBanNgoaiTe",
                            new BsonDocument
                            {
                                {"Label", "12. Cờ Mua bán ngoại tệ" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoTaiKhoanTheoYeuCau",
                            new BsonDocument
                            {
                                {"Label", "13. Cờ Tài khoản theo yêu cầu" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoAlias",
                            new BsonDocument
                            {
                                {"Label", "13. Cờ Alias" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "CoSanPhamDauTu",
                            new BsonDocument
                            {
                                {"Label", "14. Cờ Sản phẩm đầu tư" },
                                {"DataType", ReportItemDataType.Boolean },
                            }
                        },
                        {
                            "TongSanPhamTheo2022_KHCN",
                            new BsonDocument
                            {
                                {"Label", "Tổng sản phẩm theo 2022 (KHCN)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        }
                        ,
                        {
                            "TongSanPhamTheo2022_KHUT",
                            new BsonDocument
                            {
                                {"Label", "Tổng sản phẩm theo 2022 (KHUT)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        }
                    }));
        }

        public async Task SeedProvisionTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.Provision))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.Provision,
                    new BsonDocument
                    {
                        {
                            "ChiNhanh",
                            new BsonDocument
                            {
                                {"Label", "Chi nhánh" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "PhongKinhDoanh",
                            new BsonDocument
                            {
                                {"Label", "Phòng kinh doanh" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "KhachHang",
                            new BsonDocument
                            {
                                {"Label", "Khách Hàng" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "Cif",
                            new BsonDocument
                            {
                                {"Label", "CIF" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "SoFact",
                            new BsonDocument
                            {
                                {"Label", "Số fact" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "MaTenPhanKhucKH",
                            new BsonDocument
                            {
                                {"Label", "Mã - tên phân khúc KH" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "SoTK",
                            new BsonDocument
                            {
                                {"Label", "Số TK" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "LoaiTien",
                            new BsonDocument
                            {
                                {"Label", "Loại tiền" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "GiaTriCuaCacKhoanNoNguyenTe",
                            new BsonDocument
                            {
                                {"Label", "Giá trị của các khoản nợ (nguyên tệ)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "GiaTriCuaCacKhoanNoQuyDoiVND",
                            new BsonDocument
                            {
                                {"Label", "Giá trị của các khoản nợ (quy đổi VNĐ) (A)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "GiaTriKhauTruCuaTSBD",
                            new BsonDocument
                            {
                                {"Label", "Giá trị khấu trừ của TSBD (C)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "GiaTriKhoanNoTrichDuPhong",
                            new BsonDocument
                            {
                                {"Label", "Giá trị khoản nợ trích dự phòng" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "SoNgayQuaHan",
                            new BsonDocument
                            {
                                {"Label", "Số ngày quá hạn" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoGocKyNay",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ gốc kỳ này" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoHangRuiRoKyNay",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ hạng rủi ro kỳ này" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoChiTietKyNay",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ chi tiết kỳ này" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoTheoCICKyNay",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ theo CIC kỳ này" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoCuoiCungKyNayTheoKhachHang",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ cuối cùng kỳ này (theo Khách hàng)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "TyLeTrichDuPhong",
                            new BsonDocument
                            {
                                {"Label", "Tỷ lệ % trích dự phòng" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "DPCTPhaiTrichTheoNhomNoCuoiCungKyNayTheoKhachHang",
                            new BsonDocument
                            {
                                {"Label", "DP CT phải trích theo nhóm nợ cuối cùng kỳ này (theo Khách hàng)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoCuoiCungBaoGomNhomNoGiuNguyenTheoTT01TT03",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ cuối cùng (bao gồm nhóm nợ giữ nguyên theo TT01/TT03)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "DPCTPhaiTrichTheoNhomNoCuoiCungBaoGomNhomNoGiuNguyenTheoTT01TT03",
                            new BsonDocument
                            {
                                {"Label", "DPCT phải trích theo Nhóm nợ cuối cùng (bao gồm nhóm nợ giữ nguyên theo TT01/TT03)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "Nguon",
                            new BsonDocument
                            {
                                {"Label", "Nguồn" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "NhomNoGocKyTruoc",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ gốc kỳ trước" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoHangRuiRoKyTruoc",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ hạng rủi ro kỳ trước" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoCICKyTruoc",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ CIC kỳ trước" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoChiTietKyTruoc",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ chi tiết kỳ trước" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoCuoiCungKyTruocTheoKhachHang",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ cuối kỳ trước (theo Khách hàng)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NhomNoDaCapNhatCICKyTruoc",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ đã cập nhật CIC kỳ trước" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "DPCTPhaiTrichTheoNhomNoNHCTTuPhanLoaiVaCICKyTruocTheoKhachHang",
                            new BsonDocument
                            {
                                {"Label", "DP CT phải trích theo nhóm nợ NHCT tự phân loại và CIC kỳ trước (theo Khách hàng)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "MaSoThueCMT",
                            new BsonDocument
                            {
                                {"Label", "Mã số thuế/CMT" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "MaTenThanhPhanKinhTe",
                            new BsonDocument
                            {
                                {"Label", "Mã - tên thành phần kinh tế" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "MaTenLoaiKhachHangICB",
                            new BsonDocument
                            {
                                {"Label", "Mã - tên loại khách hàng (ICB)" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "MaTenNganhKinhTe",
                            new BsonDocument
                            {
                                {"Label", "Mã - tên ngành kinh tế" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "MaTenNganhKinhTeChiTiet",
                            new BsonDocument
                            {
                                {"Label", "Mã - tên ngành kinh tế chi tiết" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "LaiSuat",
                            new BsonDocument
                            {
                                {"Label", "Lãi suất" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "LaiDuThuNoiBangQuyDoi",
                            new BsonDocument
                            {
                                {"Label", "Lãi dự thu nội bảng (quy đổi)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "LaiDuThuNgoaiBangQuyDoi",
                            new BsonDocument
                            {
                                {"Label", "Lãi dự thu ngoại bảng (quy đổi)" },
                                {"DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "KyHanKhoanVay",
                            new BsonDocument
                            {
                                {"Label", "Kỳ hạn khoản vay" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "MaTenSanPhamChiTiet",
                            new BsonDocument
                            {
                                {"Label", "Mã - tên sản phẩm chi tiết" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "TrichLapDPCT",
                            new BsonDocument
                            {
                                {"Label", "Trích lập DPCT? (Y - Có; N - Không)" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "TrichLapDPC",
                            new BsonDocument
                            {
                                {"Label", "Trích lập DPC? (Y - Có; N - Không)" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "NhomNoThuCong",
                            new BsonDocument
                            {
                                {"Label", "Nhóm nợ thủ công" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "LyDOSuaNhomNoThuCong",
                            new BsonDocument
                            {
                                {"Label", "Lý do sửa nhóm nợ thủ công" },
                                {"DataType", ReportItemDataType.String },
                            }
                        },
                        {
                            "NgayBatDau",
                            new BsonDocument
                            {
                                {"Label", "Ngày bắt đầu" },
                                {"DataType", ReportItemDataType.Date },
                            }
                        },
                        {
                            "NgayKetThuc",
                            new BsonDocument
                            {
                                {"Label", "Ngày kết thúc" },
                                {"DataType", ReportItemDataType.Date },
                            }
                        },
                    }));
        }
        public async Task SeedCollateralTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.Collateral))
            {
                return;
            }

            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.Collateral,
                new BsonDocument
                        {
                            {"SoCIF", new BsonDocument{
                                { "Label", "Số CIF"},
                                { "DataType", ReportItemDataType.Number}, } },
                            {"TenKhachHang", new BsonDocument{
                                { "Label", "Tên khách hàng"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"SoFact", new BsonDocument{
                                { "Label", "Số fact"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"SoTaiKhoan", new BsonDocument{
                                { "Label", "Số tài khoản"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"GiaTriKhoanNoQuyDoi", new BsonDocument{
                                { "Label", "Giá trị khoản nợ quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"LoaiTSBD", new BsonDocument {
                                { "Label", "Loại TSBĐ" },
                                { "DataType", ReportItemDataType.String}, }},
                            {"IDTSBD", new BsonDocument {
                                { "Label", "ID TSBĐ" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TenTSBD", new BsonDocument{
                                { "Label", "Tên TSBĐ"},
                                { "DataType", ReportItemDataType.String} } },
                            {"LoaiTien", new BsonDocument{
                                { "Label", "Loại tiền"},
                                { "DataType", ReportItemDataType.String} } },
                            {"GiaTriDinhGiaTS", new BsonDocument {
                                { "Label", "Giá trị định giá tài sản" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"GiaTriTSBDChiaSeTheoTaiKhoan ", new BsonDocument {
                                { "Label", "Giá trị TSBĐ chia sẻ theo tài khoản" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"NguonHinhThanhTaiSan", new BsonDocument {
                                { "Label", "Nguồn hành thành tài sản" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"ToChucDinhGiaTSBD", new BsonDocument {
                                { "Label", "Tổ chức định giá TSBĐ" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"NgayDinhGiaTSBD", new BsonDocument {
                                { "Label", "Ngày định giá TSBĐ" },
                                { "DataType", ReportItemDataType.Date }, }},
                            {"NgayDinhGiaTiepTheo", new BsonDocument {
                                { "Label", "Ngày định giá tiếp theo" },
                                { "DataType", ReportItemDataType.Date }, }},
                            {"TyLeKhauTruTheoTSBD", new BsonDocument {
                                { "Label", "Tỷ lệ khấu trừ theo TSBĐ" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TyLeChiaSeGiaTriTSBDChoHopDongTaiNHCT", new BsonDocument {
                                { "Label", "Tỷ lệ chia sẻ giá trị TSBĐ cho hợp đồng tại NHCT" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoKhauTruKhongKhauTru", new BsonDocument {
                                { "Label", "Có khấu trừ/không có khấu trừ" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CanBoKiemSoatBoTichKhauTruTSBD", new BsonDocument {
                                { "Label", "Cán bộ kiểm soát bỏ tích khấu trừ TSBĐ" },
                                { "DataType", ReportItemDataType.String }, }},
                        }));
        }
        //String: 0, Number: 1, Date: 2
        public async Task SeedNIITemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.NII))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.NII,
                    new BsonDocument
                    {
                        {
                            "ThuNhapTuLaiThangQuyDoi",
                            new BsonDocument {
                                { "Label", "Thu nhập từ lãi tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "ChiPhiLaiThangQuyDoi",
                            new BsonDocument {
                                { "Label", "Chi phí lãi tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "ChiPhiLaiThangQuyDoiLuyKe",
                            new BsonDocument {
                                { "Label", "Chi phí lãi tháng quy đổi lũy kế" },
                                { "DataType", ReportItemDataType.Number },
                                { "CumulativeFrom", "ChiPhiLaiThangQuyDoi" },
                            }
                        },
                        {
                            "ThuNhapFTPThangQuyDoi",
                            new BsonDocument {
                                { "Label", "Thu nhập FTP tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number },
}
},
                        {
                            "ThuNhapFTPThangQuyDoiLuyKe",
                            new BsonDocument {
                                { "Label", "Thu nhập FTP tháng quy đổi lũy kế" },
                                { "DataType", ReportItemDataType.Number },
                                { "CumulativeFrom", "ThuNhapFTPThangQuyDoi" },
                            }
                        },
                        {
                            "ThuNhapLaiThuanQuyDoi",
                            new BsonDocument {
                                { "Label", "Thu nhập lãi thuần (NII) quy đổi" },
                                { "DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "ThuNhapLaiThuanQuyDoiLuyKe",
                            new BsonDocument {
                                { "Label", "NII Quy đổi lũy kế" },
                                { "DataType", ReportItemDataType.Number },
                                { "CumulativeFrom", "ThuNhapLaiThuanQuyDoi" },
                            }
                        },
                        {
                            "ChiPhiFTPThangQuyDoi",
                            new BsonDocument {
                                { "Label", "Chi phí FTP tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NIIChoVayThangQuyDoi",
                            new BsonDocument {
                                { "Label", "NII cho vay tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number },
                            }
                        },
                        {
                            "NIITienGuiThangQuyDoi",
                            new BsonDocument {
                                { "Label", "NII tiền gửi tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number },
                            }
                        },
                    }
                    )
                );
        }
        public async Task SeedLoanTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.Loan))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.Loan,
                    new BsonDocument
                        {
                            {"DepartmentId", new BsonDocument{
                                { "Label", "Mã Phòng"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"UserId", new BsonDocument{
                                { "Label", "Mã cán bộ"},
                                { "DataType",ReportItemDataType.String }, } },
                            {"Ngay", new BsonDocument{
                                { "Label", "Ngày"},
                                { "DataType", ReportItemDataType.Date}, } },
                            {"TenNguon", new BsonDocument{
                                { "Label", "Tên nguồn"},
                            { "DataType", ReportItemDataType.String}, } },
                            {"MaPGD", new BsonDocument{
                                { "Label", "Mã PGD"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"MaPhongBanGoc", new BsonDocument{
                                { "Label", "Mã phòng ban gốc"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"SoCIF", new BsonDocument{
                                { "Label", "Số CIF"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKhachHang", new BsonDocument{
                                { "Label", "Tên khách hàng"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"MaPhanKhuc", new BsonDocument{
                                { "Label", "Mã phân khúc"},
                                { "DataType", ReportItemDataType.String} } },
                            {"SoTaiKhoan", new BsonDocument{
                                { "Label", "Số tài khoản"},
                                { "DataType", ReportItemDataType.String} } },
                            {"LoaiTien", new BsonDocument{
                                { "Label", "Loại tiền"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaCanBoTinDung", new BsonDocument{
                                { "Label", "Mã cán bộ tín dụng"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaCanBoGoc", new BsonDocument{
                                { "Label", "Mã cán bộ gốc"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"NgayMoTaiKhoan", new BsonDocument{
                                { "Label", "Ngày mở tài khoản"},
                                { "DataType", ReportItemDataType.Date} } },
                            {"NgayDaoHan", new BsonDocument{
                                { "Label", "Ngày đáo hạn"},
                                { "DataType", ReportItemDataType.Date} } },
                            {"NgayTraNoGanNhat", new BsonDocument{
                                { "Label", "Ngày trả nợ gần nhất"},
                                { "DataType", ReportItemDataType.Date} } },
                            {"NgayTraNoTiepTheo", new BsonDocument{
                                { "Label", "Ngày trả nợ tiếp theo"},
                                { "DataType", ReportItemDataType.Date} } },
                            {"TenNganhKinhTeCap3", new BsonDocument{
                                { "Label", "Tên ngành kinh tế cấp 3"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaNganhKinhTeCap3", new BsonDocument{
                                { "Label", "Mã ngành kinh tế cấp 3"},
                                { "DataType", ReportItemDataType.String} } },
                            {"LoaiKyHan", new BsonDocument{
                                { "Label", "Loại kỳ hạn"},
                                { "DataType", ReportItemDataType.String} } },
                            {"LaiSuatThuc", new BsonDocument{
                            { "Label", "Lãi suất thực"},
                                { "DataType", ReportItemDataType.Number } } },
                            {"ThoiGianUuDaiLaiSuat", new BsonDocument{
                                { "Label", "Thời gian ưu đãi lãi suất"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"MaCTTD", new BsonDocument{
                                { "Label", "Mã CTTD"},
                                { "DataType", ReportItemDataType.String} } },
                            {"TenCTTD", new BsonDocument{
                                { "Label", "Tên CTTD"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaNhomNo", new BsonDocument{
                                { "Label", "Mã nhóm nợ"},
                                { "DataType", ReportItemDataType.String} } },
                            {"DuNoNgayQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ ngày quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoNgayNguyenTe", new BsonDocument{
                                { "Label", "Dư nợ ngày nguyên tệ"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoHomTruocQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ hôm trước quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoTuanTruocQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ tuần trước quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoCuoiThangTruocQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ cuối tháng trước quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoCuoiNamTruocQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ cuối năm trước quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoBQThangQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ BQ tháng quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoBQNamQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ BQ năm quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoBQNamTruocQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ BQ năm trước quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"DuNoBQThangTruocQuyDoi", new BsonDocument{
                                { "Label", "Dư nợ BQ tháng trước quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"GiaiNganThangQuyDoi", new BsonDocument{
                            { "Label", "Giải ngân tháng quy đổi"},
                            { "DataType", ReportItemDataType.Number} } },
                            {"GiaiNganNamQuyDoi", new BsonDocument{
                                { "Label", "Giải ngân năm quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"SoTienTraNoGocTrongThangQuyDoi",new BsonDocument{
                                { "Label", "Số tiền trả nợ gốc trong tháng quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"SoTienTraNoGocTrongNamQuyDoi",new BsonDocument{
                                { "Label", "Số tiền trả nợ gốc trong năm quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"SoTienTraLaiTrongThangQuyDoi",new BsonDocument{
                                { "Label", "Số tiền trả lãi trong tháng quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"SoTienTraLaiTrongNamQuyDoi",new BsonDocument{
                                { "Label", "Số tiền trả lãi trong năm quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"MaChungLoai10",new BsonDocument{
                                { "Label", "Mã chủng loại 10"},
                                { "DataType", ReportItemDataType.String} } },
                            {"HanMucHopDongQuyDoi",new BsonDocument{
                                { "Label", "Hạn mức hợp đồng quy đổi"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"TangGiamDuNoSoVoiNgay",new BsonDocument{
                                { "Label", "Tăng/giảm dư nợ so với ngày"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"TangGiamDuNoSoVoiThang",new BsonDocument{
                                { "Label", "Tăng/giảm dư nợ so với tháng"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"TangGiamDuNoSoVoiNam",new BsonDocument{
                                { "Label", "Tăng/giảm dư nợ so với năm"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"TangGiamDuNoBQSoVoiThang",new BsonDocument{
                                { "Label", "Tăng/giảm dư nợ BQ so với tháng"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"TangGiamDuNoBQSoVoiNam",new BsonDocument{
                                { "Label", "Tăng/giảm dư nợ BQ so với năm"},
                                { "DataType", ReportItemDataType.Number} } },
                        }));
        }
        public async Task SeedDepositTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.Deposit))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.Deposit,
                    new BsonDocument
                        {
                            {"DepartmentId", new BsonDocument{
                                { "Label", "Mã phòng"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"UserId", new BsonDocument{
                                { "Label", "Mã cán bộ"},
                                { "DataType",ReportItemDataType.String }, } },
                            {"Ngay", new BsonDocument{
                                { "Label", "Ngày"},
                                { "DataType", ReportItemDataType.Date}, } },
                            {"MaPGD", new BsonDocument{
                                { "Label", "Mã PGD"},
                                { "DataType",ReportItemDataType.String }, } },
                            {"SoCIF", new BsonDocument{
                                { "Label", "Số CIF"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKhachHang", new BsonDocument{
                                { "Label", "Tên khách hàng"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"MaPhanKhuc", new BsonDocument{
                                { "Label", "Mã phân khúc"},
                                { "DataType", ReportItemDataType.String} } },
                            {"LoaiKhachHang", new BsonDocument{
                                { "Label", "Loại KH (Cá nhân/Doanh nghiệp)"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"SoTaiKhoan", new BsonDocument{
                            { "Label", "Số tài khoản"},
                                { "DataType", ReportItemDataType.String} } },
                            {"LoaiTien", new BsonDocument{
                                { "Label", "Loại tiền"},
                                { "DataType", ReportItemDataType.String} } },
                            {"KyHan", new BsonDocument {
                                { "Label", "Kỳ hạn" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"LaiSuat", new BsonDocument {
                                { "Label", "Lãi suất" },
                                { "DataType", ReportItemDataType.Number}, }},
                            {"NgayMoTaiKhoan", new BsonDocument{
                                { "Label", "Ngày mở tài khoản"},
                                { "DataType", ReportItemDataType.Date} } },
                            {"NgayDaoHan", new BsonDocument{
                                { "Label", "Ngày đáo hạn"},
                                { "DataType", ReportItemDataType.Date} } },
                            {"MaSanPham", new BsonDocument {
                                { "Label", "Mã sản phẩm" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"TenSanPham", new BsonDocument {
                                { "Label", "Tên sản phẩm" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"MaPhongBanGoc", new BsonDocument {
                                { "Label", "Mã phòng ban gốc (VCRM)" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"MaCanBoGoc", new BsonDocument {
                                { "Label", "Mã cán bộ gốc (VCRM)" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"SoDuTienGuiNgayQuyDoi", new BsonDocument {
                                { "Label", "Số dư tiền gửi ngày quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuTienGuiBQThangQuyDoi", new BsonDocument {
                                { "Label", "Số dư  tiền gửi BQ tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuTienGuiBQNamQuyDoi", new BsonDocument {
                                { "Label", "Số dư  tiền gửi BQ năm quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuQuyDoiNgayHomTruoc", new BsonDocument {
                                { "Label", "Số dư quy đổi ngày hôm trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuQuyDoiTuanTruoc", new BsonDocument {
                                { "Label", "Số dư quy đổi tuần trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuQuyDoiThangTruoc", new BsonDocument {
                            { "Label", "Số dư quy đổi tháng trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuQuyDoiNamTruoc", new BsonDocument {
                                { "Label", "Số dư quy đổi năm trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuTienGuiKhongKyHanNgayQuyDoi", new BsonDocument {
                                { "Label", "Số dư tiền gửi không kỳ hạn ngày quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuTienGuiKhongKyHanBQThangQuyDoi", new BsonDocument {
                                { "Label", "Số dư tiền gửi không kỳ hạn BQ tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuTienGuiKhongKyHanBQNamQuyDoi", new BsonDocument {
                                { "Label", "Số dư tiền gửi không kỳ hạn BQ năm quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"LaiDuTraQuyDoi", new BsonDocument {
                                { "Label", "Lãi dự trả quy đổi" },
                                { "DataType", ReportItemDataType.Number}, }},
                            {"SoDuTienGuiBQThangTruocQuyDoi", new BsonDocument {
                                { "Label", "Số dư tiền gửi BQ tháng trước quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuTienGuiBQNamTruocQuyDoi", new BsonDocument {
                                { "Label", "Số dư tiền gửi BQ năm trước quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoDuTienGuiBQTuanTruocQuyDoi", new BsonDocument {
                                { "Label", "Số dư tiền gửi BQ tuần trước quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoTienGuiVaoTrongThangQuyDoi", new BsonDocument {
                                { "Label", "Số tiền gửi vào trong tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"SoTienRutRaTrongThangQuyDoi", new BsonDocument {
                                { "Label", "Số tiền rút ra trong tháng quy đổi" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TangGiamSoDuTienGuiSoVoiNgayHomTruoc", new BsonDocument {
                                { "Label", "Tăng/giảm số dư tiền gửi so với ngày hôm trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TangGiamSoDuTienGuiSoVoiThangTruoc", new BsonDocument {
                                { "Label", "Tăng/giảm số dư tiền gửi so với tháng trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TangGiamSoDuTienGuiSoVoiNamTruoc", new BsonDocument {
                                { "Label", "Tăng/giảm số dư tiền gửi so với năm trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TangGiamSoDuTienGuiBQSoVoiThangTruoc", new BsonDocument {
                                { "Label", "Tăng/giảm số dư tiền gửi BQ so với tháng trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TangGiamSoDuTienGuiBQSoVoiNamTruoc", new BsonDocument {
                                { "Label", "Tăng/giảm số dư tiền gửi BQ so với năm trước" },
                                { "DataType", ReportItemDataType.Number }, }},
                        }));
        }
        public async Task SeedDebtDueCustomerItemsTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.DebtDueCustomer))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.DebtDueCustomer,
                    new BsonDocument
                        {
                            {"STT", new BsonDocument{
                                { "Label", "STT"},
                                { "DataType", ReportItemDataType.Number}, } },
                            {"SoCIF", new BsonDocument{
                                { "Label", "Số CIF"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKhachHang", new BsonDocument{
                                { "Label", "Tên khách hàng"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"SoDienThoai", new BsonDocument{
                                { "Label", "Số điện thoại"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"SoTaiKhoan", new BsonDocument{
                                { "Label", "Số tài khoản"},
                                { "DataType", ReportItemDataType.String} } },
                            {"CanBoQLTaiKhoan", new BsonDocument{
                                { "Label", "Cán bộ quản lý tài khoản"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"MaCN", new BsonDocument{
                                { "Label", "Mã CN"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaDonVi", new BsonDocument{
                                { "Label", "Mã Đơn Vị"},
                                { "DataType", ReportItemDataType.String} } },
                            {"PhanKhuc", new BsonDocument {
                                { "Label", "Phân Khúc" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"NhomNoTK", new BsonDocument {
                                { "Label", "Nhóm nợ TK" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"NhomNoKhachHang", new BsonDocument{
                                { "Label", "Nhóm Nợ Khách Hàng"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"LoaiTien", new BsonDocument{
                                { "Label", "Loại Tiền"},
                                { "DataType", ReportItemDataType.String} } },
                            {"NgayDenHanGoc", new BsonDocument {
                                { "Label", "Ngày đến hạn gốc" },
                                { "DataType", ReportItemDataType.Date }, }},
                            {"DuNoGocNguyenTe", new BsonDocument {
                                { "Label", "Dư nợ gốc nguyên tệ" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"DuNoGocQuyDoiVND", new BsonDocument {
                                { "Label", "Dư nợ gốc quy đổi VND" },
                                { "DataType", ReportItemDataType.Number}, }},
                            {"NgayDenHanLai", new BsonDocument {
                                { "Label", "Ngày đến hạn lãi" },
                                { "DataType", ReportItemDataType.Date }, }},
                            {"DuNoLaiNguyenTe", new BsonDocument {
                            { "Label", "Dư nợ lãi nguyên tệ" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"DuNoLaiQuydoiVND", new BsonDocument {
                                { "Label", "Dư nợ lãi quy đổi VND" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"GocDenHanNguyenTe", new BsonDocument {
                                { "Label", "Gốc đến hạn nguyên tệ" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"LaiDenHanNguyenTe", new BsonDocument {
                                { "Label", "Lãi đến hạn nguyên tệ" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TongGocLaiDenHan", new BsonDocument {
                                { "Label", "Tổng gốc lãi đến hạn" },
                                { "DataType", ReportItemDataType.Number }, }},
                        }));
        }
        public async Task SeedLifeInsuranceItemsTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.LifeInsurance))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.LifeInsurance,
                new BsonDocument
                {
                    {
                        "MaCN", new BsonDocument
                        {
                            {"label", "Mã CN" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "KhuVuc", new BsonDocument
                        {
                            {"label", "KHU VỰC" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "TenCN", new BsonDocument
                        {
                            {"label", "Tên CN" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "MaPGD", new BsonDocument
                        {
                            {"label", "Mã PGD (CODE VTB)" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "ChiNhanh", new BsonDocument
                        {
                            {"label", "CHI NHÁNH (AWS của Manulife)" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "MaLead", new BsonDocument
                        {
                            {"label", "MÃ LEAD" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "MaSoHopDong", new BsonDocument
                        {
                            {"label", "MÃ SỐ HỢP ĐỒNG" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "MaSanPham", new BsonDocument
                        {
                            {"label", "MÃ SẢN PHẨM" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "TenSanPham", new BsonDocument
                        {
                            {"label", "TÊN SẢN PHẨM" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "MaSanPhamBoTro", new BsonDocument
                        {
                            {"label", "MÃ SẢN PHẨM BỘ TRỢ" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "TinhTrangHopDong", new BsonDocument
                        {
                            {"label", "TÌNH TRẠNG HỢP ĐỒNG (ghi nhận Trả phí)" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "NgayNop", new BsonDocument
                        {
                            {"label", "NGÀY NỘP" },
                            {"DataType", ReportItemDataType.Date },
                        }
                    },
                    {
                        "NgayPhatHanh", new BsonDocument
                        {
                            {"label", "NGÀY PHÁT HÀNH" },
                            {"DataType", ReportItemDataType.Date },
                        }
                    },
                    {
                        "NgayHieuLuc", new BsonDocument
                        {
                            {"label", "NGÀY HIỆU LỰC" },
                            {"DataType", ReportItemDataType.Date },
                        }
                    },
                    {
                        "NgayChamDutHD", new BsonDocument
                        {
                            {"label", "NGÀY CHẤM DỨT HĐ" },
                            {"DataType", ReportItemDataType.Date },
                        }
                    },
                    {
                        "NgayXacNhanBanGiaoHD", new BsonDocument
                        {
                            {"label", "NGÀY XÁC NHẬN BÀN GIAO HĐ" },
                            {"DataType", ReportItemDataType.Date },
                        }
                    },
                    {
                        "DinhKiDongPhi", new BsonDocument
                        {
                            {"label", "ĐỊNH KÌ ĐÓNG PHÍ" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "PhiBanDau", new BsonDocument
                        {
                            {"label", "PHÍ BAN ĐẦU (IP)" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "PhiQuyNam", new BsonDocument
                        {
                            {"label", "PHÍ QUY NĂM (APE)" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "PhiDauTu", new BsonDocument
                        {
                            {"label", "PHÍ ĐẦU TƯ (10% TOP UP)" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "HoTenKhachHang", new BsonDocument
                        {
                            {"label", "HỌ TÊN KHÁCH HÀNG" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "MaSoTuVan", new BsonDocument
                        {
                            {"label", "MÃ SỐ TƯ VẤN" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "HoTenTuVan", new BsonDocument
                        {
                            {"label", "HỌ TÊN TƯ VẤN" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "PhoGiamDoc", new BsonDocument
                        {
                            {"label", "PHÓ GIÁM ĐỐC" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "PhongGiaoDich", new BsonDocument
                        {
                            {"label", "PHÒNG GIAO DỊCH" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "TruongPhong", new BsonDocument
                        {
                            {"label", "TRƯỞNG PHÒNG" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "PhoPhong", new BsonDocument
                        {
                            {"label", "PHÓ PHÒNG" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "NhanVienNganHang", new BsonDocument
                        {
                            {"label", "NHÂN VIÊN NGÂN HÀNG" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "QLKinhDoanh", new BsonDocument
                        {
                            {"label", "QL KINH DOANH" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "QLKinhDoanhKhuVuc", new BsonDocument
                        {
                            {"label", "QL KINH DOANH KHU VỰC" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "HinhThucNop", new BsonDocument
                        {
                            {"label", "HÌNH THỨC NỘP" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                }));
        }

        public async Task SeedNonLifeInsuranceItemsTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.NonLifeInsurance))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.NonLifeInsurance,
                new BsonDocument
                {
                    {
                        "Stt", new BsonDocument
                        {
                            {"label", "STT" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "DonViDoiTacQuanLy", new BsonDocument
                        {
                            {"label", "Đơn vị đối tác quản lý" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    },
                    {
                        "MaPhong", new BsonDocument
                        {
                            {"label", "Mã phòng" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "DonViCapDon", new BsonDocument
                        {
                            {"label", "Đơn vị cấp đơn" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "SanPham", new BsonDocument
                        {
                            {"label", "Sản phẩm" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "SoHopDongVBI", new BsonDocument
                        {
                            {"label", "Số hợp đồng VBI" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "NgayPhatSinh", new BsonDocument
                        {
                            {"label", "Ngày phát sinh" },
                            {"DataType", ReportItemDataType.Date },
                        }
                    },
                    {
                        "TenKhachHang", new BsonDocument
                        {
                            {"label", "Tên khách hàng" },
                            {"DataType", ReportItemDataType.String },
                        }
                    },
                    {
                        "PhiBaoHiem", new BsonDocument
                        {
                            {"label", "Phí bảo hiểm" },
                            {"DataType", ReportItemDataType.Number },
                        }
                    }
                }));
        }
        public async Task SeedCustomerSalaryAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.CustomerSalary))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.CustomerSalary,
                    new BsonDocument
                        {
                            {"SoCIFNhanLuong", new BsonDocument{
                                { "Label", "Số CIF nhận lương"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKhachHangNhanLuong", new BsonDocument{
                                { "Label", "Tên khách hàng nhận lương"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"MaPhongQuanLyTKCBNVNhanLuong", new BsonDocument{
                                { "Label", "Mã phòng QL TK CBNV nhận lương"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"CIFTraLuong", new BsonDocument{
                                { "Label", "CIF trả lương"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenCIFTraLuong", new BsonDocument{
                                { "Label", "Tên CIF trả lương"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaPhongQLKHDNChiLuong", new BsonDocument{
                                { "Label", "Mã phòng QLKHDN chi lương"},
                                { "DataType", ReportItemDataType.String} } },
                        }));
        }
        public async Task SeedIpayCustomerTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.IpayCustomer))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.IpayCustomer,
                new BsonDocument
                        {
                            {"NgayBaoCao", new BsonDocument{
                                { "Label", "Ngày báo cáo"},
                                { "DataType", ReportItemDataType.Date}, } },
                            {"SoCIF", new BsonDocument{
                                { "Label", "Số CIF"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKH", new BsonDocument{
                                { "Label", "Tên KH"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaCBGDV", new BsonDocument{
                                { "Label", "Mã CB/GDV trên core"},
                                { "DataType", ReportItemDataType.String} } },
                            {"UserADCanBo", new BsonDocument{
                                { "Label", "User AD cán bộ"},
                                { "DataType", ReportItemDataType.String} } },
                            {"NgayDKiPay", new BsonDocument {
                                { "Label", "Ngày ĐK iPay" },
                                { "DataType", ReportItemDataType.Date}, }},
                            {"KenhDK", new BsonDocument {
                                { "Label", "Kênh ĐK" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"TrangThai", new BsonDocument{
                                { "Label", "Trạng thái"},
                                { "DataType", ReportItemDataType.String} } },
                            {"NgayDangNhapIPayGanNhat", new BsonDocument{
                                { "Label", "Ngày đăng nhập iPay gần nhất"},
                                { "DataType", ReportItemDataType.Date} } },
                            {"DkOTT", new BsonDocument {
                                { "Label", "ĐK OTT" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"DkSoftOTP", new BsonDocument {
                                { "Label", "ĐK Soft OTP" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"SuDungIPayMobile", new BsonDocument {
                                { "Label", "Sử dụng iPay mobile" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"GDTC12M", new BsonDocument {
                                { "Label", "GDTC 12M" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"eKYC", new BsonDocument {
                                { "Label", "eKYC" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"GDTC3M", new BsonDocument {
                                { "Label", "GDTC 3M" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"GDTC2M", new BsonDocument {
                                { "Label", "GDTC 2M" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"MaPhong", new BsonDocument {
                                { "Label", "Mã phòng" },
                                { "DataType", ReportItemDataType.String }, }},
                        })
                {

                });
        }
        public async Task SeedRetailDevelopmentCustomenrTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.RetailDevelopmentCustomer))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.RetailDevelopmentCustomer,
                new BsonDocument
                        {
                            {"STT", new BsonDocument{
                                { "Label", "STT"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"CIF", new BsonDocument{
                                { "Label", "CIF"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKH", new BsonDocument{
                                { "Label", "Tên KH"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaPhong", new BsonDocument{
                                { "Label", "Mã phòng"},
                                { "DataType", ReportItemDataType.String} } },
                }));
        }
        public async Task SeedForeignCurrencyTradingProfitTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.ForeignCurrencyTradingProfit))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.ForeignCurrencyTradingProfit,
                    new BsonDocument
                        {
                            {"SoCIF", new BsonDocument{
                                { "Label", "Số CIF"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKH", new BsonDocument{
                                { "Label", "Tên KH"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"PhanKhucKH", new BsonDocument {
                                { "Label", "Phân Khúc KH" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"LoiNhuanCN", new BsonDocument {
                                { "Label", "Lợi nhuận CN" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"MaPhong", new BsonDocument{
                                { "Label", "Mã phòng"},
                                { "DataType", ReportItemDataType.String} } },
                        }));
        }

        public async Task SeedCorporateCustomerItemsTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.CorporateCustomer))
            { 
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                    ReportType.CorporateCustomer,
                    new BsonDocument
                        {
                            {"SoCIF", new BsonDocument{
                                { "Label", "Số CIF"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TenKhachHang", new BsonDocument{
                                { "Label", "Tên khách hàng"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"TuoiNganHang", new BsonDocument{
                                { "Label", "Tuổi ngân hàng"},
                                { "DataType", ReportItemDataType.Number}, } },
                            {"TuoiKhachHang", new BsonDocument{
                                { "Label", "Tuổi khách hàng"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"MaPhanKhuc", new BsonDocument{
                                { "Label", "Mã phân khúc"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"NhomPhanKhucKHDN", new BsonDocument{
                                { "Label", "Nhóm phân khúc KHDN"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"CoNhuCauTaiTroVon", new BsonDocument{
                                { "Label", "Cờ Nhu cầu tài trợ vốn"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"CoNhuCauPhatTrienChuoiCungUng", new BsonDocument{
                                { "Label", "Cờ Nhu cầu phát triển chuỗi cung ứng"},
                                { "DataType", ReportItemDataType.Number} } },
                            {"CoNhuCauQuanLyDongTien", new BsonDocument {
                                { "Label", "Cờ Nhu cầu quản lý dòng tiền" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoNhuCauDauTuTaiChinh", new BsonDocument {
                                { "Label", "Cờ Nhu cầu đầu tư tài chính" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoNhuCauVeTaiKhoan", new BsonDocument {
                                { "Label", "Cờ Nhu cầu về tài khoản" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoNhuCauQuanTriThanhKhoan", new BsonDocument {
                                { "Label", "Cờ Nhu cầu quản trị thanh khoản" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoNhuCauTTTM", new BsonDocument {
                                { "Label", "Cờ Nhu cầu TTTM" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoNhuCauBaoLanh", new BsonDocument {
                                { "Label", "Cờ Nhu cầu bảo lãnh" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoNhuCauGiaoDichDienTu", new BsonDocument {
                                { "Label", "Cờ Nhu cầu giao dịch điện tử" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CoNhuCauQuanTriRuiRo", new BsonDocument {
                                { "Label", "Cờ Nhu cầu quản trị rủi ro" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"TongNhuCauDapUngChoKHDN", new BsonDocument {
                                { "Label", "Tổng nhu cầu đáp ứng cho KHDN" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"MaPhong", new BsonDocument {
                                { "Label", "Mã phòng" },
                                { "DataType", ReportItemDataType.String }, }},
                        }));
        }
        public async Task SeedCreditCardTemplateAsync()
        {
            if (await _reportTemplateRepository.AnyAsync(rp => rp.ReportType == ReportType.CreditCard))
            {
                return;
            }
            await _reportTemplateRepository.InsertAsync(
                new ReportTemplate(_guidGenerator.Create(),
                ReportType.CreditCard,
                new BsonDocument
                        {
                            {"STT", new BsonDocument{
                                { "Label", "Số thứ tự"},
                                { "DataType", ReportItemDataType.Number}, } },
                            {"CARDBRANCH", new BsonDocument{
                                { "Label", "CARD BRANCH"},
                                { "DataType", ReportItemDataType.String}, } },
                            {"CARDGROUP", new BsonDocument{
                                { "Label", "CARD GROUP"},
                                { "DataType", ReportItemDataType.String} } },
                            {"CARDSUBGROUP", new BsonDocument{
                                { "Label", "CARD SUBGROUP"},
                                { "DataType", ReportItemDataType.String} } },
                            {"CARDTYPE", new BsonDocument{
                                { "Label", "CARD TYPE"},
                                { "DataType", ReportItemDataType.String} } },
                            {"PAN", new BsonDocument {
                                { "Label", "PAN" },
                                { "DataType", ReportItemDataType.String}, }},
                            {"NAMEONCARD", new BsonDocument {
                                { "Label", "NAME ON CARD" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"loc", new BsonDocument{
                                { "Label", "loc"},
                                { "DataType", ReportItemDataType.String} } },
                            {"CBQL", new BsonDocument{
                                { "Label", "Cán bộ quản lý"},
                                { "DataType", ReportItemDataType.String} } },
                            {"MaPhong", new BsonDocument {
                                { "Label", "Mã phòng" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"SIGNSTAT", new BsonDocument {
                                { "Label", "Sign STAT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CRDSTAT", new BsonDocument {
                                { "Label", "CRD STAT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"PINCHANGE", new BsonDocument {
                                { "Label", "PIN CHANGE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"GROUPLIMIT", new BsonDocument {
                                { "Label", "GROUP LIMIT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"FINPROFILE", new BsonDocument {
                                { "Label", "FIN PROFILE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CREATEDATE", new BsonDocument {
                                { "Label", "CREATE DATE" },
                                { "DataType", ReportItemDataType.Date }, }},
                            {"EXPIRATIONDATE", new BsonDocument {
                                { "Label", "EXPIRATION DATE" },
                                { "DataType", ReportItemDataType.Date }, }},
                            {"ISSFEE", new BsonDocument {
                                { "Label", "IS FEE" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"PHYSICIALCARD", new BsonDocument {
                                { "Label", "PHYSICIAL CARD" },
                                { "DataType", ReportItemDataType.Boolean }, }},
                            {"REASONREISSUE", new BsonDocument {
                                { "Label", "REASONREISSUE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"ISSUSER", new BsonDocument {
                                { "Label", "ISS USER" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"ACCOUNTNO", new BsonDocument {
                                { "Label", "ACCOUNTNO" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"ACCBRANCH", new BsonDocument {
                                { "Label", "ACC BRANCH" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CRD", new BsonDocument {
                                { "Label", "CRD" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CREDITLIMIT/ LGBALANCE", new BsonDocument {
                                { "Label", "CREDIT LIMIT/ LG BALANCE" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"AVAILABLEBALANCE/ LIMIT", new BsonDocument {
                                { "Label", "AVAILABLE BALANCE/ LIMIT" },
                                { "DataType", ReportItemDataType.Number }, }},
                            {"CIFNO", new BsonDocument {
                                { "Label", "CIF NO" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"IDNO", new BsonDocument {
                                { "Label", "ID NO" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"BIRTHDAY", new BsonDocument {
                                { "Label", "BIRTHDAY" },
                                { "DataType", ReportItemDataType.Date }, }},
                            {"SEX", new BsonDocument {
                                { "Label", "SEX" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"ADDRESSCONT", new BsonDocument {
                                { "Label", "ADDRESSCONT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"MOBILEPHONE", new BsonDocument {
                                { "Label", "MOBILEPHONE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"EMAIL", new BsonDocument {
                                { "Label", "EMAIL" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"PHONE", new BsonDocument {
                                { "Label", "PHONE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"SMSSTATUS", new BsonDocument {
                                { "Label", "SMS STATUS" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"SMSALERT", new BsonDocument {
                                { "Label", "SMS ALERT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"INSURANCE", new BsonDocument {
                                { "Label", "INSURANCE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"PAYONLINE", new BsonDocument {
                                { "Label", "PAYONLINE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"AUTOPAYTYPE", new BsonDocument {
                                { "Label", "AUTOPAYTYPE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CUSMANAGE", new BsonDocument {
                                { "Label", "CUSMANAGE" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CUSSEGMENT", new BsonDocument {
                                { "Label", "CUS SEGMENT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"DDA", new BsonDocument {
                                { "Label", "DDA" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"CUSTOMERSEGMENT", new BsonDocument {
                                { "Label", "CUSTOMER SEGMENT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"MACANBOGIOITHIEU", new BsonDocument {
                                { "Label", "MA CAN BO GIOI THIEU" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"DOITUONGKH", new BsonDocument {
                                { "Label", "DOI TUONG KH" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"SOCIFDVCHILUONG", new BsonDocument {
                                { "Label", "SO CIF DV CHILUONG" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"SOHOSOPHT", new BsonDocument {
                                { "Label", "SO HO SO PHT" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"MATHANHVIENLK", new BsonDocument {
                                { "Label", "MA THANH VIEN LK" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"DOCMASKNAME", new BsonDocument {
                                { "Label", "DOCMASKNAME" },
                                { "DataType", ReportItemDataType.String }, }},
                            {"DOCTYPENAME", new BsonDocument {
                                { "Label", "DOCTYPENAME" },
                                { "DataType", ReportItemDataType.String }, }},

                        }));
        }
    }
}