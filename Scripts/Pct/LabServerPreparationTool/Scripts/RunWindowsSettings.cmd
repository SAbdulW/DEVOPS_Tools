@echo off

echo.
echo ----- Configuring Windows Services States -----
echo.

if exist .\WindowsSettings\WindowsServices.sdb del .\WindowsSettings\WindowsServices.sdb /q
SECEDIT /configure /db .\WindowsSettings\WindowsServices.sdb /cfg ".\WindowsSettings\ServicesTemplate.inf" /Log .\Logs\WindowsSettings\WindowsServices.txt
 
echo.
echo ----- Configuring Required Windows Settings -----
echo.

if exist .\WindowsSettings\WindowsSettings.sdb del .\WindowsSettings\WindowsSettings.sdb /q
SECEDIT /configure /db .\WindowsSettings\WindowsSettings.sdb /cfg ".\WindowsSettings\WindowsSettingsTemplate.inf" /Log .\Logs\WindowsSettings\WindowsSettings.txt


rem ----- Done with OS Settings Configuration -----


if NOT %ERRORLEVEL% == 0 goto ERRORFOUND
goto END

:ERRORFOUND
pause

:END
 