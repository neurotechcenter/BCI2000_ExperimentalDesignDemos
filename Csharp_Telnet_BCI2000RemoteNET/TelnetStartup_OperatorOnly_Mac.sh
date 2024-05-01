#!/bin/bash

echo "Please enter your bci2000 root directory: "
read -r userinputpath
bci2000path="$userinputpath/prog"

echo "Please enter your Telnet IP:"
read -r telnetIP

echo "Please enter your Telnet Port:"
read -r telnetPort

"$bci2000path/Operator.app/Contents/MacOS/Operator" --Telnet "$telnetIP:$telnetPort" --StartupIdle &