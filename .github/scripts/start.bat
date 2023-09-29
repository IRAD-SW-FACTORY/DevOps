echo "Start App Pool or Service %APP_NAME%"

Call :strlen %REPO_NAME% _length
SET KIND=%REPO_NAME%:~_length-6,6"
echo %KIND%
echo String is %_length% characters long


IF %KIND% == "WinService" GOTO service 
IF %KIND% == "AppWeb" GOTO website 

goto:eof

:service
PsExec64.exe -s -nobanner -accepteula \\%SERVER% -u %DESTINATION_USER% -p "%DESTINATION_PWD%" sc start "%APP_NAME%"
goto:eof

:website
PsExec64.exe -s -nobanner -accepteula \\%SERVER% -u %DESTINATION_USER% -p "%DESTINATION_PWD%" "C:\Windows\System32\inetsrv\appcmd.exe" "start apppool /apppool.name:%APP_NAME%"

goto:eof
:strlen  StrVar  [RtnVar]
  setlocal EnableDelayedExpansion
  set "s=#!%~1!"
  set "len=0"
  for %%N in (4096 2048 1024 512 256 128 64 32 16 8 4 2 1) do (
    if "!s:~%%N,1!" neq "" (
      set /a "len+=%%N"
      set "s=!s:~%%N!"
    )
  )
  endlocal&if "%~2" neq "" (set %~2=%len%) else echo %len%
exit /b