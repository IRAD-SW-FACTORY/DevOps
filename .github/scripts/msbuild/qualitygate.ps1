$SONAR_URL = if ($env:SONAR_URL-ne $null) { $env:SONAR_URL } else { "http://ec2-54-164-26-4.compute-1.amazonaws.com" };
$REPO_NAME = if ($env:REPO_NAME -ne $null) { $env:REPO_NAME } else { "DevOps" };
$SONAR_TOKEN = if ($env:SONAR_TOKEN -ne $null) { $env:SONAR_TOKEN } else { "-" };
Write-Output "Starting sonarqube quality gate status of ${REPO_NAME} on instance ${SONAR_URL}"
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization", "Basic ${SONAR_TOKEN}")
$response = Invoke-RestMethod "${SONAR_URL}/api/qualitygates/project_status?projectKey=${REPO_NAME}" -Method 'GET' -Headers $headers
$result = $response | ConvertTo-Json | ConvertFrom-Json
if ($result.projectStatus.status -eq "ERROR") { 
    Write-Output $result
    [Environment]::Exit(1)
} 
