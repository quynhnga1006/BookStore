using AutoMapper;
using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using BK2T.BankDataReporting.TargetPlans;
using BK2T.BankDataReporting.Utils;

namespace BK2T.BankDataReporting
{
    public class BankDataReportingApplicationAutoMapperProfile : Profile
    {
        public BankDataReportingApplicationAutoMapperProfile()
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Department, DepartmentRequestDto>().ReverseMap();
            CreateMap<TargetPlan, TargetPlanDto>();

            CreateMap<ReportFileDto, ReportFile>();
            CreateMap<ReportFile, ReportFileDto>()
                .ForMember(dest => dest.ReportStatusCode, opt => opt.MapFrom(src => EnumExtensions.GetDisplayName(src.ReportFileStatus)))
                .ForMember(dest => dest.ReportTypeCode, opt => opt.MapFrom(src => EnumExtensions.GetDisplayName(src.ReportType)))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatorId));
            CreateMap<ReportFile, ReportFileRequestDto>().ReverseMap();

            CreateMap<Report, ReportDto>()
                .ForMember(des => des.Setting, act => act.MapFrom(src => src.Setting.ToDictionary()));
            CreateMap<ReportTemplate, ReportTemplateDto>()
                .ForMember(des => des.Template, act => act.MapFrom(src => src.Template.ToDictionary()));
        }
    }
} 