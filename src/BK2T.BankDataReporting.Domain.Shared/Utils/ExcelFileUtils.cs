using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.IO;
using System.Linq;

namespace BK2T.BankDataReporting.Utils
{
    public static class ExcelFileUtils
    {
        public static DataTable ConvertExcelFileIntoDataTable(byte[] fileBytes)
        {
            var dataTable = new DataTable();

            using (var stream = new MemoryStream(fileBytes))
            {
                var spreadSheetDocument = SpreadsheetDocument.Open(stream, false);
                var sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                var worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                var workSheet = worksheetPart.Worksheet;
                var sheetData = workSheet.GetFirstChild<SheetData>();
                var rows = sheetData.Descendants<Row>();

                foreach (var cell in rows.ElementAt(0))
                {
                    dataTable.Columns.Add(GetCellValue(spreadSheetDocument, cell as Cell));
                }

                foreach (var row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        Cell cell = row.Descendants<Cell>().ElementAt(i);
                        int actualCellIndex = CellReferenceToIndex(cell);
                        if (actualCellIndex >= dataTable.Columns.Count) continue;
                        dataRow[actualCellIndex] = GetCellValue(spreadSheetDocument, cell);
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
            dataTable.Rows.RemoveAt(0);
            return dataTable;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            string value = cell.CellValue?.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return document.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText.Trim();
            }
            return cell.InnerText.Trim();
        }


        public static int CellReferenceToIndex(Cell cell)
        {
            string reference = cell.CellReference.ToString().ToUpper();
            int ci = 0;
            reference = reference.ToUpper();
            for (int ix = 0; ix < reference.Length && reference[ix] >= 'A'; ix++)
                ci = (ci * 26) + ((int)reference[ix] - 64);
            return ci - 1;
        }
    }
}