@echo off || echo 
git config credential.helper store

echo.
echo ***************** PULL ********************
if "%1" == "delete" goto skip_recovery
git pull

:skip_recovery
echo.
echo ********** ADD --all and COMPPIT **********
git add --all
git commit -am "update"

echo.
echo **************** PUSH *********************
git push

echo.
echo *************** STATUS ********************
git status
