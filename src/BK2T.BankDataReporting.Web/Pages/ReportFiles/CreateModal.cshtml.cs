using BK2T.BankDataReporting.ReportFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BK2T.BankDataReporting.Web.Pages.ReportFiles
{
    public class CreateModalModel : BankDataReportingPageModel
    {
        private readonly IReportFileAppService _reportFileAppService;

        [BindProperty]
        public CreateReportFileViewModel ReportFile { get; set; }

        public CreateModalModel(IReportFileAppService reportFileAppService)
        {
            _reportFileAppService = reportFileAppService;
        }

        public void OnGet()
        {
            ReportFile = new CreateReportFileViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReportFile.ReportDate = DateTime.SpecifyKind(ReportFile.ReportDate, DateTimeKind.Utc);
            var reportFileRequest = ObjectMapper.Map<CreateReportFileViewModel, ReportFileRequestDto>(ReportFile);
            await _reportFileAppService.CreateAsync(reportFileRequest);
            return NoContent();
        }

        public class CreateReportFileViewModel
        {
            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "ReportFiles:ReportDate")]
            public DateTime ReportDate { get; set; } = DateTime.Now.Date;

            [Required]
            [Display(Name = "ReportFiles:ReportType")]
            public ReportType ReportType { get; set; }

            [Required]
            [Display(Name = "ReportFiles:ReportFile")]
            public IFormFile FileData { get; set; }
        }
    }
}
