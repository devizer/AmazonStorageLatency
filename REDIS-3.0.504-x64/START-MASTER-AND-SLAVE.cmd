setlocal
set dir=%~dp0
call %dir%\master\start.cmd
call %dir%\slave\start.cmd
