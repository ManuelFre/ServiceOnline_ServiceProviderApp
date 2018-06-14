SELECT DATABASEPROPERTYEX('DbServiceProviderApp', 'Updateability');
GO
ALTER DATABASE DbServiceProviderApp
	SET READ_WRITE;
