@echo off
::set /p userinputpath=Please enter your bci2000 root directory: 
::set bci2000path=%userinputpath%\prog

::set /p telnetIP=Please enter your Telnet IP:
::set /p telnetPort=Please enter your Telnet Port:

set bci2000path=C:\bci2000.x64_dev\prog
set telnetIP=10.0.0.100
set telnetPort=3999

start %bci2000path%\Operator.exe --Telnet "%telnetIP%:%telnetPort%" --StartupIdle