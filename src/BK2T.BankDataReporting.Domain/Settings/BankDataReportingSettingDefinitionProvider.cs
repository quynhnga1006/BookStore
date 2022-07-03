using Volo.Abp.Settings;

namespace BK2T.BankDataReporting.Settings
{
    public class BankDataReportingSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BankDataReportingSettings.MySetting1));
        }
    }
}
