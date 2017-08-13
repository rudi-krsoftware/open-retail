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

using OpenRetail.App.UserControl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.Helper
{
    public enum ColorSection
    {
        FormBackColor = 1, PanelHeaderBackColor = 2,
        PanelFooterBackColor = 3, LabelHeaderForeColor = 4,
        TextFocusColor = 5
    }

    public static class ColorManagerHelper
    {
        public static void SetTheme(Form frm, Control parent)
        {
            frm.BackColor = GetRgbColor(ColorSection.FormBackColor);

            var frmName = frm.Name;

            if (!(frmName == "FrmLogin" || frmName == "FrmAbout" || frmName.Substring(0, 10) == "FrmPreview"))
            {
                frm.ShowIcon = false;
                frm.ShowInTaskbar = false;
            }

            Color color = Color.White;

            foreach (Control ctl in parent.Controls)
            {

                var ctlType = ctl.GetType().Name;

                switch (ctlType)
                {
                    case "AdvancedTextbox":
                        SetFormatAdvancedTextbox(ctl);
                        break;

                    case "MaskedTextBox":
                        var mask = (MaskedTextBox)ctl;

                        if (mask.Tag != null)
                        {
                            var tag = mask.Tag.ToString();

                            if (tag == "jam")
                            {
                                mask.Size = new Size(36, 20);
                                mask.Mask = "##:##";
                            }
                            else // tanggal
                            {
                                mask.Size = new Size(68, 20);
                                mask.Mask = "##/##/####";
                            }
                        }

                        break;

                    case "Panel":
                        var objPanel = (Panel)ctl;

                        color = Color.White;
                        if (objPanel.Name.Length > 8)
                        {
                            if (objPanel.Name == "pnlHeader")
                            {
                                color = GetRgbColor(ColorSection.PanelHeaderBackColor);
                                objPanel.BorderStyle = BorderStyle.FixedSingle;
                            }
                            else if (objPanel.Name.Substring(0, 9) == "pnlFooter")
                            {
                                color = GetRgbColor(ColorSection.PanelFooterBackColor);
                                objPanel.BorderStyle = BorderStyle.FixedSingle;
                            }                            
                        }

                        objPanel.BackColor = color;

                        break;

                    case "Label":
                        var objLabel = (Label)ctl;

                        if (objLabel.Name == "lblHeader")
                        {
                            objLabel.ForeColor = GetRgbColor(ColorSection.LabelHeaderForeColor);
                            objLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
                        }

                        break;

                    default:
                        break;
                }

                SetTheme(frm, ctl);
            }
        }

        private static void SetFormatAdvancedTextbox(object obj)
        {
            var advTextBox = (AdvancedTextbox)obj;

            advTextBox.EnterFocusColor = GetRgbColor(ColorSection.TextFocusColor);

            if (!advTextBox.Enabled)
            {
                advTextBox.BackColor = Color.FromArgb(232, 235, 242);
            }

            if (advTextBox.NumericOnly)
                advTextBox.Text = "0";
        }

        /// <summary>
        /// Method untuk membaca konfigurasi warna yang di simpan di file app.config
        /// </summary>
        /// <param name="colorSection"></param>
        /// <returns></returns>
        private static Color GetRgbColor(ColorSection colorSection)
        {
            var rgbColor = new string[] { "0", "0", "0" };

            var colorManager = (NameValueCollection)ConfigurationManager.GetSection("colorManager");
            if (colorManager != null)
            {
                rgbColor = colorManager[colorSection.ToString()].Split(',');
            }
            else
            {
                // set warna default
                switch (colorSection)
                {
                    case ColorSection.FormBackColor:
                        rgbColor = new string[] { "255", "255", "255" };
                        break;

                    case ColorSection.PanelHeaderBackColor:
                        rgbColor = new string[] { "31", "86", "125" };
                        break;

                    case ColorSection.PanelFooterBackColor:
                        rgbColor = new string[] { "31", "86", "125" };
                        break;

                    case ColorSection.LabelHeaderForeColor:
                        rgbColor = new string[] { "255", "255", "255" };
                        break;

                    case ColorSection.TextFocusColor:
                        rgbColor = new string[] { "255", "255", "192" };
                        break;

                    default:
                        break;
                }
            }

            return Color.FromArgb(Convert.ToInt32(rgbColor[0]), Convert.ToInt32(rgbColor[1]), Convert.ToInt32(rgbColor[2]));
        }
    }
}
