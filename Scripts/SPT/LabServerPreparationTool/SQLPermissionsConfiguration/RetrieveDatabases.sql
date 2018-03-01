SET NOCOUNT ON;
GO

SELECT
	[name]
FROM
	sys.databases
WHERE
	[name] IN
		(				
			'Archive',
			'Biometrics',
			'BOE115',
			'BPMAINDB',
			'BPWAREHOUSEDB',
			'BPWHATIFDB',
			'CentralApp',
			'CentralContact',
			'CentralDWH',
			'CommonDB',
			'ETLSTAGINGDB',
			'INCIDENT',
			'intelliminer',
			'LocalContact',
			'PCMON',
			'SpeechAnalytics',
			'SpeechProducts',
			'SSCACHEDB'
		);