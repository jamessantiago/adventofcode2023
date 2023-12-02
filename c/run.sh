#! /bin/bash

gcc -Wall $1.c -o prog.bin; time ./prog.bin
rm prog.bin
