A long time ago a guy ported rdesktop, the free opensource linux implementation of the Remote Desktop Protocol to WinCE. Unfortunately the rdesktop team did not create a new fork for this port [Original Post by Jay Sorg](http://osdir.com/ml/network.rdesktop.user/2006-04/msg00005.html). So I decided to create this opensource project and contribute my code.

![https://rdesktop-ce.googlecode.com/svn/wiki/ScreenShot01.gif](https://rdesktop-ce.googlecode.com/svn/wiki/ScreenShot01.gif)

It is in experimental phase but works OK for normal use (no audio, drive sharing and clipboard support).

In contrast to the Windows Mobile provided Terminal Service Client it supports automated connections without user intervention. A second advantage of the code is, that it does not automatically disconnect after 10Minutes oc user inactivity as the MS code does.

Hopefully some is able to add clipboard support to the code. I already implemented a barcode reader interface, so that barcodes are received by an barcode reader and transfered to the server.

[r60](https://code.google.com/p/rdesktop-ce/source/detail?r=60) adds support for SSL/TLS. Many thanks to Adam B., Trent M. and Brad H.

Binaries can be found at the [branches dir](http://code.google.com/p/rdesktop-ce/source/browse/#svn%2Fbranches%2Frdesktopce_rdp5%2Fsource%2FWindows%20Mobile%206%20Professional%20SDK%20%28ARMV4I%29%2FRelease)

To run rdesktopce (rdp5 branch) you need a binary, a winrdesktop.ini and the runtimes libeay32.dll and ssleay32.dll.