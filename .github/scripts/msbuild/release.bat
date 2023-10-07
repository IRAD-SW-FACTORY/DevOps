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
powershell Compress-Archive Deploy Release.zip

net use X: %DESTINATION_PATH% "%DESTINATION_PWD%" /User:%DESTINATION_USER%
xcopy Release.zip "X:\Releases\%APP_NAME%_%RUN_ID%.zip"  /Y /E
net use X: /d /Y
