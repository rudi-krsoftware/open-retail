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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenRetail.Model;

namespace OpenRetail.App.Helper
{
    public static class RolePrivilegeHelper
    {
        public static bool IsHaveHakAkses(string menuName, Pengguna pengguna, GrantState grantState = GrantState.CREATE)
        {
            bool isGrant = false;

            var role = pengguna.GetRoleByMenuNameAndGrant(menuName, grantState);
            if (role != null)
                isGrant = role.is_grant;

            return isGrant;
        }

        /// <summary>
        /// Method untuk mengeset hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pengguna"></param>
        /// <param name="menuId"></param>
        /// <param name="recordCount"></param>
        public static void SetHakAkses(Control parent, Pengguna pengguna, string menuId, int recordCount)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is Button)
                {
                    var btn = (Button)ctl;

                    if (btn.Tag != null)
                    {
                        var grantState = (GrantState)Convert.ToInt32(btn.Tag);

                        // ambil informasi privilege masing-masing pengguna
                        var listOfRolePrivilege = pengguna.GetRoleByMenu(menuId)
                                                          .Where(f => f.grant_id != Convert.ToInt32(GrantState.SELECT))
                                                          .ToList();

                        // cek hak akses privilege
                        bool isGrant = listOfRolePrivilege.Where(f => f.grant_id == Convert.ToInt32(grantState))
                                                          .Select(f => f.is_grant)
                                                          .SingleOrDefault();

                        btn.Enabled = isGrant;

                        if (btn.Enabled)
                        {
                            if (grantState == GrantState.UPDATE || grantState == GrantState.DELETE)
                                btn.Enabled = recordCount > 0;
                        }
                    }

                }

                SetHakAkses(ctl, pengguna, menuId, recordCount);
            }
        }        
    }
}
