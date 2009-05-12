#!/bin/sh
mono Tools/NAnt/NAnt.exe package -buildfile:reflector.build -D:CCNetLabel=1.1.0.0 -nologo -logfile:nant-build-package.log
