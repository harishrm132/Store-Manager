CREATE PROCEDURE [dbo].[spInventory_Insert]
    @ProductId INT, 
    @Quantity INT , 
    @PurchasePrice MONEY, 
    @PurchaseDate DATETIME2
AS
BEGIN

    insert into dbo.Inventory (ProductId, Quantity, PurchasePrice, PurchaseDate)
    VALUES (@ProductId, @Quantity, @PurchasePrice, @PurchaseDate)
END
