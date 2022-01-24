CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id int output,
	@CashierId nvarchar(128),
	@SaleDate DATETIME2, 
    @SubTotal MONEY, 
    @Tax MONEY, 
    @Total MONEY
AS
BEGIN
	set nocount on;

	insert into dbo.Sale(CashierId, SaleDate, SubTotal, Tax, Total)
	values (@CashierId, @SaleDate, @SubTotal, @Tax, @Total);

	select @Id = @@IDENTITY;

END
