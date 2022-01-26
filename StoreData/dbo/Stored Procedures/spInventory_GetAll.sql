CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
BEGIN
	set nocount on;

	Select Id, ProductId, Quantity, PurchasePrice, PurchaseDate
	From dbo.Inventory;
END
