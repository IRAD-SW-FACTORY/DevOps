cd %PROJECT_PATH%
msbuild /t:restore /p:RestorePackagesConfig=true /p:Configuration=%BUILD_CONFIG% /t:rebuild
@if "%TESTS_PATH%" == "-" (echo "No tests") else vstest.console %TESTS_PATH%