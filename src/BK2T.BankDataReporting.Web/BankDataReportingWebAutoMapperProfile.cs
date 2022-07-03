using AutoMapper;
using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BK2T.BankDataReporting.Web
{
    public class BankDataReportingWebAutoMapperProfile : Profile
    {
        public BankDataReportingWebAutoMapperProfile()
        {
            CreateMap<Pages.Departments.CreateModalModel.CreateDepartmentViewModel, DepartmentRequestDto>()
                .ForMember(dest => dest.CustomerSegments, opt => opt.MapFrom(src => ExtractJsonDataToArray(src.CustomerSegments)));
            CreateMap<DepartmentDto, Pages.Departments.EditModalModel.UpdateDepartmentViewModel>()
                .ForMember(dest => dest.CustomerSegments, opt => opt.MapFrom(src => src.CustomerSegments.JoinAsString(";")));
            CreateMap<Pages.Departments.EditModalModel.UpdateDepartmentViewModel, DepartmentRequestDto>()
                .ForMember(dest => dest.CustomerSegments, opt => opt.MapFrom(src => ExtractJsonDataToArray(src.CustomerSegments)));
            CreateMap<Pages.ReportFiles.CreateModalModel.CreateReportFileViewModel, ReportFileRequestDto>();
            CreateMap<Pages.TargetPlans.EditModalModel.UpdateTargetPlanViewModel, TargetPlanRequestDto>();
            CreateMap<Pages.ManualCapitals.EditModalModel.UpdateManualCapitalViewModel, ManualCapitalRequestDto>();
        }

        private static IEnumerable<string> ExtractJsonDataToArray(string jsonData)
        {
            var anonymousObject = new { Value = "" };
            var anonymousList = new[] { anonymousObject }.ToList();

            var arr = JsonConvert.DeserializeAnonymousType(jsonData, anonymousList);
            return arr.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Value).ToList();
        }
    }
}
