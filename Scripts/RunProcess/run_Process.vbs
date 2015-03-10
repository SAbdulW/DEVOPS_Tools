strComputer=WScript.Arguments(0)
strUser =WScript.Arguments(1)
strPassword =WScript.Arguments(2)
strCommand=WScript.Arguments(3)
wscript.echo "Computer Name = " &strComputer
wscript.echo "User Name = " &strUser
wscript.echo "Password = " &strPassword
wscript.echo "Command = " &strCommand

if WScript.Arguments.count >4 then  

	strDomain=WScript.Arguments(4)
	wscript.echo "Domain = " &strDomain
	Set objSWbemServices = objSWbemLocator.ConnectServer(strComputer,"root\cimv2", _
		 strUser, _
		 strPassword,_
	   "MS_409", _ 
		"NTLMDomain:" + strDomain) 
else
Set objSWbemLocator = CreateObject("WbemScripting.SWbemLocator")
	Set objSWbemServices = objSWbemLocator.ConnectServer(strComputer,"root\cimv2", _
     strUser, _
     strPassword) 

end if 
Set objProcess = objSWbemServices.Get("Win32_Process")
intReturn = objProcess.Create (strCommand, Null, null, intProcessID)


If intReturn <> 0 Then
    Wscript.Echo "Process could not be created." & _
        vbNewLine & "Command line: " & strCommand & _
        vbNewLine & "Return value: " & intReturn
Else
    Wscript.Echo "Process created." & _
        vbNewLine & "Command line: " & strCommand & _
        vbNewLine & "Process ID: " & intProcessID    
    Set colProcessStopTrace = objSWbemServices.ExecNotificationQuery _
        ("SELECT * FROM Win32_ProcessStopTrace")
        WScript.Echo "Waiting for process to stop ..."
    Do
        Set objLatestEvent = colProcessStopTrace.NextEvent
        If objLatestEvent.ProcessId = intProcessID Then
            Wscript.Echo "StoppedProcess Name: " _
                & objLatestEvent.ProcessName
            Wscript.Echo "Process ID: " & objLatestEvent.ProcessId
            WScript.Echo "Exit code: " & objLatestEvent.ExitStatus
			WScript.Quit objLatestEvent.ExitStatus
    End If
  Loop
End If





