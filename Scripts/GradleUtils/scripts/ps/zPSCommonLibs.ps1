#**************** Common Lib ****************
#************ Author: Quang Truong ************
#************ Modified: Hoai Tang ************
#*********** Last Update: 08/14/2019 ***********

#**************** Main Classes ****************
Class CommonLibs {
	# *** Insert VMIP into TrustedHosts  ***
	[Boolean] addTrustedIP ([String] $remoteIP) {
	if ($error) {$error.clear()}
	$trustedList = (Get-ChildItem WSMan:\localhost\Client\TrustedHosts).Value
	if ($trustedList) {
		if ($trustedList.indexOf($remoteIP) -ge 0) {
			Write-Host "[CommonLib]: IP $remoteIP already in TrustedHosts"
			return $true
		} else { $trustedList += "," + $remoteIP }
	} else { $trustedList = $remoteIP }
	Set-Item WSMan:\localhost\Client\TrustedHosts -Value $trustedList -Force
	if ($error) { return $false }
	Write-Host "[CommonLib]: Added $remoteIP into TrustedHosts"
	return $true
	}
	
	# *** Remove VMIP into TrustedHosts  ***
	[Boolean] removeTrustedIP ([String] $remoteIP){
	if ($error) {$error.clear()}
	$trustedList = (Get-ChildItem WSMan:\localhost\Client\TrustedHosts).Value
	if ($trustedList -or $trustedList.indexOf($remoteIP) -ge 0) {
    if ($trustedList -eq $remoteIP) {
		Clear-Item WSMan:\localhost\Client\TrustedHosts -Force
	} else {
		$trustedList = $trustedList.Replace(($remoteIP + ","), "")
		if ($trustedList.indexOf($remoteIP) -ge 0) { $trustedList = $trustedList.Replace(("," + $remoteIP), "") }
			Set-Item WSMan:\localhost\Client\TrustedHosts -Value $trustedList -Force
		}
	}
	if ($error) { return $false }
	Write-Host "[CommonLib]: Remove $remoteIP out of TrustedHosts"
	return $true
	}
	
	# *** Create PSCredential to remote VM  ***
	[PSCredential] createRemoteCredential ([String] $remoteIP, [String] $user, [String] $password){
	if ($error) {$error.clear()}
	$encryptedPwd = ConvertTo-SecureString -String $password -AsPlainText -Force
	$psCredential = New-Object System.Management.Automation.PSCredential -ArgumentList $user, $encryptedPwd
	if ($error) { return $null }
	return $psCredential
	}
	
	# *** Disconnect from mapping network drive  ***
	[Boolean] disconnectNetworkMap ([String] $remoteFolder){
	if ($error) {$error.clear()}
	$temp = $remoteFolder.Split('\')
	$remoteIP = $temp[2]
	if (!$remoteIP) {Write-Host "[CommonLib]: Can't get remote host"; return $false}
	$scriptNetwork = New-Object -ComObject WScript.Network
	Foreach ($netDrive in $scriptNetwork.EnumNetworkDrives()) {
		if ($netDrive.indexOf($remoteIP) -ge 0) {
			Write-Host "[CommonLib]: Disconnect network drive to $netDrive"
			$scriptNetwork.RemoveNetworkDrive($netDrive)
			return $true
		}
	}
	
	Write-Host "[CommonLib]: There is no connection to folder: $remoteFolder"
	if ($error) { return $false }
	return $true
	}

	# *** Verify WinRM service on remote VM  ***
	[Boolean] verifyWinRMService ([String] $remoteIP){
	if ($error) {$error.clear()}
	$timeout = 9 #timeout 90s
	if (!(Test-WSMan -ComputerName $remoteIP -ErrorAction SilentlyContinue)) {
		for ($i = 0; $i -lt $timeout; $i++) {
			Write-Host "[CommonLib]: Wait for WinRM in 10s ..."
			Start-Sleep -s 10
			if (Test-WSMan -ComputerName $remoteIP -ErrorAction SilentlyContinue) { $i = $timeout; $error.clear() }
		}
	}
	Write-Host "[CommonLib]: WinRM is ready on $remoteIP!"
	if ($error) { return $false }
	return $true
	}
	
	# *** Ensure Invoke-Command on remote VM  ***
	[Boolean] ensureRemoteCall ([String] $remoteIP, [String] $user, [String] $password){
	if ($error) {$error.clear()}
	$psCredential = $this.createRemoteCredential($remoteIP, $user, $password)
	$attempt = 3
	for ($i = 0; $i -lt $attempt; $i++) {
		if ($this.verifyWinRMService($remoteIP)) {
			Write-Host "[CommonLib]: Try Invoke-Command"
			Invoke-Command -Computer $remoteIP -ScriptBlock {ipconfig} -Credential $psCredential
			if ($error) {
				Write-Host "[CommonLib]: Error on PSSession"
				$this.removeTrustedIP($remoteIP)
				Start-Sleep -s 10
				$this.addTrustedIP($remoteIP)
				$error.clear()
			} else { $i = $attempt }
		}
	}
	if ($error) { return $false }
	return $true
	}
	
	# *** Ensure service status on remote VM  ***
	[Boolean] ensureRemoteService ([String] $remoteIP, [String] $user, [String] $password, [String] $serviceName, [String] $status){
	if ($error) {$error.clear()}
	$psCredential = $this.createRemoteCredential($remoteIP, $user, $password)
	if ($this.verifyWinRMService($remoteIP, $user, $password)) {
		$sbContent = ""
		if ($status -eq "Run") {
			$sbContent = {
				param($serName)
				Start-Service -Name $serName
				$service = Get-Service -Name $serName
				$service.WaitForStatus("Running", (New-TimeSpan -Minutes 2))
			}
		}
		if ($status -eq "Wait") {
			$sbContent = {
				param($serName)
				$service = Get-Service -Name $serName
				$service.WaitForStatus("Running", (New-TimeSpan -Minutes 2))
			}
		}
		if ($status -eq "Stop") {
			$sbContent = {
				param($serName)
				Stop-Service -Name $serName
			}
		}
		Invoke-Command -Computer $remoteIP -ScriptBlock $sbContent -ArgumentList $serviceName -Credential $psCredential
	}
	Write-Host "[CommonLib]: Service $serviceName on Vm $remoteIP with status $status"
	if ($error) { return $false }
	return $true
	}
}
