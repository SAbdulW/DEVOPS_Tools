param(
    [String]$IP,
    [String]$GUID
)

$Result = "NOT FOUND"

$Reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine,$IP) 
$Path = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
$RegSubKey = $Reg.OpenSubKey($Path)
If($RegSubKey -eq $null){
    $Path = "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
    $RegSubKey = $Reg.OpenSubKey($Path)
    If($RegSubKey -ne $null){
        $Result = $RegSubKey.GetValue("DisplayName")    
    }
}

return $Result