@echo off

if exist .\WindowsSettings\AccountRights\secedit.sdb del .\WindowsSettings\AccountRights\secedit.sdb /q

SECEDIT /configure /db .\WindowsSettings\AccountRights\secedit.sdb /cfg ".\WindowsSettings\AccountRights\AccountRights.inf" /Log .\Logs\WindowsSettings\SetUserRightsLog.txt
 

if NOT %ERRORLEVEL% == 0 goto ERRORFOUND
goto END

:ERRORFOUND
pause

:END