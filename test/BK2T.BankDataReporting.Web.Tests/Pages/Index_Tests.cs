using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace BK2T.BankDataReporting.Pages
{
    [Collection(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class Index_Tests : BankDataReportingWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
