param(
    [String]$IP,
    [String]$GUID
)

Process {
    try {
        $Log = "1"
        $Name = "NOT FOUND"
        $Log = $Log + " : 2"

        If($GUID -notmatch "{"){
            $GUID = "{" + $GUID + "}"
        }
        $Log = $Log + " : 3"
        $Reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine,$IP)
        $Log = $Log + " : 4"
        $Path = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
        $Log = $Log + " : 5"
        $RegSubKey = $Reg.OpenSubKey($Path)
        $Log = $Log + " : 6"
        If($RegSubKey -eq $null){
            $Log = $Log + " : 7"
            $Path = "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
            $Log = $Log + " : 8"
            $RegSubKey = $Reg.OpenSubKey($Path)
            $Log = $Log + " : 9"
        }
        $Log = $Log + " : 10"
        If($RegSubKey -ne $null){
            $Log = $Log + " : 11"
            $Name = $RegSubKey.GetValue("DisplayName")
        }

        $Log = $Log + " : 12"
        $eof = "`r`n"
        $Json = "{" + $eof +
                    "`"Log`"" + " : " + "`"" + $Log + "`"" + "," + $eof +
                    "`"RunFromComputer`"" + " : " + "`"" + $env:computername + "`"" + "," + $eof +
                    "`"DisplayName`"" + " : " + "`"" + $Name + "`"" + $eof +
                "}"

        return $Json
    }
    catch {
        $eof = "`r`n"
        $Json = "{" + $eof +
                    "`"Log`"" + " : " + "`"" + $Log + "`"" + "," + $eof +
                    "`"RunFromComputer`"" + " : " + "`"" + $env:computername + "`"" + "," + $eof +
                    "`"Error`"" + " : " + "`"" + $_.Exception.Message + "`"" + "," + $eof +
                    "`"DisplayName`"" + " : " + "`"" + $Name + "`"" + $eof +
                "}"

        return $Json
    }
}