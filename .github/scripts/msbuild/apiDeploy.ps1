Write-Output "Deploy started using API"
$destinationServerBaseUrl = if ($null -ne $env:SERVER) { "http://${env:SERVER}" } else { "http://ec2-54-152-53-114.compute-1.amazonaws.com" }
$releasesServerBaseUrl = if ($null -ne $env:SERVER) { "http://${env:SERVER}" } else { "http://ec2-54-152-53-114.compute-1.amazonaws.com" }
$runId = if ($null -ne $env:RUN_ID) { $env:RUN_ID } else { "6439638128" }
$kind = if ($null -ne $env:KIND) { if ($env:KIND -eq "WinService") { 1 } else { 0 } } else { 0 }
$releaseType = if ($kind -eq 0) { "Web" } else { "Service" } 
$name = if ($null -ne $env:APP_NAME) { $env:APP_NAME } else { "MultiRisWeb" }
$serviceName = if ($null -ne $env:SERVICE_NAME) { $env:SERVICE_NAME } else { "" }
$user = if ($null -ne $env:DESTINATION_USER) { $env:DESTINATION_USER } else { "devops" };
$pass = if ($null -ne $env:DESTINATION_PWD) { $env:DESTINATION_PWD } else { "Oh$&OGfK@-2K=9diZF8Cd!k0WK.ZU)Bl" }
if ($kind -eq 0) { 
    $localPath = if ($null -ne $env:DESTINATION_LOCAL_PATH) { $env:DESTINATION_LOCAL_PATH } else { "c:\\Sitios" }
}
else
{
    $localPath = if ($null -ne $env:DESTINATION_LOCAL_PATH) { $env:DESTINATION_LOCAL_PATH } else { "c:\\Servicios" }
}

$authToken = [Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes("${user}:${pass}"))
$releasedPackage = "${name}_${runId}.zip"
$backupsPath =  "${localPath}\\Backups"
$apiUrl = "$destinationServerBaseUrl/DeployApi/api/Deploy"
$releaseUrl = "$releasesServerBaseUrl/${releaseType}Releases/$releasedPackage"

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Accept", "application/json")
$headers.Add("Content-Type", "application/json")
$headers.Add("Authorization", "Basic $authToken")

$body = @"
{
    "kind": $kind,
    "name": "$name",
    "serviceName":  "$serviceName",
    "deploymentUrl": "$releaseUrl",
    "deploymentLocalPath": "$localPath",
    "backupLocalPath": "$backupsPath",
    "asyncDeploy": false
}
"@

$response = Invoke-RestMethod $apiUrl -Method 'POST' -Headers $headers -Body $body
$result = $response | ConvertTo-Json | ConvertFrom-Json
Write-Output $result
if ($result.deploymentStatus -eq 2) { 
    $env:EXIT_CODE = 0;
} 
else {
    $env:EXIT_CODE = 1;
    [Environment]::Exit(1)
}
[Environment]::Exit(0)