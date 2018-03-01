@echo off

set "USERNAME=%~1"
set "PASSWORD=%~2"
set "SQLSERVER=%~3"
set "SQLAGENT=%~4"


echo Updating SQL Services Log On Account...
echo.

if (%1)==() goto USAGE
if (%2)==() goto USAGE
if (%3)==() goto USAGE
if (%4)==() goto USAGE

echo Setting User Account for SQL Server and SQL Agent Services...

sc config "%SQLSERVER%" obj= "%USERNAME%" password= "%PASSWORD%" TYPE= own
sc config "%SQLAGENT%" obj= "%USERNAME%" password= "%PASSWORD%" TYPE= own
net stop "%SQLAGENT%"
net stop "%SQLSERVER%"
net start "%SQLSERVER%"
net start "%SQLAGENT%"

goto END 


:USAGE
echo Missing parameters: ChangeSQLServicesAccount.cmd domain\user password SQLServerServiceName SQAgentServiceName


:END
if (%5)==(silent) goto ENDSILENT
rem pause

:ENDSILENT
 