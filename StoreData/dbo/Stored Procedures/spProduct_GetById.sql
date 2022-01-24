CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
BEGIN
	set nocount on;

	Select Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable 
	From dbo.Product
	where Id = @Id
END
