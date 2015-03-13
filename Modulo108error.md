#How to avoid modulu 0x108 error

# rdesktopce only supports RDP4 #

If you get a modulu 0x108 error when you try to connect to your terminal server ensure that the server is set to use a client compatible protocol.

# Details #

rdesktopce does not support RDP5 nor RDC6 protocol. Please setup your terminal server to let the client define which protocol to use.
I have tested rdesktopce successfully against Windows 2003 server's Terminal Server.

Update 1. march 2013
The module 0x108 error has been resolved in [r51](https://code.google.com/p/rdesktop-ce/source/detail?r=51) with the help of bradh!