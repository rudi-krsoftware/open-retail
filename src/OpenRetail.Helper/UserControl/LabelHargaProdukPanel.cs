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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.Helper.UserControl
{
    public class LabelHargaProdukPanel : Panel
    {
        public LabelHargaProdukPanel()
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

        public string KodeProduk { get; set; }
        public string NamaProduk { get; set; }
        public double HargaProduk { get; set; }
        public Nullable<DateTime> LastUpdate { get; set; }
        public string FontName { get; set; }

        private void DrawString(string s, Graphics g, Font font, SolidBrush brush, int nTop)
        {
            var size = g.MeasureString(s, font);
            var nLeft = Convert.ToInt32((this.ClientRectangle.Width / 2) - (size.Width / 2));

            g.DrawString(s, font, brush, nLeft, nTop);
        }

        private IEnumerable<string> SplitByLength(string s, int length)
        {
            for (int i = 0; i < s.Length; i += length)
            {
                if (i + length <= s.Length)
                {
                    yield return s.Substring(i, length);
                }
                else
                {
                    yield return s.Substring(i);
                }
            }
        }

        public void GenerateLabel()
        {
            // Allocate new barcode image as needed
            if (!string.IsNullOrEmpty(NamaProduk))
            {
                try
                {
                    var fontName = string.IsNullOrEmpty(this.FontName) ? "Courier New" : this.FontName;

                    var resultImage = new Bitmap(180, 85); // is bottom padding, adjust to your text

                    using (var graphics = Graphics.FromImage(resultImage))
                    {
                        using (var font = new Font(fontName, 9.5f))
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

                                    var y = 0; // vertial

                                    var sf = new StringFormat();
                                    sf.Alignment = StringAlignment.Center;

                                    var size = graphics.MeasureString(string.Format("{0:N0}", HargaProduk), font);
                                    var nLeft = Convert.ToInt32((this.ClientRectangle.Width / 2) - (size.Width / 2));
                                    var nTop = Convert.ToInt32((this.ClientRectangle.Height / 2) - (size.Height / 2));

                                    var arrNamaProduk = SplitByLength(NamaProduk, 23).ToList();

                                    if (arrNamaProduk.Count > 0)
                                        this.DrawString(arrNamaProduk[0], graphics, font, brush, y);

                                    if (arrNamaProduk.Count > 1)
                                    {
                                        y += 15;
                                        this.DrawString(arrNamaProduk[1], graphics, font, brush, y);
                                    }

                                    y += 15;
                                    this.DrawString(KodeProduk, graphics, font, brush, y);

                                    y += 15;
                                    graphics.DrawString("Rp.", font, brush, nLeft - 35, y);
                                    this.DrawString(string.Format("{0:N0}", HargaProduk), graphics, new Font(fontName, 14f, FontStyle.Bold), brush, (int)y - 2);

                                    if (LastUpdate != null)
                                    {
                                        y += 20;
                                        this.DrawString(string.Format("{0:dd-MM-yyyy}", LastUpdate), graphics, font, brush, y);
                                    }
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
        }
    }
}