using System;

using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RDPman
{
    class rdp_settings
    {
        //-g geometry
        public int _iWidth = 240;
        public int _iHeight = 320;

        //-t port
        public int _iPort = 3389;

        //-a bpp BitsPerPixel, 8 or 15 or 16
        public int _iBPP = 16;

        //-f fullscreen
        public int _iFullscreen = 0;

        //-u user name
        public string _sUser = "hgode";

        //-p password
        public string _sPass = "Chopper+6";

        //-d domain
        public string _sDomain = "";

        //-s shell
        public string _sShell = "";

        //-c directory
        public string _sWorkingDir = "";

        //-n hostname
        public string _sHostname = "192.168.0.112";

        //-x clipboard, currently not supported
        public int _iClipboard = 0;

        //-b barcode reader support
        public int _iBarcodeReaderSupport = 0;

        public string _sRdekstopCE = @"\Program Files\rdesktopce\rdesktopce.exe";

        public int _iSavePassword = 1;

        public string getArgList()
        {
            string sRet = "";
            //-g
            sRet += " -g "+_iWidth.ToString()+"x"+_iHeight.ToString();
            sRet += " -n " + _sHostname + " -t " + _iPort.ToString();
            sRet += " -u " + _sUser + " -p " + _sPass;
            if (_sDomain.Length > 0)
                sRet += " -d " + _sDomain;
            if (_sShell.Length > 0)
                sRet += " -s " + _sShell;
            if (_sWorkingDir.Length > 0)
                sRet += " -c " + _sWorkingDir;

            if (_iBarcodeReaderSupport == 1)
                sRet += " -b";

            if (_iFullscreen == 1)
                sRet += " -f ";

            sRet += " -a " + _iBPP.ToString();

            return sRet;
        }

        public string getRDPstring()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            string sLine = rdpLines[0]; 
            while (sLine != null)
            {
                if (sLine.StartsWith("WorkingDir"))
                {
                        sb.Append(String.Format(sLine, _sWorkingDir));
                }
                else if (sLine.StartsWith("AlternateShell"))
                {
                        sb.Append(String.Format(sLine, _sShell));
                }
                else if (sLine.StartsWith("EnableClipboardRedirection"))
                {
                        sb.Append(String.Format(sLine, _iClipboard));
                }
                else if (sLine.StartsWith("Domain"))
                {
                        sb.Append(String.Format(sLine, _sDomain));
                }
                else if (sLine.StartsWith("MCSPort"))
                {
                    sb.Append(String.Format(sLine, _iPort));
                }
                else if (sLine.StartsWith("Password"))
                {
                    if (_sPass.Length > 0)
                    {
                        #region TESTING
                        //string sPassEnc = rdp_password.EncryptRDPPassword(_sPass);
                        //here:         0200000000000000000000000000000000000000000000000800000072006400700000000E660000100000001000000031C03CFAF193C0C61AA5346F40DB08F800000000048000001000000010000000891E0B7D71DCAB69764E9CA05E25CEB6200000004EEEE6F38BD74AC8E4CC6C0661235AA1221B6BF14C191E27260FD7F99322E5C514000000F539F077730E2DBFDE6B368D9F45B67AAED4E4FF"
                        //rdp:          0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4
                        //rdp_decrypt:  0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4

                        ////using a DLL
                        //string sPassEnc = rdp_crypt.encrypt(_sPass);
                        //string sPassClear = rdp_crypt.decrypt(sPassEnc);
                        // gives        0200000000000000000000000000000000000000000000000800000072006400700000000e6600001000000010000000c7b7d85faf8e1ead57ad6698aef297ab00000000048000001000000010000000d78d3961ef6071a94af0732d75011c8720000000cff51a599a6e3794062c03e7459d0a97f4e29660e2183a63e1e2e7e9f304ae51140000002ef45194eea70f3133f05f8df7f5d1f5b5da267c
                        //rdp:          0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4

                        //string sPassEnc = RDPcrypt.CryptTest.RDPencrypt(_sPass);
                        string sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, _sPass, string.Empty, "psw");
                        //MachineKey: 0200000000000000000000000000000000000000040000000800000072006400700000000E6600001000000010000000ABD068CB6407C7B46789983CD8497ADF000000000480000010000000100000009D5510223ADAFF0D214797166ABF00E71000000044D9ABFDD92A841F09DF3461C67FD5231400000004CF33CE7C73F5D847851D8201D6028694F1FC51
                        //User   Key: 0200000000000000000000000000000000000000000000000800000072006400700000000E6600001000000010000000FB1308672F24BB2A3E2C9625977BC1CE00000000048000001000000010000000F60BBBB68E21ACE12D3A54C26BDC7686100000009963D2B109863E4345AD3F6C4BB1DA4314000000FC55E7196D64A5B8C90BA683CBC9775D95A86A97
                        //rdp    Key: 0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4
                        //test
                        sEncrypted = DPAPI.EncryptRDP(_sPass, "rdp");
                        System.Diagnostics.Debug.WriteLine("****rdp****\r\nNo KEY    : " + sEncrypted);
                        //sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, _sPass, null, "rdp");
                        //System.Diagnostics.Debug.WriteLine("User   Key: " + sEncrypted);

                        //sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.MachineKey, _sPass, null, "psw");
                        //System.Diagnostics.Debug.WriteLine("****psw****\r\nMachineKey: " + sEncrypted);
                        //sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, _sPass, null, "psw");
                        //System.Diagnostics.Debug.WriteLine("User   Key: " + sEncrypted);
                        System.Diagnostics.Debug.WriteLine("rdp    Key: " + "0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4");
                        //user key matches more!!!
                        #endregion
                        string description="";
                        string sTest = DPAPI.Decrypt(sEncrypted, string.Empty, out description);
                        if (sTest != _sPass)
                            sEncrypted = "Error while encrypting password";
                        sb.Append(String.Format(sLine, sEncrypted));
                    }
                    else
                        sb.Append(String.Format(sLine, ""));
                }
                else if (sLine.StartsWith("ServerName"))
                {
                    sb.Append(string.Format(sLine, _sHostname));
                }
                else if (sLine.StartsWith("UserName"))
                {
                    sb.Append(string.Format(sLine, _sUser));
                }
                else if (sLine.StartsWith("SavePassword"))
                {
                    sb.Append(string.Format(sLine, _iSavePassword));
                }
                else if (sLine.StartsWith("DesktopHeight"))
                {
                    sb.Append(string.Format(sLine, _iHeight.ToString()));
                }
                else if (sLine.StartsWith("DesktopWidth"))
                {
                    sb.Append(string.Format(sLine, _iWidth.ToString()));
                }
                else if (sLine.StartsWith("ScreenStyle"))
                {
                    if(_iFullscreen==1)
                        sb.Append(string.Format(sLine, "2"));
                    else
                        sb.Append(string.Format(sLine, "0"));
                }
                else if (sLine.StartsWith("ColorDepthID"))
                {
                    sb.Append(string.Format(sLine, _iBPP.ToString()));
                }
                else
                    sb.Append(String.Format(sLine, ""));

                i++;
                sLine = rdpLines[i];
            };
            return sb.ToString();
        }

        public bool writeFile(string sFile)
        {
            bool bRet = false;
            using (StreamWriter sw = new StreamWriter(sFile))
            {
                // Add some text to the file.
                sw.Write(this.getRDPstring());
                bRet = true;
            }
            return bRet;
        }
        string[] rdpLines = new string[]{
//	        "\xFF\xFE", //this is NOT used, it is the Unicode identifier
	        //new with 3. dec 2013
	        "WorkingDir:s:{0}\r\n",
	        "AlternateShell:s:{0}\r\n",	// NOT IMPLEMENTED IN RDM!
	        "EnableClipboardRedirection:i:{0}\r\n",	//0 = disabled, 1 = enable clipboard sharing, see also g_bEnableClipboardRedirection
	        "RDPIdleTimeout:i:0\r\n",				// NOT IMPLEMENTED IN RDM!
	        //older...
	        "Domain:s:{0}\r\n",
	        "GrabFocusOnConnect:i:0\r\n",
	        "MinutesToIdleTimeout:i:0\r\n", //was 5, has no function, tested with 0 but session did timeout anyway
	        "DisableFileAccess:i:0\r\n",
	        "BBarEnabled:i:0\r\n",
	        "EnablePrinterRedirection:i:0\r\n",
	        "EnableSCardRedirection:i:1\r\n",
	        "AutoReconnectEnabled:i:1\r\n",
	        "EnableDriveRedirection:i:0\r\n",	//EnableDriveRedirection=1, enables access to local files inside the host session
	        "EnablePortRedirection:i:0\r\n",
	        "AudioRedirectionMode:i:2\r\n",		//0=Redirect sounds to the client, 1=Play sounds at the remote computer, 2=Disable sound redirection; do not play sounds at the server
	        "BitmapPersistenceEnabled:i:0\r\n",
	        "BBarShowPinBtn:i:0\r\n",
	        "Compress:i:1\r\n",
	        "KeyboardHookMode:i:0\r\n",
	        "MaxReconnectAttempts:i:20\r\n",
	        "Disable Wallpaper:i:1\r\n",
	        "Disable Full Window Drag:i:1\r\n",
	        "Disable Menu Anims:i:1\r\n",
	        "Disable Themes:i:0\r\n",
	        "KeyboardLayoutString:s:0xE0010409\r\n",
	        "KeyboardType:i:4\r\n",
	        "KeyboardSubType:i:0\r\n",
	        "KeyboardFunctionKey:i:12\r\n",
	        "BitmapCacheSize:i:21\r\n",
	        "BitmapPersistCacheSize:i:1\r\n",
	        "Keyboard Layout:s:00000409\r\n",
	        "MCSPort:i:{0}\r\n", // 3389

	        "Password:b:{0}\r\n", // 0200000000000000000000000000000000000000000000000800000072006400700000000E660000100000001000000042CC2095244E1C87923AC5BC2014D0A6000000000480000010000000100000004207CC82911829FA8E78AE77404B256820000000D1759CC8A4025896DC5C7599484D7D0CCEF9C4BBBF5A44DC5D766B25A02E32001400000011F4A0E6AD9236475C1AD25AE113EE331D893929
	        "ServerName:s:{0}\r\n",
	        "UserName:s:{0}\r\n",
	        "SavePassword:i:{0}\r\n",
	        "DesktopHeight:i:{0}\r\n",
	        "DesktopWidth:i:{0}\r\n",
	        "ScreenStyle:i:{0}\r\n",  //0=no fullscreen + no fit, 1= fit to screen+no fullscreen, 2=fullscreen+no fit, 3=fit+fullscreen
	        "ColorDepthID:i:{0}\r\n", //changed from {0} to {0} with version 4
	        null
        };
    }
}
