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

using OpenRetail.Helper;
using System;
using System.Windows.Forms;

namespace OpenRetail.App.Main
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            ShowInfoAbout();
        }

        private void ShowInfoAbout()
        {
            var firstReleaseYear = 2017;
            var currentYear = DateTime.Today.Year;
            var copyright = currentYear > firstReleaseYear ? string.Format("{0} - {1}", firstReleaseYear, currentYear) : firstReleaseYear.ToString();

            lblVersion.Text = string.Format("Version {0}{1}", MainProgram.currentVersion, MainProgram.stageOfDevelopment);
            lblCopyright.Text = string.Format("Copyright © {0} Kamarudin | Email: rudi.krsoftware@gmail.com | Phone: +62 813 8176 9915", copyright);

            lblUrl1.Text = "https://github.com/rudi-krsoftware/open-retail";
            lblUrl1.LinkClicked += lblUrl_LinkClicked;

            lblUrl2.Text = "http://coding4ever.net/";
            lblUrl2.LinkClicked += lblUrl_LinkClicked;
        }

        private void lblUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = (LinkLabel)sender;

            // Specify that the link was visited.
            link.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start(link.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAbout_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                btnOk_Click(sender, e);
        }

        private void imgDonate_Click(object sender, EventArgs e)
        {
            var url = "https://github.com/rudi-krsoftware/open-retail/wiki/Cara-Berkontribusi/";

            // Navigate to a URL.
            System.Diagnostics.Process.Start(url);
        }
    }
}