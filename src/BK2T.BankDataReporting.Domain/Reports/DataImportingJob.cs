using BK2T.BankDataReporting.Departments;
using BK2T.BankDataReporting.ReportFiles;
using BK2T.BankDataReporting.ReportTemplates;
using BK2T.BankDataReporting.Utils;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace BK2T.BankDataReporting.Reports
{
    public class DataImportingJob : AsyncBackgroundJob<DataImportingArgs>, ITransientDependency
    {
        private readonly IRepository<ReportTemplate, Guid> _reportTemplateRepository;
        private readonly IRepository<ReportItem, Guid> _reportItemRepository;
        private readonly IRepository<ReportFile, Guid> _reportFileRepository;
        private readonly IRepository<Department, Guid> _departmentItemRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IBlobContainer _blobContainer;
        private readonly IServiceProvider _serviceProvider;

        public DataImportingJob(
            IRepository<ReportTemplate, Guid> reportTemplateRepository,
            IRepository<ReportItem, Guid> reportItemRepository,
            IRepository<ReportFile, Guid> reportFileRepository,
            IRepository<Department, Guid> departmentItemRepository,
            IIdentityUserRepository identityUserRepository,
            IBlobContainer blobContainer,
            IServiceProvider serviceProvider
            )
        {
            _reportTemplateRepository = reportTemplateRepository;
            _reportItemRepository = reportItemRepository;
            _reportFileRepository = reportFileRepository;
            _departmentItemRepository = departmentItemRepository;
            _identityUserRepository = identityUserRepository;
            _blobContainer = blobContainer;
            _serviceProvider = serviceProvider;
        }

        public override async Task ExecuteAsync(DataImportingArgs args)
        {
            var dataFile = await _blobContainer.GetAllBytesOrNullAsync(args.ReportFileName);
            if (dataFile == null) return;
            var reportFile = await _reportFileRepository.GetAsync(args.ReportFileId);
            if (reportFile == null) return;
            if (args.ReportType.Equals(ReportType.Loan) || args.ReportType.Equals(ReportType.Deposit))
            {
                await GetReportItemsFromDataTable(args, dataFile);
                reportFile.ReportFileStatus = ReportFileStatus.Imported;
                await _reportFileRepository.UpdateAsync(reportFile);
                return;
            }
            var dataTable = ExcelFileUtils.ConvertExcelFileIntoDataTable(dataFile);
            if (dataTable.Rows.Count == 0) return;
            IDataImporting dataImporting;
            switch (args.ReportType)
            {
                case ReportType.Deposit:
                case ReportType.Loan:
                    //var reportItems = await GetReportItemsFromDataTable(args, dataFile);
                    //await _reportItemRepository.InsertManyAsync(reportItems);
                    break;
                case ReportType.NII:
                    dataImporting = GetDataImportingService(ReportType.NII);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.DebtDueCustomer:
                    dataImporting = GetDataImportingService(ReportType.DebtDueCustomer);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.Collateral:
                    dataImporting = GetDataImportingService(ReportType.Collateral);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.Provision:
                    dataImporting = GetDataImportingService(ReportType.Provision);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.EFastCustomerItem:
                    dataImporting = GetDataImportingService(ReportType.EFastCustomerItem);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.CustomerSalary:
                    dataImporting = GetDataImportingService(ReportType.CustomerSalary);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.LifeInsurance:
                    dataImporting = GetDataImportingService(ReportType.LifeInsurance);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.NonLifeInsurance:
                    dataImporting = GetDataImportingService(ReportType.NonLifeInsurance);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.IpayCustomer:
                    dataImporting = GetDataImportingService(ReportType.IpayCustomer);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.PersonalCustomerProduct:
                    dataImporting = GetDataImportingService(ReportType.PersonalCustomerProduct);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.ForeignCurrencyTradingProfit:
                    dataImporting = GetDataImportingService(ReportType.ForeignCurrencyTradingProfit);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.CorporateCustomer:
                    dataImporting = GetDataImportingService(ReportType.CorporateCustomer);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.CreditCard:
                    dataImporting = GetDataImportingService(ReportType.CreditCard);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.CardAcceptingUnit:
                    dataImporting = GetDataImportingService(ReportType.CardAcceptingUnit);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
                case ReportType.RetailDevelopmentCustomer:
                    dataImporting = GetDataImportingService(ReportType.RetailDevelopmentCustomer);
                    await dataImporting.ImportDataFromDataTableAsync(args, dataTable);
                    break;
            }
            reportFile.ReportFileStatus = ReportFileStatus.Imported;
            await _reportFileRepository.UpdateAsync(reportFile);
        }
        public IDataImporting GetDataImportingService(ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.Collateral:
                    return (IDataImporting)_serviceProvider.GetService(typeof(CollateralDataImporting));
                case ReportType.Deposit:
                case ReportType.Loan:
                case ReportType.NII:
                    return (IDataImporting)_serviceProvider.GetService(typeof(NIIDataImporting));
                case ReportType.DebtDueCustomer:
                    return (IDataImporting)_serviceProvider.GetService(typeof(DebtDueCustomerDataImporting));
                case ReportType.Provision:
                    return (IDataImporting)_serviceProvider.GetService(typeof(ProvisionDataImporting));
                case ReportType.EFastCustomerItem:
                    return (IDataImporting)_serviceProvider.GetService(typeof(EFastCustomerDataImporting));
                case ReportType.CustomerSalary:
                    return (IDataImporting)_serviceProvider.GetService(typeof(CustomerSalaryDataImporting));
                case ReportType.LifeInsurance:
                case ReportType.NonLifeInsurance:
                    return (IDataImporting)_serviceProvider.GetService(typeof(InsuranceDataImporting));
                case ReportType.IpayCustomer:
                    return (IDataImporting)_serviceProvider.GetService(typeof(IpayCustomerDataImporting));
                case ReportType.PersonalCustomerProduct:
                    return (IDataImporting)_serviceProvider.GetService(typeof(PersonalCustomerProductDataImporting));
                case ReportType.ForeignCurrencyTradingProfit:
                    return (IDataImporting)_serviceProvider.GetService(typeof(ForeignCurrencyTradingProfitDataImporting));
                case ReportType.CorporateCustomer:
                    return (IDataImporting)_serviceProvider.GetService(typeof(CorporateCustomerDataImporting));
                case ReportType.CreditCard:
                    return (IDataImporting)_serviceProvider.GetService(typeof(CreditCardDataImporting));
                case ReportType.CardAcceptingUnit:
                    return (IDataImporting)_serviceProvider.GetService(typeof(CardAcceptingUnitDataImporting));
                case ReportType.RetailDevelopmentCustomer:
                    return (IDataImporting)_serviceProvider.GetService(typeof(RetailDevelopmentCustomerDataImporting));
                default:
                    return null;
            }
        }

        // handling if type is loan or deposit 
        public async Task GetReportItemsFromDataTable(DataImportingArgs args, byte[] fileBytes)
        {
            var template = await _reportTemplateRepository.FirstOrDefaultAsync(rp => rp.ReportType.Equals(args.ReportType));
            var dataTypeDict = new Dictionary<string, int>();
            if (template != null)
            {
                dataTypeDict = template.Template.ToDictionary(t => t.Name, t => t.Value.AsBsonDocument.GetValue("DataType").AsInt32);
            }
            var departments = await _departmentItemRepository.GetListAsync();
            var users = await _identityUserRepository.GetListAsync();
            var userDicts = users.ToDictionary(u => u.UserName, u => u.Id);
            

            using var stream = new MemoryStream(fileBytes);
            var spreadSheetDocument = SpreadsheetDocument.Open(stream, false);
            var sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            string relationshipId = sheets.First().Id.Value;
            var worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            var rows = sheetData.Descendants<Row>();

            var keys = new List<string>();
            var dataRow = new List<string>();
            foreach (var cell in rows.ElementAt(0))
            {
                keys.Add(ExcelFileUtils.GetCellValue(spreadSheetDocument, cell as Cell));
                dataRow.Add("");
            }

            int batchSize = 500;
            int total = ((rows.Count() - 1) / batchSize) + (((rows.Count() - 1) % batchSize) > 0 ? 1 : 0);
            var count = 1;
            while (count <= total)
            {
                var reportItems = new List<ReportItem>();
                int startIndex = (count - 1) * batchSize;
                if (count == 1)
                {
                    startIndex = 1;
                }
                var batchRows = rows
                    .Skip(startIndex)
                    .Take(batchSize);
                foreach (var row in batchRows)
                {
                    var childRow = new Dictionary<string, object>();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        Cell cell = row.Descendants<Cell>().ElementAt(i);
                        int actualCellIndex = ExcelFileUtils.CellReferenceToIndex(cell);
                        if (actualCellIndex >= keys.Count - 1) continue;
                        dataRow[actualCellIndex] = (ExcelFileUtils.GetCellValue(spreadSheetDocument, cell));
                    }
                    for (int i = 0; i < keys.Count; ++i)
                    {
                        var dataType = (ReportItemDataType)dataTypeDict.GetValueOrDefault(keys[i]);
                        var convertedValue = ConvertValueToStrongType(dataType, dataRow[i]);
                        childRow.Add(keys[i], convertedValue);
                        dataRow[i] = "";
                    }
                    AddAdditionalFields(childRow, args.ReportType);
                    var (username, departmentCode) = ApplyDepartmentImportRule(childRow, departments, args.ReportType);
                    var departmentId = departments.FirstOrDefault(d => d.Code.Equals(departmentCode))?.Id;
                    childRow.TryGetValue("SoCIF", out var cif);
                    childRow.TryGetValue("SoTaiKhoan", out var accountNumber);
                    reportItems.Add(new ReportItem
                    {
                        UserId = userDicts.GetValueOrDefault(username),
                        DepartmentId = departmentId ?? Guid.Empty,
                        ReportFileId = args.ReportFileId,
                        ReportType = (int)args.ReportType,
                        DateOfData = args.DateOfData,
                        ReportData = new MongoDB.Bson.BsonDocument(childRow),
                        CifNumber = (string)cif,
                        AccountNumber = (string)accountNumber
                    });
                }
                await _reportItemRepository.InsertManyAsync(reportItems);
                count++;
            }
        }

        public static object ConvertValueToStrongType(ReportItemDataType dataType, object value)
        {
            switch (dataType)
            {
                case ReportItemDataType.String:
                    return value;
                case ReportItemDataType.Number:
                    if (double.TryParse(value.ToString(), out double parsedValue))
                        return parsedValue;
                    return null;
                case ReportItemDataType.Date:
                    if (double.TryParse(value.ToString(), out double oaDate))
                        return DateTime.SpecifyKind(DateTime.FromOADate(oaDate), DateTimeKind.Utc);
                    if (DateTime.TryParse(value.ToString(), out DateTime parsedDate))
                        return DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                    return null;
                case ReportItemDataType.Boolean:
                    if (Int16.TryParse(value.ToString(), out short parsedInt))
                        return Convert.ToBoolean(parsedInt);
                    return null;
                default: return value;
            }
        }
        // TODO: Ugly hack hard code the header for the rule. Need to do refactor!
        public (string, string) ApplyDepartmentImportRule(Dictionary<string, object> dataItem, List<Department> allDepartments, ReportType reportType)
        {
            var username = dataItem.GetValueOrDefault("MaCanBoGoc");
            if (username == null) username = "";
            dataItem["MaCanBoGoc"] = username.ToString().ToLower();
            var oldCode = dataItem.GetValueOrDefault("MaPGD").ToString();
            var departments = allDepartments.Where(x => x.OldCode.Equals(oldCode));
            if (string.IsNullOrEmpty(oldCode) || !departments.Any()) return (dataItem["MaCanBoGoc"].ToString(), dataItem["MaPhongBanGoc"].ToString());

            if (oldCode == "48098")
            {
                var customerSegment = dataItem.GetValueOrDefault("MaPhanKhuc").ToString();
                dataItem["MaPhongBanGoc"] = BuildDepartmentCode(departments, customerSegment, reportType);
                return (dataItem["MaCanBoGoc"].ToString(), dataItem["MaPhongBanGoc"].ToString());
            }
            var department = departments.FirstOrDefault();
            if (dataItem["MaPhongBanGoc"].ToString().Equals(department.Code)) return (dataItem["MaCanBoGoc"].ToString(), dataItem["MaPhongBanGoc"].ToString());
            dataItem["MaPhongBanGoc"] = department.Code;
            return (dataItem["MaCanBoGoc"].ToString(), dataItem["MaPhongBanGoc"].ToString());
        }

        // TODO: Ugly hack hard code the header for the rule. Need to do refactor!
        protected string BuildDepartmentCode(IEnumerable<Department> departments, string customerSegment, ReportType reportType)
        {
            var department = departments.FirstOrDefault(d => d.CustomerSegments.Contains(customerSegment));
            if (department != null) return department.Code;
            return reportType == ReportType.Loan ? "048003000" : "048009000";
        }
        private static void AddAdditionalFields(Dictionary<string, object> dataItem, ReportType reportType)
        {
            if (reportType == ReportType.Deposit)
            {
                dataItem.TryGetValue("SoDuTienGuiNgayQuyDoi", out var depositBalanceToday);
                dataItem.TryGetValue("SoDuQuyDoiNgayHomTruoc", out var depositBalanceYesterday);
                dataItem.TryGetValue("SoDuQuyDoiThangTruoc", out var depositBalanceLastMonth);
                dataItem.TryGetValue("SoDuQuyDoiNamTruoc", out var depositBalanceLastYear);

                dataItem.TryGetValue("SoDuTienGuiBQThangQuyDoi", out var averageDepositBalanceThisMonth);
                dataItem.TryGetValue("SoDuTienGuiBQNamQuyDoi", out var averageDepositBalanceThisYear);
                dataItem.TryGetValue("SoDuTienGuiBQThangTruocQuyDoi", out var averageDepositBalanceLastMonth);
                dataItem.TryGetValue("SoDuTienGuiBQNamTruocQuyDoi", out var averageDepositBalanceLastYear);

                dataItem.TryAdd(
                    "TangGiamSoDuTienGuiSoVoiNgayHomTruoc",
                    (double)depositBalanceToday - (double)depositBalanceYesterday);
                dataItem.TryAdd(
                    "TangGiamSoDuTienGuiSoVoiThangTruoc",
                    (double)depositBalanceToday - (double)depositBalanceLastMonth);
                dataItem.TryAdd(
                    "TangGiamSoDuTienGuiSoVoiNamTruoc",
                    (double)depositBalanceToday - (double)depositBalanceLastYear);
                dataItem.TryAdd(
                    "TangGiamSoDuTienGuiBQSoVoiThangTruoc",
                    (double)averageDepositBalanceThisMonth - (double)averageDepositBalanceLastMonth);
                dataItem.TryAdd(
                    "TangGiamSoDuTienGuiBQSoVoiNamTruoc",
                    (double)averageDepositBalanceThisYear - (double)averageDepositBalanceLastYear);
                return;
            }
            dataItem.TryGetValue("DuNoNgayQuyDoi", out var debtToday);
            dataItem.TryGetValue("DuNoHomTruocQuyDoi", out var debtYesterday);
            dataItem.TryGetValue("DuNoCuoiThangTruocQuyDoi", out var debtLastMonth);
            dataItem.TryGetValue("DuNoCuoiNamTruocQuyDoi", out var debtLastYear);

            dataItem.TryGetValue("DuNoBQThangQuyDoi", out var averageDebtThisMonth);
            dataItem.TryGetValue("DuNoBQNamQuyDoi", out var averageDebtThisYear);
            dataItem.TryGetValue("DuNoBQThangTruocQuyDoi", out var averageDebtLastMonth);
            dataItem.TryGetValue("DuNoBQNamTruocQuyDoi", out var averageDebtLastYear);

            dataItem.TryAdd(
                "TangGiamDuNoSoVoiNgay",
                (double)debtToday - (double)debtYesterday);
            dataItem.TryAdd(
                "TangGiamDuNoSoVoiThang",
                (double)debtToday - (double)debtLastMonth);
            dataItem.TryAdd(
                "TangGiamDuNoSoVoiNam",
                (double)debtToday - (double)debtLastYear);
            dataItem.TryAdd(
                "TangGiamDuNoBQSoVoiThang",
                (double)averageDebtThisMonth - (double)averageDebtLastMonth);
            dataItem.TryAdd(
                "TangGiamDuNoBQSoVoiNam",
                (double)averageDebtThisYear - (double)averageDebtLastYear);
        }
    }
}