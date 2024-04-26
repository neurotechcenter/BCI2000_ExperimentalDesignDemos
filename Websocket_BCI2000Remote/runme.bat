set /p userinputpath=Please enter your bci2000 root directory: 
set bci2000path=%userinputpath%\prog

set launchpath=%cd%

C:
cd %bci2000path%

start Operator.exe --WebSocket

start %launchpath%\changeColor.html