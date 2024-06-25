@echo off
echo Start Operator, Signal Source, and Application Modules

::set bci2000path=C:\bci2000.x64_dev\prog
::set operatorIP=192.168.42.128

set /p userinputpath=Please enter your bci2000 root directory: 
set bci2000path=%userinputpath%\prog

set /p operatorIP=Please enter the IP address of the machine running the Operator:

::because BCI2000 is stored in the C: drive (only works for drives with one letter)
set driveletter=%bci2000path:~0,2%
%driveletter% 

cd %bci2000path%

start Operator --Startup %operatorIP%
start SignalGenerator %operatorIP%
start DummySignalProcessing %operatorIP%