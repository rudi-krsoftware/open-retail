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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zen.Barcode;

namespace OpenRetail.Helper.UserControl
{
    /// <summary>
    /// BarcodePanel user control base on Zen.Barcode.Rendering.Framework (http://barcoderender.codeplex.com/)
    /// </summary>
    public class BarcodePanel : Panel
    {
        #region Private Fields
        private BarcodeSymbology _symbology;
        private int _maxBarHeight = 30;
        private int _scaleBarWidth = 1;
        private string _headerLabel = string.Empty;
        private double _priceLabel = 0;
        private bool _isDisplayPriceLabel = true;
        #endregion

        #region Public Constructors
        /// <summary>
        /// Initialises an instance of <see cref="T:BarcodePanel" />.
        /// </summary>
        public BarcodePanel()
        {
            this.SuspendLayout();
            // 
            // BarcodePanel
            // 
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResumeLayout(false);
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the barcode symbology.
        /// </summary>
        /// <value>The symbology.</value>
        [Category("OpenRetailBarcodeRendering")]
        [DefaultValue((int)BarcodeSymbology.Unknown)]
        [TypeConverter(typeof(EnumConverter))]
        [Description("Gets/sets the barcode symbology used by this control.")]
        public BarcodeSymbology Symbology
        {
            get
            {
                return _symbology;
            }
            set
            {
                if (_symbology != value)
                {
                    _symbology = value;
                    RefreshBarcodeImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <value></value>
        /// <returns>The text associated with this control.</returns>
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    RefreshBarcodeImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum height of the rendered barcode.
        /// </summary>
        /// <value>The maximum height of a barcode bar in pixels.</value>
        [Category("OpenRetailBarcodeRendering")]
        [DefaultValue(30)]
        [Description("Gets/sets the maximum height of the rendered barcode bars.")]
        public int MaxBarHeight
        {
            get
            {
                return _maxBarHeight;
            }
            set
            {
                if (_maxBarHeight != value)
                {
                    _maxBarHeight = value;
                    RefreshBarcodeImage();
                }
            }
        }

        [Category("OpenRetailBarcodeRendering")]
        [DefaultValue(1)]
        [Description("Gets/sets the scale bar width of the rendered barcode bars.")]
        public int ScaleBarWidth
        {
            get
            {
                return _scaleBarWidth;
            }
            set
            {
                if (_scaleBarWidth != value)
                {
                    _scaleBarWidth = value;
                    RefreshBarcodeImage();
                }
            }
        }

        [Category("OpenRetailBarcodeRendering")]
        public double PriceLabel
        {
            get
            {
                return _priceLabel;
            }
            set
            {
                if (_priceLabel != value)
                {
                    _priceLabel = value;
                    RefreshBarcodeImage();
                }
            }
        }

        [Category("OpenRetailBarcodeRendering")]
        public bool IsDisplayPriceLabel
        {
            get
            {
                return _isDisplayPriceLabel;
            }
            set
            {
                if (_isDisplayPriceLabel != value)
                {
                    _isDisplayPriceLabel = value;
                    RefreshBarcodeImage();
                }
            }
        }

        [Category("OpenRetailBarcodeRendering")]
        public string HeaderLabel
        {
            get
            {
                return _headerLabel;
            }
            set
            {
                if (_headerLabel != value)
                {
                    _headerLabel = value;
                    RefreshBarcodeImage();
                }
            }
        }

        #endregion

        #region Private Methods
        private void RefreshBarcodeImage()
        {
            // Allocate new barcode image as needed
            if (_symbology != BarcodeSymbology.Unknown && !string.IsNullOrEmpty(Text))
            {
                try
                {
                    var drawObject = BarcodeDrawFactory.GetSymbology(_symbology);

                    var metrics = drawObject.GetDefaultMetrics(_maxBarHeight);
                    metrics.Scale = _scaleBarWidth;

                    var barcodeImage = drawObject.Draw(Text, metrics);

                    Bitmap resultImage = null;

                    if (IsDisplayPriceLabel)
                        resultImage = new Bitmap(barcodeImage.Width, barcodeImage.Height + 37); // is bottom padding, adjust to your text
                    else
                        resultImage = new Bitmap(barcodeImage.Width, barcodeImage.Height + 30); // is bottom padding, adjust to your text

                    using (var graphics = Graphics.FromImage(resultImage))
                    {
                        using (var font = new Font("Consolas", 10))
                        {
                            using (var brush = new SolidBrush(Color.Black))
                            {
                                using (var format = new StringFormat()
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Far
                                })
                                {
                                    graphics.Clear(Color.White);

                                    float headerLabelY = 70f;
                                    float barcodeImageY = 15f;
                                    float barcodeY = 7f;

                                    if (!IsDisplayPriceLabel)
                                    {
                                        headerLabelY = 65f;
                                        barcodeImageY = 14f;
                                        barcodeY = 0f;
                                    }

                                    // draw barcode header
                                    graphics.DrawString(HeaderLabel, font, brush, resultImage.Width / 2, resultImage.Height - headerLabelY, format);

                                    // draw barcode image
                                    graphics.DrawImage(barcodeImage, 0, barcodeImageY);

                                    // draw barcode code
                                    graphics.DrawString(Text, font, brush, resultImage.Width / 2, resultImage.Height - barcodeY, format);

                                    // draw barcode price
                                    if (IsDisplayPriceLabel)
                                        graphics.DrawString(string.Format("Rp.{0:N0}", PriceLabel), font, brush, resultImage.Width / 2, resultImage.Height + 3, format);
                                }
                            }
                        }
                    }

                    BackgroundImage = resultImage;
                }
                catch
                {
                    BackgroundImage = null;
                }
            }
            else
            {
                BackgroundImage = null;
            }

            // Setup the auto-scroll minimum size
            if (BackgroundImage == null)
            {
                AutoScrollMinSize = new Size(0, 0);
            }
            else
            {
                AutoScrollMinSize = BackgroundImage.Size;
            }
        }
        #endregion
    }
}
