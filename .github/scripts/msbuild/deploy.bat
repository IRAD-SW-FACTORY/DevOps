echo "Running deploy.bat for repo %REPO_NAME%"
echo "Publishing of %APP_NAME% started"
mkdir Deploy
cd %PROJECT_PATH%%
IF "%KIND%" == "WinService" (
    msbuild  /t:restore /p:RestorePackagesConfig=true /p:Configuration=%BUILD_CONFIG%
    xcopy bin\Release\* ..\Deploy\* /Y /E
)
IF "%KIND%" == "AppWeb" (
  msbuild /t:restore /t:rebuild /p:RestorePackagesConfig=true /p:Configuration=%BUILD_CONFIG% /p:DeployOnBuild=true /p:PublishProfile=DeployFolder
)
if "%PROJECT_PATH%" NEQ "." cd ..

echo "%APP_NAME%" > %VERSION_FILE%
echo "%VERSION%" >> %VERSION_FILE%
net use X: %DESTINATION_PATH% "%DESTINATION_PWD%" /User:%DESTINATION_USER%

xcopy Deploy\* "X:\%APP_NAME%_%RUN_ID%\*" /Y /E
call .github\scripts\stop.bat
X:
xcopy "X:\%APP_NAME%\*" "X:\Backups\%APP_NAME%_%RUN_ID%\*"  /Y /E
xcopy "X:\%APP_NAME%_%RUN_ID%\*" "%APP_NAME%\*"  /Y /E
rmdir "X:\%APP_NAME%_%RUN_ID%" /Q/S
c:
net use X: /d /Y
call .github\scripts\start.bat
