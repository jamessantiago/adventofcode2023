#! /bin/bash

gcc -Wall $1.c -o prog.bin
ts=$(date +%s%N)
./prog.bin
echo Execution time: $((($(date +%s%N) - $ts)/1000000))ms
rm prog.bin
