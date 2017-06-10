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

using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.Helper
{
    public sealed class GridListControlHelper
    {
        public static void InitializeGridListControl<T>(GridListControl gridControl, IList<T> record, IList<GridListControlProperties> oglProperty, bool addRowNumber = true, int rowHeight = 25, int additionalRowCount = 0)
        {
            gridControl.ShowColumnHeader = true;
            gridControl.MultiColumn = true;
            gridControl.Grid.Model.EnableLegacyStyle = false;

            gridControl.ThemesEnabled = true;
            gridControl.FillLastColumn = true;
            gridControl.BorderStyle = BorderStyle.FixedSingle;
            gridControl.Grid.GridVisualStyles = GridVisualStyles.Office2010Silver;
            gridControl.Grid.Model.RowCount = record.Count + additionalRowCount;
            gridControl.BackColor = Color.White;
            gridControl.Grid.Model.RowHeights[0] = rowHeight;

            var colIndex = 1;
            foreach (var item in oglProperty)
            {
                gridControl.Grid.Model.ColWidths[colIndex] = item.Width;

                colIndex++;
            }

            // set header 
            gridControl.Grid.PrepareViewStyleInfo += delegate(object sender, GridPrepareViewStyleInfoEventArgs e)
            {
                if (e.ColIndex > 0 && e.RowIndex == 0)
                {
                    colIndex = 1;
                    foreach (var item in oglProperty)
                    {
                        if (colIndex == e.ColIndex)
                            e.Style.Text = item.Header;

                        colIndex++;
                    }
                }
            };

            // set col count
            gridControl.Grid.QueryColCount += delegate(object sender, Syncfusion.Windows.Forms.Grid.GridRowColCountEventArgs e)
            {
                e.Count = oglProperty.Count;
                e.Handled = true;
            };

            if (addRowNumber)
            {
                gridControl.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
                {
                    // Make sure the cell falls inside the grid
                    if (e.RowIndex > 0)
                    {
                        if (e.RowIndex % 2 == 0)
                            e.Style.BackColor = ColorCollection.BACK_COLOR_ALTERNATE;
                        else
                            e.Style.BackColor = Color.White;

                        colIndex = 1;
                        foreach (var item in oglProperty)
                        {
                            if (colIndex == e.ColIndex)
                            {
                                if (colIndex == 1) // no urut
                                {
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = e.RowIndex;
                                }
                                else
                                {
                                    e.Style.HorizontalAlignment = item.HorizontalAlignment;
                                }
                            }

                            colIndex++;
                        }

                        // we handled it, let the grid know
                        e.Handled = true;
                    }
                };
            }
            else
            {
                gridControl.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
                {
                    // Make sure the cell falls inside the grid
                    if (e.RowIndex > 0)
                    {
                        if (e.RowIndex % 2 == 0)
                            e.Style.BackColor = ColorCollection.BACK_COLOR_ALTERNATE;
                        else
                            e.Style.BackColor = Color.White;

                        // we handled it, let the grid know
                        e.Handled = true;
                    }
                };
            }
        }

        public static void InitializeGridListControl<T>(GridControl gridControl, IList<T> record, IList<GridListControlProperties> oglProperty, int rowHeight = 25)
        {
            gridControl.AllowDragSelectedRows = false;
            gridControl.AllowDragSelectedCols = false;

            gridControl.Cols.Hidden[0] = true;

            gridControl.RowCount = record.Count;
            gridControl.ColCount = oglProperty.Count;
            gridControl.EnterKeyBehavior = GridDirectionType.None;
            gridControl.ForeColor = Color.Black;

            gridControl.ThemesEnabled = true;
            gridControl.BorderStyle = BorderStyle.FixedSingle;
            gridControl.ActivateCurrentCellBehavior = GridCellActivateAction.SelectAll;

            gridControl.Model.RowHeights[0] = rowHeight;
            // set header
            var colIndex = 1;
            foreach (var item in oglProperty)
            {
                gridControl.ColWidths[colIndex] = item.Width;

                gridControl[0, colIndex].Text = item.Header;

                #region column style

                var colStyle = gridControl.Model.ColStyles[colIndex];

                colStyle.HorizontalAlignment = item.HorizontalAlignment;
                colStyle.Enabled = item.IsEditable;

                if (colIndex == 1) // nourut
                {
                    colStyle.HorizontalAlignment = GridHorizontalAlignment.Center;
                    colStyle.Enabled = false;
                }

                if (!item.IsEditable)
                    colStyle.BackColor = ColorCollection.DEFAULT_FORM_COLOR;

                #endregion cell style

                colIndex++;
            }

            GridControlColSizeHelper colSizeHelper = new GridControlColSizeHelper();
            colSizeHelper.WireGrid(gridControl);

            GridControlColSizeHelper.GridColSizeBehavior behavior = GridControlColSizeHelper.GridColSizeBehavior.FillRightColumn;
            colSizeHelper.ColSizeBehavior = behavior;
        }

        public static void AddObject<T>(GridListControl gridControl, IList<T> record, T obj, bool isLastRowFocus = true, int additionalRowCount = 0)
        {
            record.Add(obj);
            gridControl.Grid.Model.RowCount = record.Count + additionalRowCount;
            gridControl.Refresh();

            if (isLastRowFocus)
                gridControl.SetSelected(gridControl.Grid.RowCount - 1, true);
        }

        public static void AddObjects<T>(GridListControl gridControl, IList<T> record, int additionalRowCount = 0)
        {
            gridControl.Grid.Model.RowCount = record.Count + additionalRowCount;
            gridControl.Refresh();

            if (record.Count > 0)
                gridControl.SetSelected(additionalRowCount, true);
        }

        public static void UpdateObject<T>(GridListControl gridControl, IList<T> record, T obj, int additionalRowCount = 0)
        {
            record[gridControl.SelectedIndex - additionalRowCount] = obj;
            gridControl.Refresh();
        }

        public static void RemoveObject<T>(GridListControl gridControl, IList<T> record, T obj, int additionalRowCount = 0)
        {
            record.Remove(obj);
            gridControl.Grid.Model.RowCount = record.Count + additionalRowCount;

            gridControl.Refresh();

            if (record.Count > 0 && gridControl.SelectedIndex != 0)
                gridControl.SetSelected(gridControl.SelectedIndex - 1, true);
        }

        public static void Refresh<T>(GridListControl gridControl, IList<T> record, int additionalRowCount = 0)
        {
            gridControl.Grid.Model.RowCount = record.Count + additionalRowCount;
            gridControl.Refresh();

            if (record.Count > 0)
                gridControl.SetSelected(additionalRowCount, true);

        }

        public static void Refresh<T>(GridControl gridControl, IList<T> record)
        {
            gridControl.RowCount = record.Count;
            gridControl.Refresh();
        }

        public static void SelectCellText(GridControl grid, int row, int col)
        {
            // ref: http://www.syncfusion.com/forums/67771/select-text-programmatically-in-gridtextboxcellrenderer
            GridTextBoxCellRenderer cr = grid.CurrentCell.Renderer as GridTextBoxCellRenderer;
            TextBoxBase tb = cr.TextBox;

            if (tb != null && tb.TextLength > 0)
            {
                tb.SelectionStart = 0;
                tb.SelectionLength = tb.TextLength;
            }
        }

        public static void SetCellValue(object cellValue, GridQueryCellInfoEventArgs e, GridHorizontalAlignment horizontalAlignment = GridHorizontalAlignment.Left)
        {
            e.Style.HorizontalAlignment = horizontalAlignment;
            e.Style.CellValue = cellValue;
        }

        public static void SetCurrentCell(GridControl grid, int row, int col)
        {
            grid.CurrentCell.MoveTo(row, col, GridSetCurrentCellOptions.BeginEndUpdate);
            grid.ScrollCellInView(row, col);
        }
    }
}
