@echo off
set /p userinputpath=Please enter your bci2000 root directory: 
set bci2000path=%userinputpath%\prog

set /p telnetIP=Please enter your Telnet IP:
set /p telnetPort=Please enter your Telnet Port:

start %bci2000path%\Operator.exe --Telnet "%telnetIP%:%telnetPort%" --StartupIdle