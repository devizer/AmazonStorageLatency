#!/bin/bash
nuget restore
xbuild /t:Rebuild /p:Configuration=Release /verbosity:minimal
mono AmazonStorageLatency/bin/Release/AmazonS3Sample.exe 
