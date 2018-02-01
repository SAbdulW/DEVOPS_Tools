param(
    [String]$IP,
    [String]$GUID
)

Process {
    try {
        $Log = $IP + " - " + $GUID
        $Name = "NOT FOUND"

        If($GUID -notmatch "{"){
            $GUID = "{" + $GUID + "}"
            $Log = $Log + " : " + $GUID
        }

        if (test-connection $IP -count 1 -quiet) {
            $Log = $Log + " : " + "Pinged " + $IP + " succeeded"
        }
        else{
            $Log = $Log + " : " + "Pinged " + $IP + " FAILED"
        }

        $Reg = [Microsoft.Win32.RegistryKey]::OpenRemoteBaseKey([Microsoft.Win32.RegistryHive]::LocalMachine,$IP)
        $Log = $Log + ": " + "Opened remote Registry"
        $Path = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
        $Log = $Log + " : " + $Path
        $RegSubKey = $Reg.OpenSubKey($Path)
        $Log = $Log + " : " + "Opened 64 bit subkey"
        If($RegSubKey -eq $null){
            $Path = "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + $GUID
            $Log = $Log + " : " + $Path
            $RegSubKey = $Reg.OpenSubKey($Path)
            $Log = $Log + " : " + "Opened 64 bit subkey"
        }

        If($RegSubKey -ne $null){
            $Log = $Log + " : " + "Get Value"
            $Name = $RegSubKey.GetValue("DisplayName")
            $Log = $Log + " : " + "Got value = " + $Name
        }
        Else{
            $Log = $Log + " : " + "SubKey was null"
        }

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