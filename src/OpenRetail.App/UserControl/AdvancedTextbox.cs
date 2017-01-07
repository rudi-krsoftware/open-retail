using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.UserControl
{
    #region ">> Enumerators <<"

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public enum EConversion
    {
        Normal = 0,
        UpperCase = 1
    }

    #endregion

    public class AdvancedTextbox : TextBox
    {
        private Color _enterFocusColor = Color.White;
        private Color _leaveFocusColor = Color.White;
        private EConversion _conversion;
        private bool _isSelectionText = false;
        private bool _isThousandSeparator = false;
        private bool _isNumericOnly = false;
        private bool _isLetterOnly = false;
        private bool _isAutoEnter = false;
        private bool _isDecimal = false;

        #region >> property <<

        public override int MaxLength
        {
            get { return base.MaxLength; }

            set
            {
                if (this._isThousandSeparator && value > 20)
                    value = 20;
                base.MaxLength = value;
            }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public Color EnterFocusColor
        {
            get { return _enterFocusColor; }

            set { _enterFocusColor = value; }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public Color LeaveFocusColor
        {
            get { return _leaveFocusColor; }

            set { _leaveFocusColor = value; }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public bool SelectionText
        {
            get { return _isSelectionText; }
            set { _isSelectionText = value; }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public bool ThousandSeparator
        {
            get { return _isThousandSeparator; }

            set
            {
                _isThousandSeparator = value;

                if (_isThousandSeparator)
                {
                    _isNumericOnly = true;
                    this.MaxLength = 15;
                    this.TextAlign = HorizontalAlignment.Right;
                    this.Text = "0";
                }
            }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public bool NumericOnly
        {
            get { return _isNumericOnly; }
            set { _isNumericOnly = value; }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public bool LetterOnly
        {
            get { return _isLetterOnly; }
            set { _isLetterOnly = value; }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public bool AutoEnter
        {
            get { return _isAutoEnter; }
            set { _isAutoEnter = value; }
        }

        [System.ComponentModel.Category("AdvancedTextbox Properties")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)]
        public EConversion Conversion
        {
            get { return _conversion; }
            set { _conversion = value; }
        }

        #endregion

        public AdvancedTextbox()
        {
            TextChanged += AdvancedTextbox_TextChanged;
            Leave += AdvancedTextbox_Leave;
            KeyPress += AdvancedTextbox_KeyPress;
            Enter += AdvancedTextbox_Enter;
        }        

        #region ">> delegate method <<"

        private void AdvancedTextbox_Enter(object sender, EventArgs e)
        {
            if (this._isSelectionText)
                SeleksiText((System.Windows.Forms.TextBox)sender);

            this.BackColor = this._enterFocusColor;
        }

        private void AdvancedTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this._isAutoEnter)
                if (e.KeyChar == (char)(Keys.Return))
                    SendKeys.Send("{Tab}");

            if (this._isNumericOnly)
            {
                if (_isDecimal && e.KeyChar == '.')
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = ValidasiAngka(e);
                }

            }
            else if (this._isLetterOnly)
            {
                if (this._conversion == EConversion.UpperCase)
                    e.KeyChar = HurufBesar(e);
                e.Handled = ValidasiHuruf(e);

            }
            else if (this._conversion == EConversion.UpperCase)
            {
                e.KeyChar = HurufBesar(e);
            }
        }

        private void AdvancedTextbox_Leave(object sender, EventArgs e)
        {
            if (this._isNumericOnly)
                if (!(this.Text.Length > 0))
                    this.Text = "0";

            this.BackColor = this._leaveFocusColor;
        }

        private void AdvancedTextbox_TextChanged(object sender, EventArgs e)
        {
            _isDecimal = false;

            int index = this.Text.IndexOf(".");
            _isDecimal = !(index < 0);

            if (this._isNumericOnly && this._isThousandSeparator)
            {
                if (this.Text.Length > 0)
                {
                    if (this.Text.Substring(0, 1) == ".")
                        this.Text = this.Text.Replace(".", "");

                    long x = Convert.ToInt64(this.Text.Replace(",", ""));
                    string strAfterFormat = string.Format("{0:N0}", x);

                    if (this.Text != strAfterFormat)
                    {
                        int pos = this.Text.Length - this.SelectionStart;

                        this.Text = strAfterFormat;
                        if (((this.Text.Length - pos) < 0))
                        {
                            this.SelectionStart = 0;
                        }
                        else
                        {
                            this.SelectionStart = this.Text.Length - pos;
                        }
                    }
                }
            }
        }

        #endregion

        #region ">> private method <<"

        private char HurufBesar(System.Windows.Forms.KeyPressEventArgs e)
        {
            return Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        private void SeleksiText(System.Windows.Forms.TextBox sender)
        {
            sender.SelectionStart = 0;
            sender.SelectionLength = sender.Text.Length;
        }

        private bool ValidasiAngka(System.Windows.Forms.KeyPressEventArgs e)
        {
            var strValid = "0123456789";

            if (!_isThousandSeparator)
                strValid += ".";

            var pos = strValid.IndexOf(e.KeyChar);
            if (pos < 0 && !(e.KeyChar == (char)(Keys.Back)))
            {
                return true; // not valid                
            }
            else
            {
                return false; // valid                
            }
        }

        private bool ValidasiHuruf(System.Windows.Forms.KeyPressEventArgs e)
        {
            var strValid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ. ";

            var pos = strValid.IndexOf(e.KeyChar);
            if (pos < 0 && !(e.KeyChar == (char)(Keys.Back)))
            {
                return true; // not valid                
            }
            else
            {
                return false; // valid                
            }
        }

        #endregion        
    }
}
