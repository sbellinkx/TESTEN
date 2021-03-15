﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using CsvHelper;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace E_60Toevoegen
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class CSV_E60Toevoegen : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            System.IO.Directory.CreateDirectory(@"c:\\temp\\E_60");
            Result r = Result.Succeeded;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string emptyFirstCellDocument = "";

            // Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            // Get Document
            Document doc = uidoc.Document;

            FilteredElementCollector col = new FilteredElementCollector(doc)
            .OfClass(typeof(ViewSchedule));

            // Export Options on how the .txt file will look. Text "" is gone
            // FieldDelimiter is TAB replaced with ,            
            ViewScheduleExportOptions opt = new ViewScheduleExportOptions()
            {
                TextQualifier = ExportTextQualifier.DoubleQuote,
                FieldDelimiter = ","
            };

            // Formating for writing to xlsx
            var format = new ExcelTextFormat()
            {
                Culture = CultureInfo.InvariantCulture,
                // Escape character for values containing the Delimiter
                // ex: "A,Name",1 --> two cells, not three
                TextQualifier = '"'
                // Other properties
                // EOL, DataTypes, Encoding, SkipLinesBeginning/End
            };

            // Creates new excelpackage this 
            using (ExcelPackage excelEngine = new ExcelPackage())
            {
                ExcelWorksheet wbUitzondering = excelEngine.Workbook.Worksheets.Add("Uitzondering");
                foreach (ViewSchedule vs in col)
                {
                    // Searches for schedules containing AE E60 M52 en M57 ventilatierooster
                    // dit zijn de schedules waarbij het met aantallen is.
                    if (vs.Name.Contains("AE_E60")
                        || vs.Name.Contains("AE_M52")
                        || vs.Name.Contains("AE_M57_ Ventilatieroosters")
                        || vs.Name.Contains("AE_M57_Toestellen VENT")
                        || vs.Name.Contains("AE_M50_Toestellen HVAC coll"))
                    {
                        //create a WorkSheet
                        ExcelWorksheet ws1 = excelEngine.Workbook.Worksheets.Add(vs.Name);
                        // Export c:\\temp --> Will be save as
                        string filename = Environment.UserName + vs.Name;

                        vs.Export(@"c:\\temp\\E_60\", filename + ".csv", opt);

                        // Lege eerste kolom eruithalen. Dan zo 1 grote .csv met alle "lege" in en dan nog altijd per .csv per sheet
                        string normalDocument = "";

                        string StringPathFile = @"c:\\temp\\E_60\" + filename + ".csv";
                        string[] lines = File.ReadAllLines(StringPathFile);
                        char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

                        foreach (string line in lines)
                        {
                            if (line.Split(delimiterChars)[1] == "")
                            {
                                emptyFirstCellDocument += line + Environment.NewLine;
                                System.Diagnostics.Debug.WriteLine(emptyFirstCellDocument + Environment.NewLine);
                            }
                            else
                            {
                                normalDocument += line + Environment.NewLine;
                                System.Diagnostics.Debug.WriteLine(normalDocument + Environment.NewLine);
                            }
                        }
                        File.WriteAllText(@"c:\\temp\\E_60\" + filename + ".csv", normalDocument.ToString());
                        FileInfo file = new FileInfo(@"c:\\temp\\E_60\" + filename + ".csv");
                        // Adds Worksheet as first in the row 
                        ws1.Workbook.Worksheets.MoveToStart(vs.Name);
                        ws1.Cells["A1"].LoadFromText(file, format);

                        // the path of the file
                        string filePath = "C:\\temp\\E_60\\Excel_E_60.xlsx";

                        // Write the file to the disk
                        FileInfo fi = new FileInfo(filePath);
                        excelEngine.SaveAs(fi);
                    }
                }
                File.WriteAllText(@"c:\\temp\\E_60\Uitzonderingen.csv", emptyFirstCellDocument.ToString());
                FileInfo fileUitzondering = new FileInfo(@"c:\\temp\\E_60\Uitzonderingen.csv");
                wbUitzondering.Cells["A1"].LoadFromText(fileUitzondering, format);

                string stringPath = "C:\\temp\\E_60\\Uitzonderingen.xlsx";

                // Write the file to the disk
                FileInfo fileInfoUitzondering = new FileInfo(stringPath);
                excelEngine.SaveAs(fileInfoUitzondering);

                excelEngine.Dispose();
            }
            return r;
        }
    }
}