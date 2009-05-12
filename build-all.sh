#!/bin/sh
mono Tools/NAnt/NAnt.exe -buildfile:reflector.build -D:codemetrics.output.type=HtmlFile -nologo -logfile:nant-build-all.log
