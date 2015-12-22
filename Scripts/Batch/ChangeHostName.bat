@ECHO OFF

REM Get a random number [0,32767], and set it to randname variable
REM set randname="ATL-Slave-D-%RANDOM%"

REM Generate a range between 11 and 99
SET /a _rand=(%RANDOM%*99/32768)+11
set randname="ATL-Slave-D-%_rand%"

REM Execute "netdom" (in WindowsServer 2012) to change computer name to the above randname, and force to restart
netdom renamecomputer %COMPUTERNAME% /NewName:%randname% /Force /REBoot:3
