param(
    [System.IO.FileInfo]$Path
)
Process {
    try {
        $Result = @{}
        
        # Open MSI database
        $WindowsInstaller = New-Object -ComObject WindowsInstaller.Installer
        $MSIDatabase = $WindowsInstaller.GetType().InvokeMember("OpenDatabase", "InvokeMethod", $null, $WindowsInstaller, @($Path.FullName, 0))
        
        # Query
        $Query = "SELECT Value FROM Property WHERE Property = 'ProductCode'"
        $View = $MSIDatabase.GetType().InvokeMember("OpenView", "InvokeMethod", $null, $MSIDatabase, ($Query))
        $View.GetType().InvokeMember("Execute", "InvokeMethod", $null, $View, $null)
        $Record = $View.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $View, $null)
        If ($Record -ne $null){
            $Value = $Record.GetType().InvokeMember("StringData", "GetProperty", $null, $Record, 1)            
        }
        Else{
            $Value = ""
        }
        $View.GetType().InvokeMember("Close", "InvokeMethod", $null, $View, $null)
        $Result.Add("ProductCode", $Value)
        
        $Query = "SELECT Value FROM Property WHERE Property = 'Manufacturer'"
        $View = $MSIDatabase.GetType().InvokeMember("OpenView", "InvokeMethod", $null, $MSIDatabase, ($Query))
        $View.GetType().InvokeMember("Execute", "InvokeMethod", $null, $View, $null)
        $Record = $View.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $View, $null)
        If ($Record -ne $null){
            $Value = $Record.GetType().InvokeMember("StringData", "GetProperty", $null, $Record, 1)            
        }
        Else{
            $Value = ""
        }
        $View.GetType().InvokeMember("Close", "InvokeMethod", $null, $View, $null)
        $Result.Add("Manufacturer", $Value)
        
        $Query = "SELECT Value FROM Property WHERE Property = 'ProductName'"
        $View = $MSIDatabase.GetType().InvokeMember("OpenView", "InvokeMethod", $null, $MSIDatabase, ($Query))
        $View.GetType().InvokeMember("Execute", "InvokeMethod", $null, $View, $null)
        $Record = $View.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $View, $null)
        If ($Record -ne $null){
            $Value = $Record.GetType().InvokeMember("StringData", "GetProperty", $null, $Record, 1)            
        }
        Else{
            $Value = ""
        }
        $View.GetType().InvokeMember("Close", "InvokeMethod", $null, $View, $null)
        $Result.Add("ProductName", $Value)
        
        $Query = "SELECT Value FROM Property WHERE Property = 'INSTALLATIONUNITGUID'"
        $View = $MSIDatabase.GetType().InvokeMember("OpenView", "InvokeMethod", $null, $MSIDatabase, ($Query))
        $View.GetType().InvokeMember("Execute", "InvokeMethod", $null, $View, $null)
        $Record = $View.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $View, $null)
        If ($Record -ne $null){
            $Value = $Record.GetType().InvokeMember("StringData", "GetProperty", $null, $Record, 1)            
        }
        Else{
            $Value = ""
        }
        $View.GetType().InvokeMember("Close", "InvokeMethod", $null, $View, $null)
        $Result.Add("INSTALLATIONUNITGUID", $Value)

        $Query = "SELECT Value FROM Property WHERE Property = 'INSTALLATIONUNIT'"
        $View = $MSIDatabase.GetType().InvokeMember("OpenView", "InvokeMethod", $null, $MSIDatabase, ($Query))
        $View.GetType().InvokeMember("Execute", "InvokeMethod", $null, $View, $null)
        $Record = $View.GetType().InvokeMember("Fetch", "InvokeMethod", $null, $View, $null)
        If ($Record -ne $null){
            $Value = $Record.GetType().InvokeMember("StringData", "GetProperty", $null, $Record, 1)
        }
        Else{
            $Value = ""
        }
        $View.GetType().InvokeMember("Close", "InvokeMethod", $null, $View, $null)
        $Result.Add("INSTALLATIONUNIT", $Value)
        
        # Commit database and close view        
        $MSIDatabase.GetType().InvokeMember("Commit", "InvokeMethod", $null, $MSIDatabase, $null)
        $MSIDatabase = $null
        $View = $null
 
        # Return the value
        $eof = "`r`n"
        $Json = "{" + $eof +
                    "`"ProductCode`"" + " : " + "`"" + $Result.ProductCode + "`"" +  "," + $eof +
                    "`"Manufacturer`"" + " : " + "`"" + $Result.Manufacturer + "`"" +  "," + $eof +
                    "`"ProductName`"" + " : " + "`"" + $Result.ProductName + "`"" + "," + $eof +
                    "`"INSTALLATIONUNITGUID`"" + " : " + "`"" + $Result.INSTALLATIONUNITGUID + "`"" + "," + $eof +
                    "`"INSTALLATIONUNIT`"" + " : " + "`"" + $Result.INSTALLATIONUNIT + "`"" + $eof +
                 "}"
        return $Json
    } 
    catch {
        Write-Warning -Message $_.Exception.Message
        return "{}"
    }
}
End {
    # Run garbage collection and release ComObject
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($WindowsInstaller) | Out-Null
    [System.GC]::Collect()
}