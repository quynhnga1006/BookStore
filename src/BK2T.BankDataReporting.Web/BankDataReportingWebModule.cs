using Abp.AspNetCore.Mvc.UI.Theme.AdminLTE;
using Abp.AspNetCore.Mvc.UI.Theme.AdminLTE.Bundling;
using BK2T.BankDataReporting.Localization;
using BK2T.BankDataReporting.MongoDB;
using BK2T.BankDataReporting.Permissions;
using BK2T.BankDataReporting.Web.Menus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace BK2T.BankDataReporting.Web
{
    [DependsOn(
        typeof(BankDataReportingHttpApiModule),
        typeof(BankDataReportingApplicationModule),
        typeof(BankDataReportingMongoDbModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpSettingManagementWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreMvcUiAdminLTEThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
        )]
    public class BankDataReportingWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(BankDataReportingResource),
                    typeof(BankDataReportingDomainModule).Assembly,
                    typeof(BankDataReportingDomainSharedModule).Assembly,
                    typeof(BankDataReportingApplicationModule).Assembly,
                    typeof(BankDataReportingApplicationContractsModule).Assembly,
                    typeof(BankDataReportingWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureSameSiteCookiePolicy(context);
            ConfigureUrls(configuration);
            ConfigureBundles();
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices(context.Services);
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);
            ConfigureAuthorizePages();
        }

        private void ConfigureSameSiteCookiePolicy(ServiceConfigurationContext context)
        {
            // This to be fix the Chrome login issue for the IdentityServer4
            // https://community.abp.io/posts/patch-for-chrome-login-issue-identityserver4-samesite-cookie-problem-weypwp3n
            context.Services.AddSameSiteCookiePolicy();
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    AdminLTEThemeBundles.Styles.Global,
                    bundle =>
                    {
                        bundle.AddFiles("/global-styles.css");
                    }
                );
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "BankDataReporting";
                });
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BankDataReportingWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<BankDataReportingDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}BK2T.BankDataReporting.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BankDataReportingDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}BK2T.BankDataReporting.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BankDataReportingApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}BK2T.BankDataReporting.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BankDataReportingApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}BK2T.BankDataReporting.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BankDataReportingWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("vi", "vi", "Tiếng Việt"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });
        }

        private void ConfigureNavigationServices(IServiceCollection service)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new BankDataReportingMenuContributor(service));
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(BankDataReportingApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BankDataReporting API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        private void ConfigureAuthorizePages()
        {
            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Departments/Index", BankDataReportingPermissions.Departments.Default);
                options.Conventions.AuthorizePage("/Departments/CreateModal", BankDataReportingPermissions.Departments.Create);
                options.Conventions.AuthorizePage("/Departments/EditModal", BankDataReportingPermissions.Departments.Update);
                options.Conventions.AuthorizePage("/ReportFiles/Index", BankDataReportingPermissions.ReportFiles.Default);
                options.Conventions.AuthorizePage("/ReportFiles/CreateModal", BankDataReportingPermissions.ReportFiles.Create);
                options.Conventions.AuthorizePage("/Reports/Index", BankDataReportingPermissions.Reports.Default);
                options.Conventions.AuthorizePage("/ReportTemplates/Index", BankDataReportingPermissions.ReportTemplates.Default);
                options.Conventions.AuthorizePage("/ReportTemplates/Reports/CreateModal", BankDataReportingPermissions.ReportTemplates.Default);
                options.Conventions.AuthorizePage("/ReportTemplates/Reports/EditModal", BankDataReportingPermissions.ReportTemplates.Update);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCookiePolicy();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            app.UseUnitOfWork();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BankDataReporting API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
