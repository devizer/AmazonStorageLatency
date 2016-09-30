setlocal
set dir=%~dp0
pushd %dir%
start /min "SLAVE Redis" ..\redis-server redis.conf
popd