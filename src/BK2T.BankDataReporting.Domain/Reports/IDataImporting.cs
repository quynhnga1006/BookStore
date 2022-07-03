using System.Data;
using System.Threading.Tasks;

namespace BK2T.BankDataReporting.Reports
{
    public interface IDataImporting
    {
        Task ImportDataFromDataTableAsync(DataImportingArgs args, DataTable dataTable);
    }
}
