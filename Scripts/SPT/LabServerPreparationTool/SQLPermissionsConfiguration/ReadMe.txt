Database Permissions Configuration Tool

To launch "Database Permissions Configuration Tool" please do the following:
 1) Right click on the script (Setup_signed.ps1)
 2) Select the "Run with Powershell" context menu option

or

 1) Double click on Setup.bat

Note:

   User which runs the "Database Permissions Configuration Tool" needs be a member of SQL Server "sysadmin" role




Changes:

11.2.0.1118:

New: Add new permission "ALTER ANY LINKED SERVER" to DMSA SQL login


11.2.0.1220:

Fix: Alter authorization definition on schemas related SQL login


11.2.1.0131:

New: Add new execute permission to DMSA & IMSA SQL logins

 * GRANT EXECUTE ON [sp_execute] TO [Domain\LoginName];
 * GRANT EXECUTE ON [sp_executesql] TO [Domain\LoginName];

11.2.1.0205

New: Add support Windows 2012 & SQL Server 2012


11.2.1.0220:

New: Add new execute permission to DMSA SQL login

 * GRANT EXECUTE ON [sp_setuserbylogin] TO [Domain\LoginName];

New: Add I360DBUser/I360AppUser and I360DBRole/I360AppRole to all Impact 360 databases (IMSA/DMSA user change)

11.2.1.0221:

New: Add new console execution functionality

Usage example:

 powershell -ExecutionPolicy Bypass -File "%~dp0\Setup_signed.ps1" 
		-SQLServerName "<SERVER_NAME>"
		-SQLServerPort "<PORT>"
		-DatabaseUserName "<DMSA_DOMAIN\DMSA_USERNAME>"
		-DatabaseUserPassword "<DMSA_PASSWORD>"
		-ApplicationUserName "<IMSA_DOMAIN\IMSA_USERNAME>"
		-ApplicationUserPassword "<IMSA_PASSWORD>"
		-LogLocation "<LOG_FILE_LOCATION>"

11.2.1.0226:

Fix: Enhance invalid SQL login exception handling

11.2.1.0508:

New: Add new validation for [public] server role (EXECUTE permission on SQL system extended stored procedures)

Change: remove grant execute permission on [sp_setuserbylogin] from DMSA SQL login
Change: remove grant execute permission on [xp_fixeddrives] from DMSA SQL login
Change: remove grant execute permission on [xp_regread] from DMSA SQL login
Change: remove grant execute permission on [xp_instance_regread] from DMSA SQL login

Change: remove grant execute permission on [xp_fixeddrives] from IMSA SQL login
Change: remove grant execute permission on [xp_regread] from IMSA SQL login
Change: remove grant execute permission on [xp_instance_regread] from IMSA SQL login

11.2.1.0520:

Fix: Update signature certificate (new expiration date is 5/6/2016)

11.2.1.0527

Change: the new tool name is "Database Permissions Configuration Tool"

11.2.1.0617

Fix: in the console mode the internal usage of -DatabaseUserPassword and -ApplicationUserPassword arguments were incorrect

11.2.1.0825:

New: Add new execute permission to DMSA & IMSA SQL logins

 * GRANT EXECUTE ON [sp_xml_preparedocument] TO [Domain\LoginName];
 * GRANT EXECUTE ON [sp_xml_removedocument] TO [Domain\LoginName];
 * GRANT EXECUTE ON [sp_prepare] TO [Domain\LoginName];
 * GRANT EXECUTE ON [sp_prepexec] TO [Domain\LoginName];
 * GRANT EXECUTE ON [sp_unprepare] TO [Domain\LoginName];

11.2.1.1108:

Fix: credential verification for non-domain environment

11.2.2.1027:

Fix: Change ownership of jobs which are dependent on proxy account

11.2.3.0527:

Change: assign [RSExecRole] role to [I360AppUser] user

11.2.4.0111

Change: sign the "Setup_signed.ps1" PowerShell script with SHA256 hashing algorithm and a time stamp server URL specified