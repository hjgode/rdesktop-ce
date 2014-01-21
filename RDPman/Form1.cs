using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace RDPman
{
    public partial class Form1 : Form
    {
        rdp_settings _settings;
        public Form1()
        {
            InitializeComponent();
            _settings = new rdp_settings();
            initForm();
        }

        void initForm()
        {
            txtHost.Text = _settings._sHostname;
            txtPass.Text = _settings._sPass;
            if (_settings._bSavePassword == 1)
                chkSavePassword.Checked = true;
            else
                chkSavePassword.Checked = false;

            txtPort.Text = _settings._iPort.ToString();
            txtUser.Text = _settings._sUser;
            txtDomain.Text = _settings._sDomain;

            txtWidth.Text = _settings._iWidth.ToString();
            txtHeight.Text = _settings._iHeight.ToString();
            setBPP(_settings._iBPP);
            if(_settings._iFullscreen==1)
                chkFullscreen.Checked = true;
            else
                chkFullscreen.Checked = false;

            if (_settings._iBarcodeReaderSupport == 1)
                chkBarcodeReader.Checked = true;
            else
                chkBarcodeReader.Checked = false;

            txtProgramLocation.Text = _settings._sRdekstopCE;
        }

        void updateSettings()
        {
            _settings._sHostname = txtHost.Text;
            _settings._sPass = txtPass.Text;
            try
            {
                int iP = Convert.ToInt16(txtPort.Text);
                _settings._iPort = iP;
            }
            catch (Exception)
            {
                txtPort.BackColor = Color.LightPink;
                tabHost.Focus();
                txtPort.Focus();
                return;
            }
            _settings._sUser = txtUser.Text;
            if(_settings._bSavePassword==1)
                _settings._sPass = txtPass.Text;
            else
                _settings._sPass = "";

            _settings._sDomain = txtDomain.Text;
            try
            {
                int iW = Convert.ToInt16(txtWidth.Text);
                _settings._iWidth = iW;
            }
            catch (Exception)
            {
                txtWidth.BackColor = Color.LightPink;
                tabScreen.Focus();
                txtWidth.Focus();
                return;
            }
            try
            {
                int iH = Convert.ToInt16(txtHeight.Text);
                _settings._iHeight = iH;
            }
            catch (Exception)
            {
                txtHeight.BackColor = Color.LightPink;
                tabScreen.Focus();
                txtHeight.Focus();
                return;
            }
            _settings._iBPP = getBPP();
            if (chkFullscreen.Checked)
                _settings._iFullscreen = 1;
            else
                _settings._iFullscreen = 0;
            _settings._sRdekstopCE = txtProgramLocation.Text;

            if (chkBarcodeReader.Checked)
                _settings._iBarcodeReaderSupport = 1;
            else
                _settings._iBarcodeReaderSupport = 0;

        }

        void setBPP(int iBPP)
        {
            for (int i = 0; i < cboColorDepth.Items.Count; i++)
            {
                if (cboColorDepth.Items[i].ToString() == iBPP.ToString())
                {
                    cboColorDepth.SelectedIndex = i;
                    break;
                }
            }
        }
        int getBPP()
        {
            int iRet = 8;
            if (cboColorDepth.SelectedIndex == -1)
                return iRet;
            int iTest=0;
            string s = cboColorDepth.SelectedItem.ToString();
            try
            {
                iTest = Convert.ToInt16(s);
                if (iTest == 8 || iTest == 15 || iTest == 16)
                    iRet = iTest;
            }
            catch (Exception)
            {
            }
            return iRet;
        }

        private void mnuStart_Click(object sender, EventArgs e)
        {
            updateSettings();
            string sArg = _settings.getArgList();
            System.Diagnostics.Debug.WriteLine(sArg);
            //string sRDP = _settings.getRDPstring();
            _settings.writeFile("\\test.rdp");
        }
    }
}