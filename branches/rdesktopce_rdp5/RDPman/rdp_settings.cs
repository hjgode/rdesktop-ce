using System;

using System.Collections.Generic;
using System.Text;

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
        public string _sUser = "";

        //-p password
        public string _sPass = "";

        //-d domain
        public string _sDomain = "";

        //-s shell
        public string _sShell = "";

        //-c directory
        public string _sWorkingDir = "";

        //-n hostname
        public string _sHostname = "";

        //-x clipboard, currently not supported
        public int _iClipboard = 0;

        //-b barcode reader support
        public int _iBarcodeReaderSupport = 0;

    }
}
