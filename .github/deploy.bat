 mkdir Deploy
cd ${{ inputs.projectPath }} 
msbuild /p:DeployOnBuild=true /p:PublishProfile=DeployFolder /p:Configuration=Release /p:Platform="Any CPU"
echo "${{ github.repository }}" > %VERSION_FILE%
echo "${{ github.ref }}.${{ github.job }}.${{ github.run_id }}" >> %VERSION_FILE%
net use X: %DESTINATION_PATH% "%DESTINATION_PWD%" /User:%DESTINATION_USER%
xcopy Deploy\* X:\%APP_NAME%_%RUN_ID%\* /Y /E
X:
move /Y X:\%APP_NAME% X:\Backups\%APP_NAME%_%RUN_ID%
rename X:\%APP_NAME%_%RUN_ID% %APP_NAME%
c:
net use X: /d /Y