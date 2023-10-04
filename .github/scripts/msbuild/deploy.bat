echo "Running deploy.bat for repo %REPO_NAME%"
echo "Publishing of %APP_NAME% started"
mkdir Deploy
cd %PROJECT_PATH%%
IF "%KIND%" == "WinService" (
    msbuild  /t:restore /p:RestorePackagesConfig=true /p:Configuration=Release /p:Platform="Any CPU"
    xcopy bin\Release\* ..\Deploy\* /Y /E
)
IF "%KIND%" == "AppWeb" (
  msbuild /t:restore /p:RestorePackagesConfig=true /p:DeployOnBuild=true /p:PublishProfile=DeployFolder /p:Configuration=Release /p:Platform="Any CPU"
)
if "%PROJECT_PATH%" NEQ "." cd ..

echo "%APP_NAME%" > %VERSION_FILE%
echo "%VERSION%" >> %VERSION_FILE%
net use X: %DESTINATION_PATH% "%DESTINATION_PWD%" /User:%DESTINATION_USER%

xcopy Deploy\* "X:\%APP_NAME%_%RUN_ID%\*" /Y /E
call .github\scripts\stop.bat
X:
move /Y "X:\%APP_NAME%" "X:\Backups\%APP_NAME%_%RUN_ID%"
rename "X:\%APP_NAME%_%RUN_ID%" "%APP_NAME%"
c:
net use X: /d /Y
call .github\scripts\start.bat
