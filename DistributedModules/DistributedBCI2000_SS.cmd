@echo off
echo Start Signal Processing Module

set /p userinputpath=Please enter your bci2000 root directory: 
set bci2000path=%userinputpath%\prog

set /p operatorIP=Please enter the IP address of the machine running the Operator:

::because BCI2000 is stored in the C: drive (only works for drives with one letter)
set driveletter=%bci2000path:~0,2%
%driveletter% 

cd %bci2000path%

start SignalGenerator %operatorIP%