$SONAR_URL = if ($null -ne $env:SONAR_URL) { $env:SONAR_URL } else { "http://ec2-54-164-26-4.compute-1.amazonaws.com/" };
$REPO_NAME = if ($null -ne $env:REPO_NAME) { $env:REPO_NAME } else { "DevOps" };
$SONAR_TOKEN = if ($null -ne $env:SONAR_TOKEN) { $env:SONAR_TOKEN } else { "c3FhX2Y1MDVhYTA4YTRhNmJmNzI0MDI0YThmZDBkNTBhN2RiN2RkYWQyNzA6" };
$URL = "${SONAR_URL}api/qualitygates/project_status?projectKey=${REPO_NAME}"
Write-Output "Getting sonarqube quality gate status from ${URL}"
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization", "Basic ${SONAR_TOKEN}")
$response = Invoke-RestMethod $URL -Method "GET" -Headers $headers
$result = $response | ConvertTo-Json | ConvertFrom-Json
Write-Output $result.projectStatus
if ($result.projectStatus.status -eq "OK") { 
    $env:EXIT_CODE = 0;
} 
else {
    $env:EXIT_CODE = 1;
    #[Environment]::Exit(1)
}
#[Environment]::Exit(0)