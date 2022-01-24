﻿CREATE PROCEDURE [dbo].[spSale_Lookup]
	@CashierId nvarchar(128),
	@SaleDate DATETIME2
AS
BEGIN
	set nocount on;

	select Id
	from dbo.Sale
	where CashierId = @CashierId and SaleDate = @SaleDate;

END
