using System;

using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

namespace RDPman
{
    public class myxml
    {
        public static string SerializeToXmlString(object targetInstance)
        {
            string retVal = string.Empty;
            TextWriter writer = new StringWriter();
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(targetInstance.GetType());
            serializer.Serialize(writer, targetInstance, ns);
            retVal = writer.ToString();
            return retVal;
        }
        /// <summary>
        /// convert xml back to object
        /// </summary>
        /// <param name="objectXml">xml with object data</param>
        /// <param name="targetType">type of the object to create</param>
        /// <returns></returns>
        public static object DeserializeFromXmlString(string objectXml, Type targetType)
        {
            object retVal = null;
            XmlSerializer serializer = new XmlSerializer(targetType);
            StringReader stringReader = new StringReader(objectXml);
            XmlTextReader xmlReader = new XmlTextReader(stringReader);
            retVal = serializer.Deserialize(xmlReader);
            return retVal;
        }
    }

    [XmlRoot("rdesktopce")]
    public class rdp_settings
    {
        #region FIELDS
        //-g geometry
        [XmlElement]
        public int DesktopHeight = 240;
        [XmlElement]
        public int DesktopWidth = 320;

        //-t port
        [XmlElement]
        public int MCSPort = 3389;

        //-a bpp BitsPerPixel, 8 or 15 or 16
        [XmlElement]
        public int ColorDepthID = 16;

        //-f fullscreen
        [XmlElement]
        public int ScreenStyle = 0;

        //-u user name
        [XmlElement]
        public string UserName = "hgode";

        //-p password
        /// <summary>
        /// save the password encrypted
        /// use DPAPI.Decrypt to get clear password
        /// </summary>
        [XmlElement]
        public string Password {
            set{
                string sEnc=DPAPI.EncryptRDP(value,"rdp");
                _password = sEnc;
            }
            get
            {
                return _password;
            }
        }    

        [XmlIgnore]
        private string _password;
        [XmlIgnore]
        private string _passwordClearText
        {
            get
            {
                string s = DPAPI.Decrypt(_password);
                return s;
            }
        }

        //-d domain
        [XmlElement]
        public string Domain = "";

        //-s shell
        [XmlElement]
        public string AlternateShell = "";

        //-c directory
        [XmlElement]
        public string WorkingDir = "";

        //-n hostname
        [XmlElement]
        public string ServerName = "192.168.0.112";

        //-x clipboard, currently not supported
        [XmlElement]
        public int EnableClipboardRedirection = 0;

        //-b barcode reader support
        [XmlElement]
        public int usebarcodereader = 0;

        [XmlElement]
        public string rdesktopce = @"\Program Files\rdesktopce\rdesktopce.exe";

        [XmlElement]
        public int SavePassword = 1;
        #endregion
        public string getArgList()
        {
            string sRet = "";
            //-g
            sRet += " -g "+DesktopHeight.ToString()+"x"+DesktopWidth.ToString();
            sRet += " -n " + ServerName + " -t " + MCSPort.ToString();
            sRet += " -u " + UserName + " -p " + _passwordClearText;
            if (Domain.Length > 0)
                sRet += " -d " + Domain;
            if (AlternateShell.Length > 0)
                sRet += " -s " + AlternateShell;
            if (WorkingDir.Length > 0)
                sRet += " -c " + WorkingDir;

            if (usebarcodereader == 1)
                sRet += " -b";

            if (ScreenStyle == 1)
                sRet += " -f ";

            sRet += " -a " + ColorDepthID.ToString();

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
                        sb.Append(String.Format(sLine, WorkingDir));
                }
                else if (sLine.StartsWith("AlternateShell"))
                {
                        sb.Append(String.Format(sLine, AlternateShell));
                }
                else if (sLine.StartsWith("EnableClipboardRedirection"))
                {
                        sb.Append(String.Format(sLine, EnableClipboardRedirection));
                }
                else if (sLine.StartsWith("Domain"))
                {
                        sb.Append(String.Format(sLine, Domain));
                }
                else if (sLine.StartsWith("MCSPort"))
                {
                    sb.Append(String.Format(sLine, MCSPort));
                }
                else if (sLine.StartsWith("Password"))
                {
                    if (_passwordClearText.Length > 0)
                    {
                        #region TESTING
                        //string sPassEnc = rdp_password.EncryptRDPPassword(Password);
                        //here:         0200000000000000000000000000000000000000000000000800000072006400700000000E660000100000001000000031C03CFAF193C0C61AA5346F40DB08F800000000048000001000000010000000891E0B7D71DCAB69764E9CA05E25CEB6200000004EEEE6F38BD74AC8E4CC6C0661235AA1221B6BF14C191E27260FD7F99322E5C514000000F539F077730E2DBFDE6B368D9F45B67AAED4E4FF"
                        //rdp:          0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4
                        //rdp_decrypt:  0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4

                        ////using a DLL
                        //string sPassEnc = rdp_crypt.encrypt(Password);
                        //string sPassClear = rdp_crypt.decrypt(sPassEnc);
                        // gives        0200000000000000000000000000000000000000000000000800000072006400700000000e6600001000000010000000c7b7d85faf8e1ead57ad6698aef297ab00000000048000001000000010000000d78d3961ef6071a94af0732d75011c8720000000cff51a599a6e3794062c03e7459d0a97f4e29660e2183a63e1e2e7e9f304ae51140000002ef45194eea70f3133f05f8df7f5d1f5b5da267c
                        //rdp:          0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4

                        //string sPassEnc = RDPcrypt.CryptTest.RDPencrypt(Password);
                        string sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, _passwordClearText, string.Empty, "psw");
                        //MachineKey: 0200000000000000000000000000000000000000040000000800000072006400700000000E6600001000000010000000ABD068CB6407C7B46789983CD8497ADF000000000480000010000000100000009D5510223ADAFF0D214797166ABF00E71000000044D9ABFDD92A841F09DF3461C67FD5231400000004CF33CE7C73F5D847851D8201D6028694F1FC51
                        //User   Key: 0200000000000000000000000000000000000000000000000800000072006400700000000E6600001000000010000000FB1308672F24BB2A3E2C9625977BC1CE00000000048000001000000010000000F60BBBB68E21ACE12D3A54C26BDC7686100000009963D2B109863E4345AD3F6C4BB1DA4314000000FC55E7196D64A5B8C90BA683CBC9775D95A86A97
                        //rdp    Key: 0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4
                        //test
                        sEncrypted = DPAPI.EncryptRDP(_passwordClearText, "rdp");
                        System.Diagnostics.Debug.WriteLine("****rdp****\r\nNo KEY    : " + sEncrypted);
                        //sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, Password, null, "rdp");
                        //System.Diagnostics.Debug.WriteLine("User   Key: " + sEncrypted);

                        //sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.MachineKey, Password, null, "psw");
                        //System.Diagnostics.Debug.WriteLine("****psw****\r\nMachineKey: " + sEncrypted);
                        //sEncrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, Password, null, "psw");
                        //System.Diagnostics.Debug.WriteLine("User   Key: " + sEncrypted);
                        System.Diagnostics.Debug.WriteLine("rdp    Key: " + "0200000000000000000000000000000000000000000000000800000072006400700000000E66000010000000100000001E392BB875946E7281F1D962E2CBA05900000000048000001000000010000000BEBDA63E164615F22450CEC59F37D46B200000003BC01FF8CF7EC256730228E21FA4434597E6FF98EBC66B4B1D96EA4E76F7C6AA14000000BCA27291CE4AD6B9B1C3BD3CE397D53647213EC4");
                        //user key matches more!!!
                        #endregion
                        string description="";
                        string sTest = DPAPI.Decrypt(sEncrypted, string.Empty, out description);
                        if (sTest != _passwordClearText)
                            sEncrypted = "Error while encrypting password";
                        sb.Append(String.Format(sLine, Password));
                    }
                    else
                        sb.Append(String.Format(sLine, ""));
                }
                else if (sLine.StartsWith("ServerName"))
                {
                    sb.Append(string.Format(sLine, ServerName));
                }
                else if (sLine.StartsWith("UserName"))
                {
                    sb.Append(string.Format(sLine, UserName));
                }
                else if (sLine.StartsWith("SavePassword"))
                {
                    sb.Append(string.Format(sLine, SavePassword));
                }
                else if (sLine.StartsWith("DesktopHeight"))
                {
                    sb.Append(string.Format(sLine, DesktopWidth.ToString()));
                }
                else if (sLine.StartsWith("DesktopWidth"))
                {
                    sb.Append(string.Format(sLine, DesktopHeight.ToString()));
                }
                else if (sLine.StartsWith("ScreenStyle"))
                {
                    if(ScreenStyle==1)
                        sb.Append(string.Format(sLine, "2"));
                    else
                        sb.Append(string.Format(sLine, "0"));
                }
                else if (sLine.StartsWith("ColorDepthID"))
                {
                    sb.Append(string.Format(sLine, ColorDepthID.ToString()));
                }
                else if (sLine.StartsWith("rdesktopce")){
                    sb.Append(string.Format(sLine, rdesktopce));
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

        public bool writeXML(string sFile)
        {
            bool bRet = false;
            string sXml;
            sXml = myxml.SerializeToXmlString(this);
            using (StreamWriter sw = new StreamWriter(sFile + ".xml"))
            {
                sw.WriteLine(sXml);
                bRet = true;
            }
            return bRet;
        }

        public static rdp_settings loadSettings(string sXmlFile)
        {
            //bool bRet = false;
            //var fields = typeof(rdp_settings).GetFields();
            //foreach (var f in fields)
            //    System.Diagnostics.Debug.WriteLine(f.Name +": "+ f.GetValue(this));
            rdp_settings sett = null;
            try
            {                
                using (StreamReader sr = new StreamReader(sXmlFile))
                {
                    string line = "";
                    line = sr.ReadToEnd();
                    sett = (rdp_settings) myxml.DeserializeFromXmlString(line, typeof(rdp_settings));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("The xml file could not be read:");
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return sett;
        }
        #region OBSOLETE
        enum rdpEntryType
        {
            s,
            b,
            i
        }
        class valueList{
            string sNameRDP;
            rdpEntryType t;
            public valueList(string _s, rdpEntryType _t){
                sNameRDP=_s;
                t=_t;
            }
            public override string ToString()
            {
                return sNameRDP;
            }
        };
        valueList[] settings_table= new valueList[]{
            new valueList("WorkingDir" ,rdpEntryType.s),
            new valueList("AlternateShell" ,rdpEntryType.s),
            new valueList("EnableClipboardRedirection" ,rdpEntryType.i),
            new valueList("RDPIdleTimeout" ,rdpEntryType.i),
            new valueList("Domain" ,rdpEntryType.s),
            new valueList("MCSPort" ,rdpEntryType.i),
            new valueList("Password" ,rdpEntryType.b),
            new valueList("ServerName" ,rdpEntryType.s),
            new valueList("UserName" ,rdpEntryType.s),
            new valueList("SavePassword" ,rdpEntryType.i),
            new valueList("DesktopHeight" ,rdpEntryType.i),
            new valueList("DesktopWidth" ,rdpEntryType.i),
            new valueList("ScreenStyle" ,rdpEntryType.i),
            new valueList("ColorDepthID" ,rdpEntryType.i),
            new valueList("rdesktopce" ,rdpEntryType.s),
            new valueList("usebarcodereader" ,rdpEntryType.s),
        };
        #endregion

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
            "rdesktopce:s:{0}\r\n",
	        null
        };
    }
}
