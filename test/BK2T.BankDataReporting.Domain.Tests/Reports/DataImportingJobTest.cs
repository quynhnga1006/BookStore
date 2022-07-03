using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Xunit;

namespace BK2T.BankDataReporting.Reports
{
    [Collection(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class DataImportingJobTest : BankDataReportingDomainTestBase
    {
        private IRepository<ReportTemplate, Guid> _mockReportTemplateRepository;
        private IRepository<ReportItem, Guid> _mockReportItemRepository;
        private IRepository<ReportFile, Guid> _mockReportFileRepository;
        private IRepository<Department, Guid> _mockDepartmentRepository;
        private IBlobContainer _mockBlobContainer;
        private IIdentityUserRepository _mockIdentityUserRepository;
        private readonly DepartmentManager _departmentManager;
        private DataImportingJobTestInstance _dataImportingJob;
        private IServiceProvider _mockServiceProvider;

        public DataImportingJobTest()
        {
            _departmentManager = ServiceProvider.GetRequiredService<DepartmentManager>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _mockReportTemplateRepository = Substitute.For<IRepository<ReportTemplate, Guid>>();
            _mockReportItemRepository = Substitute.For<IRepository<ReportItem, Guid>>();
            _mockReportFileRepository = Substitute.For<IRepository<ReportFile, Guid>>();
            _mockDepartmentRepository = Substitute.For<IRepository<Department, Guid>>();
            _mockBlobContainer = Substitute.For<IBlobContainer>();
            _mockServiceProvider = Substitute.For<IServiceProvider>();
            _mockIdentityUserRepository = Substitute.For<IIdentityUserRepository>();
            _dataImportingJob = new DataImportingJobTestInstance(
                _mockReportTemplateRepository,
                _mockReportItemRepository,
                _mockReportFileRepository,
                _mockDepartmentRepository,
                _mockIdentityUserRepository,
                _mockBlobContainer,
                _mockServiceProvider);
        }

        [Theory]
        [InlineData(ReportType.Loan, "48030", "", "048030000")]
        [InlineData(ReportType.Loan, "48098", "06", "048005000")]
        [InlineData(ReportType.Loan, "48098", "88", "048008000")]
        [InlineData(ReportType.Loan, "48098", "09", "048028000")]
        [InlineData(ReportType.Loan, "48098", "", "048003000")]
        [InlineData(ReportType.Deposit, "48030", "", "048030000")]
        [InlineData(ReportType.Deposit, "48098", "06", "048005000")]
        [InlineData(ReportType.Deposit, "48098", "88", "048008000")]
        [InlineData(ReportType.Deposit, "48098", "09", "048028000")]
        [InlineData(ReportType.Deposit, "48098", "", "048009000")]
        public async Task ApplyDepartmentImportRule_ShouldChangeDataItemCodeAsync(ReportType reportType, string oldCode, string customerSegment, string expectedCode)
        {
            // Arrange
            var segment1 = new List<string> { "06", "07", "08" };
            var segment2 = new List<string> { "00", "01", "02", "03", "05", "88", "89", "97", "99" };
            var segment3 = new List<string> { "04", "09" };
            var departments = new List<Department>
            {
                await _departmentManager.CreateAsync("048030000", "PHONG DICH VU KHACH HANG 1", "48030", new List<string>()),
                await _departmentManager.CreateAsync("048005000", "PHONG DICH VU KHACH HANG 2", "48098", segment1),
                await _departmentManager.CreateAsync("048008000", "PHONG DICH VU KHACH HANG 3", "48098", segment2),
                await _departmentManager.CreateAsync("048028000", "PHONG DICH VU KHACH HANG 4", "48098", segment3),
            };
            var dataItem = new Dictionary<string, object>
            {
                { "MaPGD", oldCode },
                { "MaPhanKhuc", customerSegment },
                { "MaPhongBanGoc", "" }
            };

            // Act
            _dataImportingJob.ApplyDepartmentImportRuleTest(dataItem, departments, reportType);

            // Assert
            dataItem.GetValueOrDefault("MaPhongBanGoc").ShouldBe(expectedCode);
        }
    }

    public class DataImportingJobTestInstance : DataImportingJob
    {
        public DataImportingJobTestInstance(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<ReportItem, Guid> reportItemRepository,
            IRepository<ReportFile, Guid> reportFileRepository,
            IRepository<Department, Guid> departmentItemRepository,
            IIdentityUserRepository identityUserRepository,
            IBlobContainer blobContainer,
            IServiceProvider serviceProvider)
            : base(reportTemplateRepository, reportItemRepository, reportFileRepository, departmentItemRepository, identityUserRepository, blobContainer, serviceProvider)
        {
        }
        public void ApplyDepartmentImportRuleTest(Dictionary<string, object> dataItem, List<Department> allDepartments, ReportType reportType)
        {
            base.ApplyDepartmentImportRule(dataItem, allDepartments, reportType);
        }
    }
}
