using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Syncfusion.Windows.Forms.Grid;

namespace OpenRetail.App.Helper
{
    public sealed class GridListControlProperties
    {
        public GridListControlProperties()
        {
            this.IsEditable = true;
            this.HorizontalAlignment = GridHorizontalAlignment.Left;
        }

        public string Header { get; set; }
        public string FieldName { get; set; }
        public int Width { get; set; }
        public bool IsEditable { get; set; }
        public GridHorizontalAlignment HorizontalAlignment { get; set; }
    }
}
