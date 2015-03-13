# command line options #



winrdesktop `[`-g widthxheight`]` `[`-t port`]` `[`-a bpp`]`

> `[`-f`]` `[`-u username`]` `[`-p password`]` `[`-d domain`]`

> `[`-s shell`]` `[`-c working directory`]` `[`-n host name`]`

> server-name-or-ip

Example:

winrdesktop -g 240x320 -a 16 -f -u rdesktop -p password 192.168.128.5

Details:

-g 240x320  :starts a remote session within a 240x320 window

-t 3389     :specify 3389 as port to terminal server host

-a 16       :use 16 bit colors per pixel

-f          :run rdesktopce in fullscreen mode without title and menu bar (see FullScreen)

-u username :use username to logon at terminal server

-p password :use password to authenticate username at terminal server

-d domain   :use domain for user authentication

-s shell    :define the remote shell to use

-c dir      :use dir as remote working dir

-n hostname :use hostname at terminal server side (viewablle i terminal server management)

servername-or-ip  :the terminal server's or remote desktop PC's IP address or host name

see also WinRdesktopIni