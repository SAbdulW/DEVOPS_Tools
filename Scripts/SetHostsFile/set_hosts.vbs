strComputer=WScript.Arguments(0)
strUser =WScript.Arguments(1)
strPassword =WScript.Arguments(2)

'strCommand="cmd /c ipconfig >c:\test.txt"
'strComputer = "10.200.50.55" 
'strDomain = "2008_base" 
wscript.echo "Computer Name = " &strComputer
wscript.echo "User Name = " &strUser
wscript.echo "Password = " &strPassword
wscript.echo "Command = " &strCommand

dim NIC1, Nic, StrIP, CompName,file
Set Sh = CreateObject("WScript.Shell")









if WScript.Arguments.count >3 then  

	strDomain=WScript.Arguments(3)
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

file ="C:\Windows\System32\drivers\etc\hosts"
cmd= "cmd /c  ""echo 127.0.0.1	localhost > "&file&" """ 





Set NIC1 = objSWbemServices.InstancesOf("Win32_NetworkAdapterConfiguration")

For Each Nic in NIC1
	if Nic.IPEnabled then
		StrIP = Nic.IPAddress(i)
		Set WshNetwork = WScript.CreateObject("WScript.Network")
		CompName= WshNetwork.Computername
'	MsgBox "IP Address: "&StrIP & vbNewLine _
'& "Computer Name: "&CompName,4160,"IP Address and Computer Name"
		wscript.echo StrIP
		cmd = " echo "& StrIP & " 	localhost" 
'Set objProcess = objSWbemServices.Get("Win32_Process")
		'		intReturn = objProcess.Create (cmd, Null, null, intProcessID)
'		wscript.echo intReturn
	wscript.quit

	end if

next


'wscript.echo "Return value after running = " &intReturn

WScript.Quit intReturn













For Each Nic in NIC1
	if Nic.IPEnabled then
		StrIP = Nic.IPAddress(i)
		Set WshNetwork = WScript.CreateObject("WScript.Network")
		CompName= WshNetwork.Computername
'	MsgBox "IP Address: "&StrIP & vbNewLine _
'& "Computer Name: "&CompName,4160,"IP Address and Computer Name"
wscript.echo StrIP
		CMD = " echo "& StrIP & " 	localhost" 
		Sh.Run "cmd /c  """ & CMD &" >> "&file&" """ , 1, True


'	wscript.quit

	end if

next


Set Sh = Nothing