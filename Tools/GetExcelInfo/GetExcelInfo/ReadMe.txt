1.The excel file shold be placed in the C:\
2.The name of the excel file shold be 1.xlsx
3.You could change the connection string to sql server in the App.config.

4.The C:\1.xlsx is hard coding in the Program.cs.

5.The procedure of importing the data to the database.

ALTER procedure [dbo].[InitialDataBase]
	@BookNumber NVarChar(max),
	@Title NVarChar(max),
	@Description NVarChar(max),
	@UserAndTeam NVarChar(max),
	@Publisher NVarChar(max),
	@Supplier NVarChar(max),
	@NetPrice float,
	@PurchaseDate datetime,
	@RequestedBy NVarChar(max),
	@BorrowBy nvarchar(max),
	@BorrowedDate datetime
as
begin
	declare @bookCount int
	select @bookCount=count(1) from Books where BookNumber = @BookNumber
	if(@bookCount = 0)
	begin
		insert into BookTypes values(@Title,@Description,@UserAndTeam,@Publisher)
		declare @currentBookTypeId bigint
		select @currentBookTypeId=max(BookTypeId) from BookTypes
		insert into Books values(@BookNumber,@NetPrice,@PurchaseDate,@RequestedBy,'',@Supplier,@currentBookTypeId)
		if(@BorrowBy is not null and @BorrowBy != '')
		begin
			declare @bookid bigint
			select @bookid = max(BookId) from Books
			declare @userId bigint
			select @userId=UserId from Users where FullName = @BorrowBy
			insert into BorrowRecords values(@bookid,@userId,1,@BorrowedDate)
		end
	end
end