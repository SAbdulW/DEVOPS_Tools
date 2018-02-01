param(
    [String]$IP,
    [String]$GUID
)

Process {
    try {
        $Name = "NOT FOUND"

        If($GUID -notmatch "{"){
            $GUID = "{" + $GUID + "}"
        }

        $Reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine,$IP)
        $Path = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
        $RegSubKey = $Reg.OpenSubKey($Path)
        If($RegSubKey -eq $null){
            $Path = "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
            $RegSubKey = $Reg.OpenSubKey($Path)
        }

        If($RegSubKey -ne $null){
            $Name = $RegSubKey.GetValue("DisplayName")
        }

        $eof = "`r`n"
        $Json = "{" + $eof +
                    "`"RunFromComputer`"" + " : " + "`"" + $env:computername + "`"" + "," + $eof +
                    "`"DisplayName`"" + " : " + "`"" + $Name + "`"" + $eof +
                "}"

        return $Json
    }
    catch {
        $eof = "`r`n"
        $Json = "{" + $eof +
                    "`"RunFromComputer`"" + " : " + "`"" + $env:computername + "`"" + "," + $eof +
                    "`"Error`"" + " : " + "`"" + $_.Exception.Message + "`"" + "," + $eof +
                    "`"DisplayName`"" + " : " + "`"" + $Name + "`"" + $eof +
                "}"

        return $Json
    }
}