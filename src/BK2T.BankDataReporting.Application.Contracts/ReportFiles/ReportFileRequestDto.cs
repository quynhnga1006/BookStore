using System;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Http;
using BK2T.BankDataReporting.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using BK2T.BankDataReporting.Localization;
using Microsoft.Extensions.Localization;

namespace BK2T.BankDataReporting.ReportFiles
{
    public class ReportFileRequestDto : FullAuditedEntityDto<Guid>, IValidatableObject
    {
        public DateTime ReportDate { get; set; }
        public ReportType ReportType { get; set; }

        [AllowedExtensions(new string[] { ".xlsx"})]
        public IFormFile FileData { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var localier = validationContext.GetRequiredService<IStringLocalizer<BankDataReportingResource>>();          
            if (ReportDate.Date > DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    localier["ReportFiles:DateUploadReportFile"]
                );
            }
        }
    }
}
