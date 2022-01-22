CREATE PROCEDURE [dbo].[spUserLookup]
	@Id NVARCHAR(128)
AS
BEGIN
	set nocount on;

	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate 
	from [dbo].[User]
	where Id = @Id
END
