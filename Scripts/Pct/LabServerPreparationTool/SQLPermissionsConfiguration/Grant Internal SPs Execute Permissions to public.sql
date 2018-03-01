SET NOCOUNT ON;
GO

SELECT
	'GRANT EXECUTE ON [' + [name] + '] TO [public];'
FROM
	master.sys.system_objects o
WHERE
	o.[name] IN
		(
			'sp_AddFunctionalUnitToComponent',
			'sp_createorphan',
			'sp_droporphans',
			'sp_fetchLOBfromcookie',
			'sp_getschemalock',
			'sp_http_generate_wsdl_complex',
			'sp_http_generate_wsdl_simple',
			'sp_MSgetversion',
			'sp_releaseschemalock',
			'sp_replddlparser',
			'sp_replhelp',
			'sp_replsendtoqueue',
			'sp_replsetsyncstatus',
			'sp_replwritetovarbin',
			'sp_reset_connection',
			'sp_resyncexecute',
			'sp_resyncexecutesql',
			'sp_resyncprepare',
			'sp_resyncuniquetable',
			'sp_SetOBDCertificate',
			'sp_setuserbylogin',
			'sp_showmemo_xml',
			'sp_start_user_instance',
			'sp_update_user_instance',
			'xp_dirtree',
			'xp_fileexist',
			'xp_fixeddrives',
			'xp_getnetname',
			'xp_instance_regread',
			'xp_MSADEnabled',
			'xp_qv',
			'xp_regread',
			'xp_repl_convert_encrypt_sysadmin_wrapper',
			'xp_replposteor'
		)
AND
		NOT EXISTS(	SELECT
					1
				FROM
					master.sys.database_permissions dp
				WHERE
					o.[object_id] = dp.[major_id]
				AND dp.[grantee_principal_id] = DATABASE_PRINCIPAL_ID('public')
				AND dp.[type] = 'EX'
				AND dp.[state] = 'G')
ORDER BY o.[name];