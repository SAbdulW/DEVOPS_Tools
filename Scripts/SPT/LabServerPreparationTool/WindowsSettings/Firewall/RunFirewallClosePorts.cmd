@echo off

if (%1)==() goto USAGE

SET /P ANSWER=Are you sure you want to disable the firewall for platform %1 (Y/N)?
if /i {%ANSWER%}=={y} (goto :RUN)
if /i {%ANSWER%}=={n} (goto :END)

echo Invalid Input
goto :END

:RUN
cd WindowsSettings\Firewall
powershell.exe -ExecutionPolicy AllSigned /f CloseFirewallPorts_11-2.ps1 %1
net stop MpsSvc
net start MpsSvc
goto END

:USAGE
echo Platform type was not provided


:END
pause

 