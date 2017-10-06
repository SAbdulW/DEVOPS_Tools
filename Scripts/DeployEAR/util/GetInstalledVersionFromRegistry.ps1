param($vmIP, $jsonPath)
. $PSScriptRoot\PSConstant.ps1
. $PSScriptRoot\PSCommonLibs.ps1

if ($error) { $error.clear() }
$result = $false
$psLibs = New-Object CommonLibs
$psCredential = $psLibs.createRemoteCredential($vmIP, $vmUser, $vmPwd)
$fileName = $jsonPath.Split('\') | Select-Object -Last 1
$remoteFolder = '\\' + $vmIP + '\C$\Impact360'
if ($psLibs.disconnectNetworkMap($remoteFolder)) {
  New-PSDrive -Name X -PSProvider FileSystem -Root $remoteFolder -Credential $psCredential
}
Copy-Item -Path $jsonPath -Destination 'X:\' -Force
Write-Host "[GetInstalledVersionFromRegistry] - KB list json file copied!"
$sbContent = {
  param($jsonName)
  $jsonFile = "C:\Impact360\$jsonName"
  $jsonData = [System.IO.File]::ReadAllText($jsonFile) | ConvertFrom-Json
  $regkey64 = 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall'
  $regkey32 = 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall'
  $foundKey = $true
  ForEach ($item in $jsonData) {

    Write-Host "[GetInstalledVersionFromRegistry] - Searching Version for:"$item.componentID
    $key = Get-ChildItem $regkey64 -Recurse | ForEach-Object { Get-ItemProperty $_.PSPath } | Where-Object {$_.DisplayName -eq $item.componentID}

    if (!($key)) {
        Write-Host "[GetInstalledVersionFromRegistry] - Not found"$itemArtifact.artifactId"in 64bit keys. Now Searching on 32bit keys..."
        $key = Get-ChildItem $regkey32 -Recurse | ForEach-Object { Get-ItemProperty $_.PSPath } | Where-Object {($_.DisplayName -match $item.componentID)}
    }

    if (!($key)) {
        Write-Host "[GetInstalledVersionFromRegistry] - Not found"$item.componentID"in registry!"
        $foundKey = $false
        break
    }

    $ComponentVersion = $key.DisplayVersion

    break

  }
  Write-Host "[GetInstalledVersionFromRegistry] - Checking result on $vmIP : $foundKey"
  $jsonObj = New-Object -TypeName PSObject
  Add-Member -InputObject $jsonObj -NotePropertyName 'ComponentVersion' -NotePropertyValue $ComponentVersion.toString()
  Add-Member -InputObject $jsonObj -NotePropertyName 'Result' -NotePropertyValue $foundKey.toString()
  $jsonData = ConvertTo-Json -InputObject $jsonObj
  Set-Content -Path $jsonFile -Value $jsonData
}
if ($psLibs.addTrustedIP($vmIP)) { Get-Item WSMan:\localhost\Client\TrustedHosts }
if ($psLibs.ensureRemoteCall($vmIP, $vmUser, $vmPwd)) {
  Write-Host "[GetInstalledVersionFromRegistry] - Verify KB list from test machine registry ..."
  Invoke-Command -Computer $vmIP -ScriptBlock $sbContent -ArgumentList $fileName -Credential $psCredential
}
if (-Not $psLibs.removeTrustedIP($vmIP)) {Clear-Item WSMan:\localhost\Client\TrustedHosts -Force}
Move-Item -Path "X:\$fileName" -Destination $jsonPath -Force
Remove-PSDrive -Name X -Force

if ($error) {Write-Host "ERROR: $error"}