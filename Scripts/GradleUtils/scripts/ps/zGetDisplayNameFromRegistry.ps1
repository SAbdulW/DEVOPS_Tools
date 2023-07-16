param($vmIP, $vmUser="ap2admin", $vmPwd="verint1!", $componentGUID, $jsonOutFileName)
. $PSScriptRoot\zPSConstant.ps1
. $PSScriptRoot\zPSCommonLibs.ps1

if ($error) { $error.clear() }
$resultDisplayName = ''
$psLibs = New-Object CommonLibs
$psCredential = $psLibs.createRemoteCredential($vmIP, $vmUser, $vmPwd)

Write-Host "[zGetDisplayNameFromRegistry] - vmUser:"$vmUser
Write-Host "[zGetDisplayNameFromRegistry] - Getting credential:"$psCredential.Username
Write-Host "[zGetDisplayNameFromRegistry] - Searching DisplayName for:"$componentGUID

$sbContent = {
  param($iComponentGUID)
  
    
  $regkey64 = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{$iComponentGUID}"
  $regkey32 = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{$iComponentGUID}"

  Write-Host "[zGetDisplayNameFromRegistry] - Searching DisplayName for:"$iComponentGUID
  Write-Host "[zGetDisplayNameFromRegistry] - Searching registry: "$regkey64
  $key =  Get-ItemProperty -Path $regkey64 
  $foundKey = $true
  $ComponentDisplayName = ''

  if (!($key)) {
    Write-Host "[zGetDisplayNameFromRegistry] - Not found"$iComponentGUID"in 64bit keys. Now Searching on 32bit keys..."
    Write-Host "[zGetDisplayNameFromRegistry] - Searching registry: "$regkey32
    $key =  Get-ItemProperty -Path $regkey32 
  
    if (!($key)) {
        Write-Host "[zGetDisplayNameFromRegistry] - Not found"$iComponentGUID"in registry!"     
        $foundKey = $false
    }    
  }
  
  if ($foundKey){
        $ComponentDisplayName = $key.DisplayName
  }
    
  Write-Host "[zGetDisplayNameFromRegistry] - Result finding $iComponentGUID : $ComponentDisplayName"
  
  return $ComponentDisplayName
}

#START EXECUTION

if ($psLibs.addTrustedIP($vmIP)) { Get-Item WSMan:\localhost\Client\TrustedHosts }

if ($psLibs.ensureRemoteCall($vmIP, $vmUser, $vmPwd)) {
  Write-Host "[zGetDisplayNameFromRegistry] - invoking command remotely ..."
  $resultDisplayName = Invoke-Command -Computer $vmIP -ScriptBlock $sbContent -ArgumentList "$componentGUID"  -Credential $psCredential
}

Write-Host "[zGetDisplayNameFromRegistry] - Result execution remotely $componentGUID : $resultDisplayName"

if (-Not $psLibs.removeTrustedIP($vmIP)) {Clear-Item WSMan:\localhost\Client\TrustedHosts -Force}

$jsonObj = New-Object -TypeName PSObject

Add-Member -InputObject $jsonObj -NotePropertyName 'ComponentDisplayName' -NotePropertyValue $resultDisplayName.toString()

$jsonData = ConvertTo-Json -InputObject $jsonObj

Write-Host "[zGetDisplayNameFromRegistry] - Saving DisplayName to:"$jsonOutFileName

$jsonData | out-file -FilePath $jsonOutFileName -Encoding utf8 -Force

#Set-Content -Path ".\$jsonOutFileName" -Value $jsonData


if ($error) {Write-Host "ERROR: $error"}

Write-Host "[zGetDisplayNameFromRegistry] - EOE $componentGUID : $resultDisplayName"

return $resultDisplayName