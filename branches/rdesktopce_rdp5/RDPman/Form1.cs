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
        }
        void initForm()
        {
            txtHost.Text = _settings._sHostname;
            txtPass.Text = _settings._sPass;
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
    }
}