@echo off
rem set SONAR_TOKEN="c3FhX2Y1MDVhYTA4YTRhNmJmNzI0MDI0YThmZDBkNTBhN2RiN2RkYWQyNzA6"
rem set SONAR_URL="http://ec2-54-164-26-4.compute-1.amazonaws.com/"
rem set REPO_NAME="DevOps"

curl -s --header "Authorization: Basic %SONAR_TOKEN%" --location "%SONAR_URL%api/qualitygates/project_status?projectKey=%REPO_NAME%" > output.json
type output.json | jq .projectStatus.status > status.txt
set /p Status=<status.txt
echo Quality gate status: %Status%
if %Status% == "OK" exit /b 0
exit /b 1