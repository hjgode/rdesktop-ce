![https://rdesktop-ce.googlecode.com/svn/wiki/ScreenShot01.gif](https://rdesktop-ce.googlecode.com/svn/wiki/ScreenShot01.gif)

# The ini file explained #

You can start a terminal server connection by starting winrdesktop.exe with a number of arguments (CommandLine) or by providing an ini file with the connection settings.


# The ini file #

Here is a sample ini file. You can either place the ini file in the root of the windows ce or windows mobile device or in the directory where winrdesktop.exe is stored.

`[`main`]`

server=192.168.128.5

port=3389

username=yourysername

password=yourpassword

bpp=16

geometry=1024x768

`#`fullscreen

## server ##
provide the server IP or DNS host name
## port ##
provide the port to use for RDP, usually 3389
## username ##
provide the login user name for the terminal client session
## password ##
provide the password of the user
## bpp ##
define the bits per pixel to be used (number of colors per pixel)
## geometry ##
define the size of the remote desktop window
width x height
An application to be used on windows mobile devices should match the screen size of the device. For example 240x320 (QVGA). Otherwise the user has to scroll the window to get access to all of the remote screen.
## fullscreen ##
supported since [r16](https://code.google.com/p/rdesktop-ce/source/detail?r=16), see FullScreen