using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace BK2T.BankDataReporting
{
    public class BankDataReportingWebTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<BankDataReportingWebTestModule>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}