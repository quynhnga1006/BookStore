using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace BK2T.BankDataReporting
{
    public static class BankDataReportingModuleExtensionConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ConfigureExistingProperties();
                ConfigureExtraProperties();
            });
        }

        private static void ConfigureExistingProperties()
        {
         }

        private static void ConfigureExtraProperties()
        {
            ObjectExtensionManager.Instance.Modules()
                  .ConfigureIdentity(identity =>
                  {
                      identity.ConfigureUser(user =>
                      {
                          user.AddOrUpdateProperty<Guid?>(
                              "DepartmentId",
                              property => {
                                  property.DisplayName = new FixedLocalizableString("Department");
                                  property.UI.Lookup.Url = "/api/app/department";
                                  property.UI.Lookup.DisplayPropertyName = "name";
                              }
                          ); 
                      });
                  });
        }
    }
}
