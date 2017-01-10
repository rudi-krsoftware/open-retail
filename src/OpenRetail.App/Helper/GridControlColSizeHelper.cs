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

using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.App.Helper
{
    /// <summary>
    /// Wraps code required for auto-adjusting colwidths into one class
    /// </summary>
    public sealed class GridControlColSizeHelper
    {
        private GridControlBase grid = null;
        private GridColSizeBehavior _colSizeBehavior;
        private double[] colRatios = null;

        public GridControlColSizeHelper()
        {

        }

        public void WireGrid(GridControlBase grid)
        {
            if (this.grid != grid)
            {
                if (this.grid != null)
                    UnwireGrid();

                this.grid = grid;
                if (grid is GridDataBoundGrid)
                {
                    ((GridDataBoundGrid)this.grid).SmoothControlResize = false;
                }
                else if (grid is GridControl)
                {
                    ((GridControl)this.grid).SmoothControlResize = false;
                }
                //Save original col ratios
                colRatios = new double[this.grid.Model.ColCount + 1];
                double dWidth = this.grid.ClientSize.Width;
                for (int col = 0; col <= this.grid.Model.ColCount; ++col)
                {
                    colRatios[col] = this.grid.Model.ColWidths[col] / dWidth;
                }

                this.grid.Model.QueryColWidth += new GridRowColSizeEventHandler(grid_QueryColWidth);
                this.grid.Model.ColWidthsChanged += new GridRowColSizeChangedEventHandler(grid_ColWidthsChanged);
                this.grid.ResizingColumns += new GridResizingColumnsEventHandler(grid_ResizingColumns);
            }
        }

        public void UnwireGrid()
        {
            this.grid.Model.QueryColWidth -= new GridRowColSizeEventHandler(grid_QueryColWidth);
            this.grid.Model.ColWidthsChanged -= new GridRowColSizeChangedEventHandler(grid_ColWidthsChanged);
            this.grid.ResizingColumns -= new GridResizingColumnsEventHandler(grid_ResizingColumns);

            this.grid = null;
        }

        private void grid_ResizingColumns(object sender, GridResizingColumnsEventArgs e)
        {
            if (_colSizeBehavior == GridColSizeBehavior.EqualProportional)
                e.Cancel = true;
            else if (_colSizeBehavior == GridColSizeBehavior.FillRightColumn && e.Columns.Right == this.grid.Model.ColCount)
                e.Cancel = true;
            else if (_colSizeBehavior == GridColSizeBehavior.FillLeftColumn && e.Columns.Left == this.grid.Model.Cols.HeaderCount + 1)
                e.Cancel = true;
        }
        private void grid_QueryColWidth(object sender, GridRowColSizeEventArgs e)
        {
            switch (_colSizeBehavior)
            {
                case GridColSizeBehavior.FillRightColumn:
                    if (e.Index == this.grid.Model.ColCount)
                    {
                        e.Size = this.grid.ClientSize.Width - this.grid.Model.ColWidths.GetTotal(0, this.grid.Model.ColCount - 1);
                        e.Handled = true;
                    }
                    break;
                case GridColSizeBehavior.FillLeftColumn:
                    if (e.Index == this.grid.Model.Cols.FrozenCount + 1)
                    {
                        int leftPiece = this.grid.Model.ColWidths.GetTotal(0, this.grid.Model.Cols.FrozenCount);
                        int rightPiece = this.grid.Model.ColWidths.GetTotal(this.grid.Model.Cols.FrozenCount + 2, this.grid.Model.ColCount);
                        e.Size = this.grid.ClientSize.Width - leftPiece - rightPiece;
                        e.Handled = true;
                    }
                    break;
                //				case GridColSizeBehavior.FixedProportional:
                //					if(e.Index == this.grid.Model.ColCount)
                //					{
                //						e.Size = this.grid.ClientSize.Width - this.grid.Model.ColWidths.GetTotal(0, this.grid.Model.ColCount - 1);
                //					}
                //					else
                //					{
                //						e.Size = (int) (this.colRatios[e.Index] * this.grid.ClientSize.Width);
                //					}
                //					e.Handled = true;
                //					break;
                case GridColSizeBehavior.EqualProportional:
                    if (e.Index == this.grid.Model.ColCount)
                    {
                        e.Size = this.grid.ClientSize.Width - this.grid.Model.ColWidths.GetTotal(0, this.grid.Model.ColCount - 1);
                    }
                    else
                    {
                        e.Size = (int)(this.colRatios[e.Index] * this.grid.ClientSize.Width);
                    }
                    e.Handled = true;

                    break;
                default:
                    break;
            }
        }

        private bool inColWidthsChanged = false;
        private void grid_ColWidthsChanged(object sender, GridRowColSizeChangedEventArgs e)
        {
            if (this.inColWidthsChanged)
                return;
            inColWidthsChanged = true;

            if (this._colSizeBehavior != GridColSizeBehavior.EqualProportional)
            {
                this.colRatios = new double[this.grid.Model.ColCount + 1];
                double dWidth = this.grid.ClientSize.Width;
                for (int col = 0; col <= this.grid.Model.ColCount; ++col)
                {
                    this.colRatios[col] = this.grid.Model.ColWidths[col] / dWidth;
                }
            }
            inColWidthsChanged = false;
        }
        public GridColSizeBehavior ColSizeBehavior
        {
            get { return _colSizeBehavior; }
            set { _colSizeBehavior = value; }
        }
        public enum GridColSizeBehavior
        {
            None = 0,
            FillRightColumn,
            FillLeftColumn,
            EqualProportional
        }
    }
}
