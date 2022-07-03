using BK2T.BankDataReporting.MongoDB;
using Xunit;

namespace BK2T.BankDataReporting
{
    [CollectionDefinition(BankDataReportingTestConsts.CollectionDefinitionName)]
    public class BankDataReportingApplicationCollection : BankDataReportingMongoDbCollectionFixtureBase
    {

    }
}
