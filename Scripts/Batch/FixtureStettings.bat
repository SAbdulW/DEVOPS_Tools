cmd /c  mkdir c:\share  >c:\setting.log
cmd /c  net share share=c:\share /GRANT:everyone,FULL >>c:\share\setting.log
cmd /c  rd /S /Q C:\ProgramData\PuppetLabs\puppet\etc\ssl >>c:\share\setting.log
cmd /c sc stop puppet  >>c:\share\setting.log
cmd /c sc config puppet start= disabled >>c:\share\setting.log
cmd /c netsh interface ip set dns "Local Area Connection" dhcp >>c:\share\setting.log
cmd /k netsh interface ip set address "Local Area Connection" dhcp >>c:\share\setting.log



