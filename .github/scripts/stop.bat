echo "Stop App Pool or Service %APP_NAME%"
IF "%KIND%" == "WinService" GOTO :service 
IF "%KIND%" == "AppWeb" GOTO :website 
goto:eof

:service
PsExec64.exe -nobanner -accepteula \\%SERVER% -s -u %DESTINATION_USER% -p "%DESTINATION_PWD%" "C:\Windows\System32\sc.exe" stop "%SERVICE_NAME%"
goto:eof

:website
PsExec64.exe -nobanner -accepteula \\%SERVER% -s -u %DESTINATION_USER% -p "%DESTINATION_PWD%" "C:\Windows\System32\inetsrv\appcmd.exe" stop apppool /apppool.name:"%APP_NAME%"
