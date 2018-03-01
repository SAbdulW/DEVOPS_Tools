USE [master];
GO

SET NOCOUNT ON;
GO

BEGIN TRY

	IF SUSER_ID('$(I360LoginDomain)\$(I360LoginName)') IS NULL
	BEGIN
		RAISERROR('''$(I360LoginDomain)\$(I360LoginName)'' is not a valid login.', 15, 1);
	END

	EXECUTE AS LOGIN = 'sa';

	PRINT 'Grant VIEW SERVER STATE permission to ''$(I360LoginDomain)\$(I360LoginName)'' SQL login';
	GRANT VIEW SERVER STATE TO [$(I360LoginDomain)\$(I360LoginName)];
	
	PRINT 'Grant ALTER SETTINGS permission to ''$(I360LoginDomain)\$(I360LoginName)'' SQL login';
	GRANT ALTER SETTINGS TO [$(I360LoginDomain)\$(I360LoginName)];
	
	PRINT 'Grant ALTER ANY LINKED SERVER permission to ''$(I360LoginDomain)\$(I360LoginName)'' SQL login';
	GRANT ALTER ANY LINKED SERVER TO [$(I360LoginDomain)\$(I360LoginName)];
	
	REVERT;

	PRINT 'Add ''$(I360LoginDomain)\$(I360LoginName)'' SQL login to ''bulkadmin'' fixed server role'
	EXEC sp_addsrvrolemember @loginame = N'$(I360LoginDomain)\$(I360LoginName)', @rolename = N'bulkadmin'

	PRINT 'Add ''$(I360LoginDomain)\$(I360LoginName)'' SQL login to ''processadmin'' fixed server role'
	EXEC sp_addsrvrolemember @loginame = N'$(I360LoginDomain)\$(I360LoginName)', @rolename = N'processadmin'
	
END TRY
BEGIN CATCH

	DECLARE @msg NVARCHAR(4000),
			@new_line	CHAR(2)
	
	SET @new_line = CHAR(13) + CHAR(10);

	--Report for error  (error retrieval routine)
	SET @msg =  'An exception has occurred!' + @new_line +
				' - Error Number: '    + CAST(ERROR_NUMBER() AS VARCHAR(11))	+ @new_line +
				' - Error Severity: '  + CAST(ERROR_SEVERITY() AS VARCHAR(11))	+ @new_line +
				' - Error State: '     + CAST(ERROR_STATE() AS VARCHAR(11))		+ @new_line +
				' - Error Procedure: ' + ISNULL(ERROR_PROCEDURE(), 'N/A')		+ @new_line +
				' - Error Line: '      + CAST(ERROR_LINE() AS VARCHAR(11))		+ @new_line +
				' - Error Message: '   + ISNULL(ERROR_MESSAGE(), 'N/A')			+ @new_line +
				'Rolling back any changes...';

	RAISERROR(@msg, 16, 1);
		
END CATCH

GO
SET NOCOUNT OFF;
GO