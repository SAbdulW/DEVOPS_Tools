SET NOCOUNT ON;
GO

DECLARE @CommandRestore			NVARCHAR(4000),
		@CommandDelete			NVARCHAR(4000),
		@CommandChangeJobOwner	NVARCHAR(4000),
		@RowCount				INT;

SET @CommandDelete = N'';
SET @CommandRestore = N'';
SET @CommandChangeJobOwner = N'';

BEGIN TRY

	IF SUSER_ID('$(I360LoginDomain)\$(I360LoginName)') IS NULL
	BEGIN
		RAISERROR('''$(I360LoginDomain)\$(I360LoginName)'' is not a valid login.', 15, 1);
	END;

	BEGIN TRANSACTION;

	IF EXISTS(SELECT * FROM msdb.dbo.sysproxies WHERE name = N'I360DBProxy')
	BEGIN
		
		SELECT
			@CommandChangeJobOwner = @CommandChangeJobOwner + CAST(N'EXEC msdb.dbo.sp_update_job @job_id = N''' + CAST(job_id AS NVARCHAR(50)) + N''', @owner_login_name = N''$(I360LoginDomain)\$(I360LoginName)''; ' AS NVARCHAR(4000))
		FROM
			msdb.dbo.sysjobsteps js
				INNER JOIN
			msdb.dbo.sysproxies p
				ON js.proxy_id = p.proxy_id
		WHERE
			p.name = N'I360DBProxy';
		
		SET @RowCount = @@ROWCOUNT;

		IF (LEN(@CommandChangeJobOwner) > 0)
		BEGIN
			PRINT 'Change ownership of jobs (' + STR(@RowCount, 2, 0) + ') which are dependent on ''I360DBProxy'' proxy account';
			EXEC(@CommandChangeJobOwner);
		END;

		SELECT-- DISTINCT
			@CommandDelete = @CommandDelete + CAST(N'EXEC msdb.dbo.sp_update_jobstep @job_id = N''' + CAST(job_id AS NVARCHAR(50)) + N''', @step_id=' + CAST(js.step_id AS NVARCHAR(3)) + N', @proxy_name = N''''; ' AS NVARCHAR(4000)),
			@CommandRestore = @CommandRestore + CAST(N'EXEC msdb.dbo.sp_update_jobstep @job_id = N''' + CAST(job_id AS NVARCHAR(50)) + N''', @step_id=' + CAST(js.step_id AS NVARCHAR(3)) + N', @proxy_name = N''' + p.name + '''; ' AS NVARCHAR(4000))
		FROM
			msdb.dbo.sysjobsteps js
				INNER JOIN
			msdb.dbo.sysproxies p
				ON js.proxy_id = p.proxy_id
		WHERE
			p.name = N'I360DBProxy';
		
		SET @RowCount = @@ROWCOUNT;
		
		IF (LEN(@CommandDelete) > 0)
		BEGIN
			PRINT 'Remove all references (' + STR(@RowCount, 2, 0) + ') from the jobs to ''I360DBProxy'' proxy account';
			EXEC(@CommandDelete);
		END;

		PRINT 'Delete ''I360DBProxy'' proxy account';
		EXEC msdb.dbo.sp_delete_proxy @proxy_name = N'I360DBProxy';
	END;

	IF EXISTS(SELECT * FROM sys.credentials WHERE name = N'I360DBCredential')
	BEGIN
		PRINT 'Drop ''I360DBCredential'' credential';
		DROP CREDENTIAL [I360DBCredential];
	END;

	PRINT 'Create ''I360DBCredential'' credential mapped to ''$(I360LoginDomain)\$(I360LoginName)''';
	CREATE CREDENTIAL [I360DBCredential] WITH IDENTITY = N'$(I360LoginDomain)\$(I360LoginName)', SECRET = N'$(I360LoginPwd)';

	PRINT 'Add ''I360DBProxy'' proxy account mapped to ''I360DBCredential'' credential';
	EXEC msdb.dbo.sp_add_proxy	@proxy_name = N'I360DBProxy',
								@credential_name = N'I360DBCredential', 
								@enabled = 1;

	-- Operating System (CmdExec)
	PRINT 'Grant ''I360DBProxy'' proxy to the ''Operating System (CmdExec)'' subsystem';
	EXEC msdb.dbo.sp_grant_proxy_to_subsystem	@proxy_name = N'I360DBProxy',
												@subsystem_id = 3;

	-- SSIS package execution
	PRINT 'Grant ''I360DBProxy'' proxy to the ''SSIS package execution'' subsystem';
	EXEC msdb.dbo.sp_grant_proxy_to_subsystem	@proxy_name = N'I360DBProxy',
												@subsystem_id = 11;

	PRINT 'Grant login to ''I360DBProxy'' proxy';
	EXEC msdb.dbo.sp_grant_login_to_proxy	@proxy_name = N'I360DBProxy',
											@msdb_role = N'I360DBRole';

	IF (DB_ID(N'CommonDB') IS NOT NULL)
	BEGIN
		PRINT 'Set default proxy name (I360DBProxy) in CommonDB database';
		EXEC ('UPDATE [CommonDB].[dbo].[Params] SET [value] = ''I360DBProxy'' WHERE [name] = ''PRM_JOB_PROXY_NAME''');
	END;

	IF (LEN(@CommandRestore) > 0)
	BEGIN
		PRINT 'Restore all references (' + STR(@RowCount, 2, 0) + ') in jobs to ''I360DBProxy'' proxy account';
		EXEC(@CommandRestore);
	END;

	COMMIT TRANSACTION;

END TRY
BEGIN CATCH

	DECLARE @msg		NVARCHAR(4000),
			@new_line	CHAR(2);

	SET @new_line = CHAR(13) + CHAR(10);

	--Report for error  (error retrieval routine)
	SET @msg =  'An exception has occurred!' + @new_line +
				' - Error Number: '    + CAST(ERROR_NUMBER() AS VARCHAR)  + @new_line +
				' - Error Severity: '  + CAST(ERROR_SEVERITY()AS VARCHAR) + @new_line +
				' - Error State: '     + CAST(ERROR_STATE()AS VARCHAR)    + @new_line +
				' - Error Procedure '  + ISNULL(ERROR_PROCEDURE(), 'N/A') + @new_line +
				' - Error Line: '      + CAST(ERROR_LINE()AS VARCHAR)     + @new_line +
				' - Error Message: '   + ISNULL(ERROR_MESSAGE(), 'N/A')   + @new_line +	
				'Rolling back any changes...';

    IF (@@TRANCOUNT > 0)
    BEGIN
		ROLLBACK TRANSACTION;
	END
	
	RAISERROR(@msg, 16, 1);

END CATCH

GO
SET NOCOUNT OFF;
GO