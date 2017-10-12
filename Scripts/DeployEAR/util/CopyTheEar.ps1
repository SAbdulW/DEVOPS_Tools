param($vmIP, $vmUser="ap2admin", $vmPwd="verint1!", $toFolder)
. $PSScriptRoot\PSConstant.ps1
. $PSScriptRoot\PSCommonLibs.ps1

if ($error) { $error.clear() }

$psLibs = New-Object CommonLibs
$psCredential = $psLibs.createRemoteCredential($vmIP, $vmUser, $vmPwd)

Write-Host "[Trying to copy the ear from VM:"$vmIP

$impact360SoftwareDirRemote = ""

$sbContent = {
  param()
  
  $impact360SoftwareDir = (Get-ChildItem Env:IMPACT360SOFTWAREDIR).value
  
  Write-Host "[CopyTheEar] - Trying to get the ear from:"$impact360SoftwareDir
  
  $earLocation = "$impact360SoftwareDir\ProductionServer\weblogic\Impact360\wfoSuite.ear"  
  $targetLocation = "C:\Windows\Temp"

  Copy-Item -Path $earLocation -Destination $targetLocation  -Force -PassThru -Verbose ;

  Write-Host "[CopyTheEar] - Copying to common place "
  
  return $impact360SoftwareDir
}

#START EXECUTION

if ($psLibs.addTrustedIP($vmIP)) { Get-Item WSMan:\localhost\Client\TrustedHosts }

if ($psLibs.ensureRemoteCall($vmIP, $vmUser, $vmPwd)) {
  Write-Host "[CopyTheEar] - invoking command remotely ..."
  $impact360SoftwareDirRemote = Invoke-Command -Computer $vmIP -ScriptBlock $sbContent -Credential $psCredential
}

#Copy to local
$srcEarLocation = "X:\wfoSuite.ear"

$remoteFolder = '\\' + $vmIP + '\C$\Windows\Temp'

if ($psLibs.disconnectNetworkMap($remoteFolder)) {
  New-PSDrive -Name X -PSProvider FileSystem -Root $remoteFolder -Credential $psCredential
}


while (!(Test-Path $srcEarLocation -PathType Leaf)){
    Write-Host "[CopyTheEar] - Waiting for the availablitiy of $srcEarLocation"
    Start-Sleep 10
}

New-Item -ItemType Directory -Force -Path $toFolder

Move-Item -Path $srcEarLocation -Destination $toFolder  -Force -PassThru -Verbose ;


Write-Host "[CopyTheEar] - Result execution remotely Impact360Software folder : $impact360SoftwareDirRemote"

if (-Not $psLibs.removeTrustedIP($vmIP)) {Clear-Item WSMan:\localhost\Client\TrustedHosts -Force}
Remove-PSDrive -Name X -Force


#$jsonObj = New-Object -TypeName PSObject

#Add-Member -InputObject $jsonObj -NotePropertyName 'IMPACT360SOFTWAREDIR' -NotePropertyValue $impact360SoftwareDirRemote.toString()

#$jsonData = ConvertTo-Json -InputObject $jsonObj

#Write-Host "[CopyTheEar] - Saving Version to:"$jsonOutFileName

#$jsonData | out-file -FilePath $jsonOutFileName -Encoding utf8 -Force

##Set-Content -Path ".\$jsonOutFileName" -Value $jsonData


if ($error) {Write-Host "ERROR: $error"}

Write-Host "[CopyTheEar] - EOE IMPACT360SOFTWAREDIR : $impact360SoftwareDirRemote"
Write-Host "[CopyTheEar] - Check the availability of the ear here : $toFolder"

return $impact360SoftwareDirRemote