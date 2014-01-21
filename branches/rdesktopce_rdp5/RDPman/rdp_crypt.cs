using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RDPman
{
    class rdp_crypt
    {
        [DllImport("rdp_crypt.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int decryptPassword(
             StringBuilder szPassEncryptedHEX, 
             StringBuilder szPassDecrypted, 
            ref int dwSizeOut);

        [DllImport("rdp_crypt.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int encryptPassword(
             StringBuilder szPass,
             StringBuilder szReturn,
                ref int dSize);

        public static string decrypt(string szPassHex)
        {
            string s = "";
            StringBuilder sbPassHex = new StringBuilder(szPassHex);
            int iLen = 1024;
            StringBuilder sbPassClear = new StringBuilder(1024);
            int iRes = decryptPassword( sbPassHex,  sbPassClear, ref iLen);
            if (iRes == 0)
                s = sbPassClear.ToString();
            return s;
        }
        public static string encrypt(string szPassClear)
        {
            string s = "";
            StringBuilder sbPassClear = new StringBuilder(szPassClear);
            StringBuilder sbPassHEX = new StringBuilder(1024);
            int iRes = 0;
            int iSize = 1024;
            iRes = encryptPassword( sbPassClear,  sbPassHEX, ref iSize);
            if (iRes == 0)
                s = sbPassHEX.ToString();
            return s;
        }
    }
}
