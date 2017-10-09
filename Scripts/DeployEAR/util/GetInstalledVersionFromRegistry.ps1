param($vmIP, $vmUser="ap2admin", $vmPwd="verint1!", $componentName, $jsonOutFileName)
. $PSScriptRoot\PSConstant.ps1
. $PSScriptRoot\PSCommonLibs.ps1

if ($error) { $error.clear() }
$resultVersion = ''
$psLibs = New-Object CommonLibs
$psCredential = $psLibs.createRemoteCredential($vmIP, $vmUser, $vmPwd)

Write-Host "[GetInstalledVersionFromRegistry] - Searching Version for:"$componentName

$sbContent = {
  param($iComponentName)

    
  $regkey64 = 'HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall'
  $regkey32 = 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall'

  Write-Host "[GetInstalledVersionFromRegistry] - Searching Version for:"$iComponentName

  $key = Get-ChildItem $regkey64 -Recurse | ForEach-Object { Get-ItemProperty $_.PSPath } | Where-Object {$_.DisplayName -eq $iComponentName}
  $foundKey = $true
  $ComponentVersion = ''

  if (!($key)) {
    Write-Host "[GetInstalledVersionFromRegistry] - Not found"$iComponentName"in 64bit keys. Now Searching on 32bit keys..."
    $key = Get-ChildItem $regkey32 -Recurse | ForEach-Object { Get-ItemProperty $_.PSPath } | Where-Object {($_.DisplayName -match $iComponentName)}
  
    if (!($key)) {
        Write-Host "[GetInstalledVersionFromRegistry] - Not found"$iComponentName"in registry!"     
        $foundKey = $false
    }    
  }
  
  if ($foundKey){
        $ComponentVersion = $key.DisplayVersion
  }
    
  Write-Host "[GetInstalledVersionFromRegistry] - Result finding $iComponentName : $ComponentVersion"
  
  return $ComponentVersion
}

#START EXECUTION

if ($psLibs.addTrustedIP($vmIP)) { Get-Item WSMan:\localhost\Client\TrustedHosts }

if ($psLibs.ensureRemoteCall($vmIP, $vmUser, $vmPwd)) {
  Write-Host "[GetInstalledVersionFromRegistry] - invoking command remotely ..."
  $resultVersion = Invoke-Command -Computer $vmIP -ScriptBlock $sbContent -ArgumentList "$componentName"  -Credential $psCredential
}

Write-Host "[GetInstalledVersionFromRegistry] - Result execution remotely $ComponentName : $resultVersion"

if (-Not $psLibs.removeTrustedIP($vmIP)) {Clear-Item WSMan:\localhost\Client\TrustedHosts -Force}

$jsonObj = New-Object -TypeName PSObject

Add-Member -InputObject $jsonObj -NotePropertyName 'ComponentVersion' -NotePropertyValue $resultVersion.toString()

$jsonData = ConvertTo-Json -InputObject $jsonObj

Write-Host "[GetInstalledVersionFromRegistry] - Saving Version to:"$jsonOutFileName

$jsonData | out-file -FilePath $jsonOutFileName -Encoding utf8 -Force

#Set-Content -Path ".\$jsonOutFileName" -Value $jsonData


if ($error) {Write-Host "ERROR: $error"}

Write-Host "[GetInstalledVersionFromRegistry] - EOE $ComponentName : $resultVersion"

return $resultVersion