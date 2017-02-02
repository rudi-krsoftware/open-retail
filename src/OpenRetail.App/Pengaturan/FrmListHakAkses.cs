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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;
using OpenRetail.App.UserControl;
using ConceptCave.WaitCursor;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.Windows.Forms.Tools;
using log4net;

namespace OpenRetail.App.Pengaturan
{
    public partial class FrmListHakAkses : FrmListEmptyBody, IListener
    {
        private IRoleBll _bll; // deklarasi objek business logic layer 
        private IList<Role> _listOfRole = new List<Role>();
        private IList<RolePrivilege> _listOfRolePrivilege = null;
        private IList<MenuAplikasi> _listOfMenuAplikasi = null;
        private ILog _log;

        public FrmListHakAkses(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new RoleBll(_log);            

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                {
                    LoadMenuParent();
                    SetMenuParent(cmbMenu);

                    LoadData();
                }

                cmbMenu.Enabled = role.is_grant;
                chkPilihSemua.Enabled = role.is_grant;
                btnSimpan.Enabled = role.is_grant;
            }    

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfRole.Count);
        }                

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Hak Akses", Width = 400 });
            gridListProperties.Add(new GridListControlProperties { Header = "Status", Width = 100 });

            GridListControlHelper.InitializeGridListControl<Role>(this.gridList, _listOfRole, gridListProperties);

            if (_listOfRole.Count > 0)
            {
                this.gridList.SetSelected(0, true);
                HandleSelectionChanged(this.gridList);
            }                

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfRole.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {

                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfRole.Count)
                        {
                            var role = _listOfRole[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = role.nama_role;
                                    break;

                                case 3:
                                    e.Style.CellValue = role.is_active ? "Aktif" : "Non Aktif";
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                default:
                                    break;
                            }

                            // we handled it, let the grid know
                            e.Handled = true;
                        }
                    }
                }
            };
        }

        private bool IsAdministrator(string roleName)
        {
            return roleName.ToLower() == "administrator";
        }

        private void LoadData()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfRole = _bll.GetAll();

                GridListControlHelper.Refresh<Role>(this.gridList, _listOfRole);
            }

            ResetButton();
        }

        private void LoadMenuParent()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                IMenuBll menuBll = new MenuBll(_log);
                _listOfMenuAplikasi = menuBll.GetAll();
            }
        }

        private void SetMenuParent(ComboBox combo)
        {
            foreach (var menu in _listOfMenuAplikasi.Where(f => f.parent_id == null && f.is_active == true))
            {
                cmbMenu.Items.Add(menu.judul_menu);
            }

            cmbMenu.SelectedIndex = 0;
        }

        private void LoadMenuChild(string menuParentId)
        {

            treeViewAdv.Nodes.Clear();

            var menuChild = _listOfMenuAplikasi.Where(f => f.parent_id == menuParentId && f.is_active == true && f.nama_form.Length > 0)
                                               .OrderBy(f => f.order_number)
                                               .ToList();

            foreach (var itemMenuChild in menuChild)
            {
                var nodeChild = new TreeNodeAdv();
                nodeChild.Text = itemMenuChild.judul_menu;
                nodeChild.Tag = itemMenuChild.menu_id;
                nodeChild.ShowCheckBox = true;
                nodeChild.InteractiveCheckBox = true;
                nodeChild.ExpandAll();

                IItemMenuBll itemMenuBll = new ItemMenuBll(_log);
                var listOfItemMenu = itemMenuBll.GetByMenu(itemMenuChild.menu_id);

                // filter menu laporan yg ditampilkan hanya hak akses SELECT
                if (itemMenuChild.nama_menu.Substring(0, 6) == "mnuLap")
                {
                    listOfItemMenu = listOfItemMenu.Where(f => f.grant_id == Convert.ToInt32(GrantState.SELECT)).ToList();
                }

                foreach (var itemMenu in listOfItemMenu)
                {
                    var nodeTag = new TreeNodeAdv();
                    nodeTag.Text = itemMenu.keterangan;
                    nodeTag.Tag = itemMenu.grant_id;
                    nodeTag.ShowCheckBox = true;
                    nodeTag.InteractiveCheckBox = true;

                    nodeChild.Nodes.Add(nodeTag);
                }

                treeViewAdv.Nodes.Add(nodeChild);
            }

        }

        private void CheckRecursive(TreeNodeAdv treeNode, bool isSave)
        {
            var nodeParent = treeNode.Parent;

            if (isSave)
            {
                if (!base.IsSelectedItem(this.gridList.SelectedIndex, this.Text))
                    return;

                var obj = _listOfRole[this.gridList.SelectedIndex];

                if (nodeParent.Tag != null)
                {
                    var rolePrivilege = new RolePrivilege
                    {
                        role_id = obj.role_id,
                        menu_id = nodeParent.Tag.ToString(),
                        grant_id = Convert.ToInt32(treeNode.Tag),
                        is_grant = treeNode.CheckState == CheckState.Checked
                    };

                    IRolePrivilegeBll rolePrivilegeBll = new RolePrivilegeBll(_log);
                    var result = rolePrivilegeBll.Save(rolePrivilege);
                }
            }
            else
            {
                treeNode.CheckState = CheckState.Unchecked;

                if (_listOfRolePrivilege != null)
                {
                    if (nodeParent.Tag != null)
                    {
                        if (treeNode.Tag != null)
                        {
                            var role = _listOfRole[this.gridList.SelectedIndex];

                            var rolePrivilege = _listOfRolePrivilege.Where(f => f.role_id == role.role_id && f.menu_id == nodeParent.Tag.ToString() &&
                                                                          f.grant_id == Convert.ToInt32(treeNode.Tag))
                                                                          .SingleOrDefault();
                            if (rolePrivilege != null)
                                treeNode.CheckState = rolePrivilege.is_grant ? CheckState.Checked : CheckState.Unchecked;
                        }
                    }
                }
            }

            // Print each node recursively.
            foreach (TreeNodeAdv tn in treeNode.Nodes)
            {
                CheckRecursive(tn, isSave);
            }
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfRole.Count > 0);
        }

        private void EnabledObject(bool isEnabled)
        {

            cmbMenu.Enabled = isEnabled;
            treeViewAdv.Enabled = isEnabled;
            chkPilihSemua.Enabled = isEnabled;
            btnSimpan.Enabled = isEnabled;
        }

        private void HandleSelectionChanged(GridListControl gridList)
        {
            if (gridList.SelectedIndex < 0)
                return;

            var role = _listOfRole[gridList.SelectedIndex];

            EnabledObject(true);
            lblRole.Text = "Hak akses : ";

            if (role != null)
            {
                if (IsAdministrator(role.nama_role))
                    EnabledObject(false);

                lblRole.Text = string.Format("Hak akses : {0}", role.nama_role);

                IRolePrivilegeBll rolePrivilegeBll = new RolePrivilegeBll(_log);
                _listOfRolePrivilege = rolePrivilegeBll.GetByRole(role.role_id);

                foreach (TreeNodeAdv node in treeViewAdv.Nodes)
                {
                    CheckRecursive(node, false);
                }
            }
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryHakAkses("Tambah Data " + this.Text, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var role = _listOfRole[index];

            if (IsAdministrator(role.nama_role))
            {
                MsgHelper.MsgWarning("Maaf hak akses 'Administrator' tidak bisa diedit");
                return;
            }

            var frm = new FrmEntryHakAkses("Edit Data " + this.Text, role, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var role = _listOfRole[index];

            if (IsAdministrator(role.nama_role))
            {
                MsgHelper.MsgWarning("Maaf hak akses 'Administrator' tidak bisa dihapus");
                return;
            }

            if (MsgHelper.MsgDelete())
            {
                var result = _bll.Delete(role);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Role>(this.gridList, _listOfRole, role);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        public void Ok(object sender, object data)
        {
            throw new NotImplementedException();
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var role = (Role)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Role>(this.gridList, _listOfRole, role);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Role>(this.gridList, _listOfRole, role);
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            foreach (TreeNodeAdv node in treeViewAdv.Nodes)
            {
                CheckRecursive(node, true);
            }

            HandleSelectionChanged(this.gridList);
        }

        private void chkPilihSemua_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            foreach (TreeNodeAdv node in treeViewAdv.Nodes)
            {
                PilihSemua(node, chk.Checked);
            }
        }

        private void PilihSemua(TreeNodeAdv treeNode, bool isChecked)
        {
            treeNode.CheckState = isChecked ? CheckState.Checked : CheckState.Unchecked;

            foreach (TreeNodeAdv tn in treeNode.Nodes)
            {
                PilihSemua(tn, isChecked);
            }
        }

        private void cmbMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                var obj = (ComboBox)sender;

                var menuParent = _listOfMenuAplikasi.Where(f => f.parent_id == null && f.is_active == true).ToList()[obj.SelectedIndex];

                if (menuParent != null)
                    LoadMenuChild(menuParent.menu_id);

                foreach (TreeNodeAdv node in treeViewAdv.Nodes)
                {
                    CheckRecursive(node, false);
                }

                chkPilihSemua.Checked = false;
            }


            chkPilihSemua.Checked = false;
        }

        private void gridList_SelectedValueChanged(object sender, EventArgs e)
        {
            HandleSelectionChanged((GridListControl)sender);
        }
    }
}
