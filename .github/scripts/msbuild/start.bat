echo "Start App Pool or Service %APP_NAME%"
PsExec64.exe -s -nobanner -accepteula \\%SERVER% -u %DESTINATION_USER% -p "%DESTINATION_PWD%" "cmd /c C:\Windows\System32\inetsrv\appcmd.exe" "start apppool /apppool.name:%APP_NAME%"
