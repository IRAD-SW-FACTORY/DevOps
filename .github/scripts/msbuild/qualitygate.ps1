Write-Output "Starting sonarqube quality gate status check"
$SONAR_URL = $env:SONAR_URL;
$REPO_NAME = $env:REPO_NAME;
$SONAR_TOKEN = $env:SONAR_TOKEN;
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization", "Basic ${SONAR_TOKEN}")
$response = Invoke-RestMethod "${SONAR_URL}api/qualitygates/project_status?projectKey=${REPO_NAME}" -Method 'GET' -Headers $headers
$result = $response | ConvertTo-Json | ConvertFrom-Json
Write-Output $result.projectStatus.status
if ($result.projectStatus.status -eq "ERROR") { 
    [Environment]::Exit(1)
}
Write-Output "Sonarqube quality gate status check finished $result"