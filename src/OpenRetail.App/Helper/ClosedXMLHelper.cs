/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenRetail.App.Helper
{
    public static class ClosedXMLHelper
    {
        public static void SetValue(IXLWorksheet ws, int row, int column, object value)
        {
            ws.Cell(row, column).Value = value;
        }

        public static void SetValue(IXLWorksheet ws, int row, int column, object value, XLCellValues valueDataType)
        {
            ws.Cell(row, column).SetValue(value).SetDataType(valueDataType);
        }

        public static void SetFormatCell(IXLWorksheet ws,
                                         int firstCellRow, int firstCellColumn, int lastCellRow, int lastCellColumn,
                                         bool isFontBold = false, int fontSize = 10, bool isMergeCell = false,
                                         bool isSetColorHeader = false, bool isSetBorder = true,
                                         XLAlignmentHorizontalValues HorizontalAlgn = XLAlignmentHorizontalValues.Center,
                                         XLAlignmentVerticalValues VerticalAlgn = XLAlignmentVerticalValues.Center)
        {
            // Defining ranges
            // From worksheet
            var rangeTable = ws.Range(firstCellRow, firstCellColumn, lastCellRow, lastCellColumn);
            rangeTable.Style.Alignment.Horizontal = HorizontalAlgn;
            rangeTable.Style.Alignment.Vertical = VerticalAlgn;
            rangeTable.Style.Font.Bold = isFontBold;
            rangeTable.Style.Font.FontSize = fontSize;

            if (isSetColorHeader)
                rangeTable.Style.Fill.BackgroundColor = XLColor.FromArgb(184, 184, 184);

            if (isMergeCell)
            {
                // Merge title cells
                //rngTable.Row(1).Merge(); // We could've also used: rngTable.Range("A1:E1").Merge()
                rangeTable.Merge();
            }

            if (isSetBorder)
            {
                rangeTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                rangeTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
        }

        public static void SetColumnWidth(IXLWorksheet ws, int column, double width)
        {
            ws.Column(column).Width = width;
        }

        public static void SetColumnWidth(IXLWorksheet ws, int firstColumn, int lastColumn, double width)
        {
            ws.Columns(firstColumn, lastColumn).Width = width;
        }

        public static void Save(XLWorkbook wb, IXLWorksheet ws, string output, bool isOpenExcelFile = true)
        {
            // Adjust column widths to their content
            //ws.Columns().AdjustToContents();

            // Saving the workbook
            wb.SaveAs(output);

            if (isOpenExcelFile)
            {
                var fi = new FileInfo(output);
                if (fi.Exists)
                    Process.Start(output);
            }
        }
    }
}
