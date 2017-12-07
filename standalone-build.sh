#!/bin/bash
work=~/tmp/; mkdir -p $work; cd $work; rm -rf AmazonStorageLatency; git clone https://github.com/devizer/AmazonStorageLatency; cd AmazonStorageLatency
nuget restore
xbuild /t:Rebuild /p:Configuration=Release /verbosity:minimal
mono AmazonStorageLatency/bin/Release/AmazonS3Sample.exe 
