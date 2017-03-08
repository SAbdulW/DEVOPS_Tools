@ECHO OFF

REM Get a random number [0,32767], and set it to randname variable
REM set randname="ATL-Slave-D-%RANDOM%"

REM Generate a range between 11 and 99
REM SET /a _rand=(%RANDOM%*99/32768)+11
REM set randname="ATL-Slave-D-%_rand%"

REM Execute "netdom" (in WindowsServer 2012) to change computer name to the above randname, and force to restart
REM netdom renamecomputer %COMPUTERNAME% /NewName:%randname% /Force /REBoot:3

REM Get current time in HHMMSS
for /F "usebackq tokens=1,2 delims==" %%i in (`wmic os get LocalDateTime /VALUE 2^>NUL`) do if '.%%i.'=='.LocalDateTime.' set cur_time=%%j
set cur_time=%cur_time:~6,2%%cur_time:~8,2%%cur_time:~10,2%%cur_time:~12,2%

REM Format computer name: Type-D-HHMMSS
set new_name="Type-D-%cur_time%"

REM Execute "netdom" (in WindowsServer 2012) to change computer name to the above new_name, and force to restart
netdom renamecomputer %COMPUTERNAME% /NewName:%new_name% /Force /REBoot:3