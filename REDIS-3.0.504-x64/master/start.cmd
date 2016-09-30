setlocal
set dir=%~dp0
pushd %dir%
start /min "MASTER Redis" ..\redis-server redis.conf
popd
