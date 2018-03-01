@echo off

rem %1 DMSAAccount
rem %2 DMSAPassword
rem %3 IMSAAccount
rem %4 IMSAPassword
rem %5 SQL server
rem %6 SQL port

echo.
echo ----- Adding certificates to trusted store to run signed PS scripts -----
echo.

certutil -addstore -f "CA" Certs\AddTrustRootCA.cer
certutil -addstore -f "TrustedPublisher" Certs\VerintCert.cer


echo.
echo ----- Running DB Permissions Configuration Tool -----
echo.

cd SQLPermissionsConfiguration

echo Running the DB Permissions Configuration Tool...

if (%7)==(silent) goto SILENT

powershell -ExecutionPolicy AllSigned -File "Setup_signed.ps1" -SQLServerName "%5" -SQLServerPort %6 -DatabaseUserName "%1" -DatabaseUserPassword "%2" -ApplicationUserName "%3" -ApplicationUserPassword "%4" -LogLocation "..\Logs\DBPermissions.txt"

echo Setting DB permissions is done.
rem pause
goto END

:SILENT
powershell -ExecutionPolicy AllSigned -File "Setup_signed.ps1" -SQLServerName "%5" -SQLServerPort %6 -DatabaseUserName "%1" -DatabaseUserPassword "%2" -ApplicationUserName "%3" -ApplicationUserPassword "%4" -LogLocation "..\Logs\DBPermissions.txt" >>..\Logs\Silent_SQLPermissionsTool.txt

:END
