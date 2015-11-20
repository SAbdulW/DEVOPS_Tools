echo delete c:\share
cmd /c  mkdir c:\share  >c:\setting.log
Echo net share share=c:\share /GRANT:everyone,FULL
cmd /c  net share share=c:\share /GRANT:everyone,FULL >>c:\share\setting.log
echo sc stop puppet 
cmd /c sc stop puppet  >>c:\share\setting.log
echo timeout 15
cmd /c timeout 15 >>c:\share\setting.log
echo sc config puppet start= disabled
cmd /c sc config puppet start= disabled >>c:\share\setting.log
echo rd /S /Q C:\ProgramData\PuppetLabs\puppet\etc\ssl
cmd /c  rd /S /Q C:\ProgramData\PuppetLabs\puppet\etc\ssl >>c:\share\setting.log
set res=0
ipconfig /all| findstr "DHCP Enabled. . . . . . . . . . . : Yes" 1>nul
if errorlevel 1 set res=90
echo netsh interface ip set dns "Ethernet" dhcp 
cmd /c netsh interface ip set dns "Ethernet" dhcp >>c:\share\setting.log
echo netsh interface ip set address "Ethernet" dhcp 
cmd /c netsh interface ip set address "Ethernet" dhcp >>c:\share\setting.log
echo ipconfig /all
cmd /c ipconfig /all >>c:\share\setting.log
echo taskkill /IM "PSEXESVC.exe" 
cmd /c taskkill /IM "PSEXESVC.exe" /F
echo Done!!!!!
exit res