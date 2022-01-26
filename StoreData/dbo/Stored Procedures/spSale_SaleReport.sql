CREATE PROCEDURE [dbo].[spSale_SaleReport]
AS
BEGIN
	set nocount on;

	select u.FirstName, u.LastName, u.EmailAddress, [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total]
	from dbo.Sale s
	inner join dbo.[User] u on s.CashierId = u.Id;
END
