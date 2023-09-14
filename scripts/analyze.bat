 cd %PROJECT_PATH%
SonarScanner.MSBuild.exe begin /k:"%REPO_NAME%" /d:sonar.login="%SONAR_TOKEN%" /d:sonar.host.url="%SONAR_URL%"
msbuild /t:restore /p:RestorePackagesConfig=true /p:Configuration=%BUILD_CONFIG% /t:rebuild
@if "%TESTS_PATH%" == "-" (echo "No tests") else vstest.console %TESTS_PATH%
SonarScanner.MSBuild.exe end /d:sonar.login="%SONAR_TOKEN%"