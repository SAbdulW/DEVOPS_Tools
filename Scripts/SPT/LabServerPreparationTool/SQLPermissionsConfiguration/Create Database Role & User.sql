SET NOCOUNT ON;
GO

DECLARE @RoleName		SYSNAME,
		@RoleMemberName SYSNAME,
		@UserName		SYSNAME,
		@Command		NVARCHAR(4000)
		
SET @RoleName = N'$(I360RoleName)';
SET @UserName = N'$(I360UserName)';
SET @Command  = N'';

BEGIN TRY

	IF SUSER_ID('$(I360LoginDomain)\$(I360LoginName)') IS NULL
	BEGIN
		RAISERROR('''$(I360LoginDomain)\$(I360LoginName)'' is not a valid login.', 15, 1);
	END

	BEGIN TRANSACTION;
	
	-- Modify schemas related to the affected SQL login
	SELECT
		@Command = @Command + 'ALTER AUTHORIZATION ON SCHEMA::[' + s.[name] + '] TO [dbo];'
	FROM
		sys.schemas s
			INNER JOIN
		sys.database_principals dp
			ON s.principal_id = dp.principal_id
	WHERE
		dp.[sid] = SUSER_SID('$(I360LoginDomain)\$(I360LoginName)')
	AND dp.[type] = 'U'
		
	IF (LEN(@Command) > 0)
	BEGIN
		PRINT 'Alter authorization definition on schemas related SQL login ''$(I360LoginDomain)\$(I360LoginName)'' to ''dbo''';
		EXEC(@Command);
		
		SET @Command  = N'';
	END
	
	-- Check if there is a mapping to some other database user and drop it
	SELECT
		@Command = 'DROP USER [' + [name] + '];'
	FROM
		sys.database_principals
	WHERE
		[sid] = SUSER_SID('$(I360LoginDomain)\$(I360LoginName)')
	AND [name] NOT LIKE '%DB%'
	AND [type] = 'U'
	
	IF (LEN(@Command) > 0)
	BEGIN
		PRINT 'Remove user related to SQL login ''$(I360LoginDomain)\$(I360LoginName)''';
		EXEC(@Command);
		
		SET @Command  = N'';
	END

	IF EXISTS (SELECT * FROM sys.database_principals WHERE [name] = @RoleName AND [type] = 'R')
	BEGIN
		SELECT
			@Command = @Command + N'EXEC sp_droprolemember @rolename = ''' + @RoleName + N''', @membername = ''' + [name] + N''';'
		FROM
			sys.database_principals dp
				INNER JOIN
			sys.database_role_members drm
				ON dp.principal_id = drm.member_principal_id
		WHERE
			drm.role_principal_id = DATABASE_PRINCIPAL_ID(@RoleName)

		IF (LEN(@Command) > 0)
		BEGIN
			PRINT 'Drop all members from ''$(I360RoleName)'' database role';
			EXEC(@Command);
			
			SET @Command  = N'';
		END
	END

	IF EXISTS (SELECT * FROM sys.database_principals WHERE [name] = @RoleName AND [type] = 'R')
	BEGIN
		PRINT 'Drop database role ''$(I360RoleName)''';
		DROP ROLE [$(I360RoleName)];
	END

	PRINT 'Create database role ''$(I360RoleName)''';
	CREATE ROLE [$(I360RoleName)] AUTHORIZATION [dbo];

	IF EXISTS (SELECT * FROM sys.database_principals WHERE [name] = @UserName AND [type] = 'U')
	BEGIN
		PRINT 'Drop database user ''$(I360UserName)''';
		DROP USER [$(I360UserName)];
	END

	-- Create database user (if required)
	IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE [sid] = SUSER_SID('$(I360LoginDomain)\$(I360LoginName)') AND [type] = 'U')
	BEGIN
		PRINT 'Create database user ''$(I360UserName)'' for SQL login ''$(I360LoginDomain)\$(I360LoginName)'' mapped to dbo schema';
		CREATE USER [$(I360UserName)] FOR LOGIN [$(I360LoginDomain)\$(I360LoginName)] WITH DEFAULT_SCHEMA=[dbo];
	END ELSE
	BEGIN
		SELECT
			@UserName = [name]
		FROM
			sys.database_principals
		WHERE
			[sid] = SUSER_SID('$(I360LoginDomain)\$(I360LoginName)')
		AND [type] = 'U'
	END
	
	IF (DB_NAME() = 'msdb')
	BEGIN
		IF DATABASE_PRINCIPAL_ID(N'db_dtsadmin') IS NOT NULL AND @UserName NOT LIKE '%App%'
		BEGIN
			PRINT 'Add ''db_dtsadmin'' database role to ''' + @UserName + ''' user';
			EXEC sp_addrolemember N'db_dtsadmin', @UserName;
		END
		
		IF DATABASE_PRINCIPAL_ID(N'db_ssisadmin') IS NOT NULL AND @UserName NOT LIKE '%App%'
		BEGIN
			PRINT 'Add ''db_ssisadmin'' database role to ''' + @UserName + ''' user';
			EXEC sp_addrolemember N'db_ssisadmin', @UserName;
		END

		IF DATABASE_PRINCIPAL_ID(N'RSExecRole') IS NOT NULL AND @UserName LIKE '%App%'
		BEGIN
			PRINT 'Add ''RSExecRole'' database role to ''' + @UserName + ''' user';
			EXEC sp_addrolemember N'RSExecRole', @UserName;
		END

		PRINT 'Add ''db_datareader'' database role to ''' + @UserName + ''' user';
		EXEC sp_addrolemember N'db_datareader', @UserName;
		
		PRINT 'Add ''SQLAgentReaderRole'' database role to ''' + @UserName + ''' user';
		EXEC sp_addrolemember N'SQLAgentReaderRole', @UserName;
	END
	
	ELSE
	
	BEGIN
	
		PRINT 'Add ''db_owner'' database role to ''' + @UserName + ''' user';
		EXEC sp_addrolemember N'db_owner', @UserName;

	END
	
	PRINT 'Add ''$(I360RoleName)'' database role to ''' + @UserName + ''' user';
	EXEC sp_addrolemember N'$(I360RoleName)', @UserName;
	
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