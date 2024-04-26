set launchpath=%cd%
set bci2000path=C:\bci2000.x64\prog

C:
cd %bci2000path%

start Operator.exe --WebSocket

start %launchpath%\changeColor.html