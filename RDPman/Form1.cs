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
            updateForm();
        }

        void updateForm()
        {
            txtHost.Text = _settings.ServerName;
            txtPass.Text = DPAPI.Decrypt( _settings.Password );
            if (_settings.SavePassword == 1)
                chkSavePassword.Checked = true;
            else
                chkSavePassword.Checked = false;

            txtPort.Text = _settings.MCSPort.ToString();
            txtUser.Text = _settings.UserName;
            txtDomain.Text = _settings.Domain;

            txtWidth.Text = _settings.DesktopHeight.ToString();
            txtHeight.Text = _settings.DesktopWidth.ToString();
            setBPP(_settings.ColorDepthID);
            if(_settings.ScreenStyle==1)
                chkFullscreen.Checked = true;
            else
                chkFullscreen.Checked = false;

            if (_settings.usebarcodereader == 1)
                chkBarcodeReader.Checked = true;
            else
                chkBarcodeReader.Checked = false;

            txtProgramLocation.Text = _settings.rdesktopce;
        }

        void updateSettings()
        {
            _settings.ServerName = txtHost.Text;
            _settings.Password = txtPass.Text;
            try
            {
                int iP = Convert.ToInt16(txtPort.Text);
                _settings.MCSPort = iP;
            }
            catch (Exception)
            {
                txtPort.BackColor = Color.LightPink;
                tabHost.Focus();
                txtPort.Focus();
                return;
            }
            _settings.UserName = txtUser.Text;
            if(_settings.SavePassword==1)
                _settings.Password = txtPass.Text;
            else
                _settings.Password = "";

            _settings.Domain = txtDomain.Text;
            try
            {
                int iW = Convert.ToInt16(txtWidth.Text);
                _settings.DesktopHeight = iW;
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
                _settings.DesktopWidth = iH;
            }
            catch (Exception)
            {
                txtHeight.BackColor = Color.LightPink;
                tabScreen.Focus();
                txtHeight.Focus();
                return;
            }
            _settings.ColorDepthID = getBPP();
            if (chkFullscreen.Checked)
                _settings.ScreenStyle = 1;
            else
                _settings.ScreenStyle = 0;
            _settings.rdesktopce = txtProgramLocation.Text;

            if (chkBarcodeReader.Checked)
                _settings.usebarcodereader = 1;
            else
                _settings.usebarcodereader = 0;

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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string sProg = "";
            Lime49.OpenFileDialog ofd = new Lime49.OpenFileDialog();
            ofd.Filter = "exe";// "applications|*.exe|all files|*.*";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sProg = ofd.SelectedFile;
                if(System.IO.File.Exists(sProg))
                    txtProgramLocation.Text = sProg;
            }
            ofd.Dispose();
        }

        private void mnuLoad_Click(object sender, EventArgs e)
        {
            string sXmlFile = "";
            Lime49.OpenFileDialog ofd = new Lime49.OpenFileDialog();
            ofd.Filter = "xml";// "applications|*.exe|all files|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sXmlFile = ofd.SelectedFile;
                if (System.IO.File.Exists(sXmlFile))
                {
                    rdp_settings rdpsett = rdp_settings.loadSettings(sXmlFile);
                    if (rdpsett != null)
                    {
                        _settings = rdpsett;
                        updateForm();
                    }
                }
            }
            ofd.Dispose();

        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            string sXmlFile = "";
            Lime49.OpenFileDialog ofd = new Lime49.OpenFileDialog();
            ofd.Filter = "xml";// "applications|*.exe|all files|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sXmlFile = ofd.SelectedFile;
                _settings.writeXML(sXmlFile);
            }
            ofd.Dispose();
        }
    }
}