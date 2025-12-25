-- BACKUP SCRIPT
USE master
GO
BACKUP DATABASE ITHelpDesk 
TO DISK = '/var/opt/mssql/backup/ITHelpDesk.bak'
WITH FORMAT, INIT, NAME = 'ITHelpDesk Full Backup'
GO

-- RESTORE SCRIPT
USE master
GO
RESTORE DATABASE ITHelpDesk 
FROM DISK = '/var/opt/mssql/backup/ITHelpDesk.bak'
WITH REPLACE
GO