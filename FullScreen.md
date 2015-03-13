# Running rdesktop-ce in fullscreen mode #

![https://rdesktop-ce.googlecode.com/svn/wiki/ScreenShotFullscreen.gif](https://rdesktop-ce.googlecode.com/svn/wiki/ScreenShotFullscreen.gif)

Per default rdesktopce runs in a window without title bar and a simple menu bar with an option to exit rdesktopce. If fullscreen is enabled (fullscreen enty is in winrdesktop.Ini, see WinRdesktopIni) or the program is started with option '-f' (see CommandLine), the program will not display a menu bar at all. The whole screen of the windows mobile device is then occupied by the remote desktop / terminal server window.

If the geometry setting of rdesktopce matches the screen size of the windows mobile device, there will also be no scrollbars and the user does not need to scroll in the remote window.

To exit rdesktopce running in fullscreen mode, you have to doubleclick in the middle of the upper area inside an imaginary title bar. You will then be asked if you like to end rdesktopce and disconnect your remote session.