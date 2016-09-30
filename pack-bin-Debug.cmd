mkdir .\Releases 1>nul 2>nul
del .\Releases\AmazonStorageLatency-bin-Debug.rar
pushd AmazonStorageLatency\bin\Debug
winrar a ..\..\..\Releases\AmazonStorageLatency-bin-Debug.rar . -r -m5 -s -ep1 -apAmazonStorageLatency -x*.xml
popd
