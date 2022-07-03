using BK2T.BankDataReporting.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace BK2T.BankDataReporting.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var localizer = validationContext.GetRequiredService<IStringLocalizer<BankDataReportingResource>>();
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    var allowedExtensions = string.Join(", ", _extensions);
                    var error = localizer.GetString(BankDataReportingDomainErrorCodes.AllowedFileExtension, allowedExtensions);
                    return new ValidationResult(error);
                }
            }

            return ValidationResult.Success;
        }
    }
}
