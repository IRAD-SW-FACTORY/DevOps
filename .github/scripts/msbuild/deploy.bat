echo "Running deploy.bat for repo %REPO_NAME%"
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
