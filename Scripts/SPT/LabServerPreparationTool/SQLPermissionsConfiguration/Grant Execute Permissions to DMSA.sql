USE [master];
GO

SET NOCOUNT ON;
GO

BEGIN TRY

	IF SUSER_ID('$(I360LoginDomain)\$(I360LoginName)') IS NULL
	BEGIN
		RAISERROR('''$(I360LoginDomain)\$(I360LoginName)'' is not a valid login.', 15, 1);
	END

	BEGIN TRANSACTION;
	
	PRINT 'Grant execute permission on ''sp_prepare'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [sp_prepare] TO [$(I360LoginDomain)\$(I360LoginName)];

	PRINT 'Grant execute permission on ''sp_prepexec'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [sp_prepexec] TO [$(I360LoginDomain)\$(I360LoginName)];
		
	PRINT 'Grant execute permission on ''sp_execute'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [sp_execute] TO [$(I360LoginDomain)\$(I360LoginName)];
	
	PRINT 'Grant execute permission on ''sp_unprepare'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [sp_unprepare] TO [$(I360LoginDomain)\$(I360LoginName)];	
	
	PRINT 'Grant execute permission on ''sp_executesql'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [sp_executesql] TO [$(I360LoginDomain)\$(I360LoginName)];
	
	PRINT 'Grant execute permission on ''xp_msver'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [xp_msver] TO [$(I360LoginDomain)\$(I360LoginName)];

	PRINT 'Grant execute permission on ''xp_sqlagent_enum_jobs'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [xp_sqlagent_enum_jobs] TO [$(I360LoginDomain)\$(I360LoginName)];

	PRINT 'Grant execute permission on ''xp_enum_oledb_providers'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [xp_enum_oledb_providers] TO [$(I360LoginDomain)\$(I360LoginName)];

	PRINT 'Grant execute permission on ''sp_xml_preparedocument'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [sp_xml_preparedocument] TO [$(I360LoginDomain)\$(I360LoginName)];

	PRINT 'Grant execute permission on ''sp_xml_removedocument'' stored procedure to ''$(I360LoginDomain)\$(I360LoginName)'''
	GRANT EXECUTE ON [sp_xml_removedocument] TO [$(I360LoginDomain)\$(I360LoginName)];

	COMMIT TRANSACTION;

END TRY
BEGIN CATCH

	DECLARE @msg NVARCHAR(4000),
			@new_line	CHAR(2)

	SET @new_line = CHAR(13) + CHAR(10);

	--Report for error  (error retrieval routine)
	SET @msg =  'An exception has occurred!' + @new_line +
				' - Error Number: '    + CAST(ERROR_NUMBER() AS VARCHAR)  + @new_line +
				' - Error Severity: '  + CAST(ERROR_SEVERITY()AS VARCHAR) + @new_line +
				' - Error State: '     + CAST(ERROR_STATE()AS VARCHAR)    + @new_line +
				' - Error Procedure '  + ISNULL(ERROR_PROCEDURE(), 'N/A') + @new_line +
				' - Error Line: '      + CAST(ERROR_LINE()AS VARCHAR)     + @new_line +
				' - Error Message: '   + ISNULL(ERROR_MESSAGE(), 'N/A')   + @new_line +	
				'Rolling back any changes...'

    IF (@@TRANCOUNT > 0)
    BEGIN
		ROLLBACK TRANSACTION;
	END
	
	RAISERROR(@msg, 16, 1);

END CATCH

GO
SET NOCOUNT OFF;
GO