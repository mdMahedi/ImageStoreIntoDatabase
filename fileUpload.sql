create table tblImages
(
	Id int primary key identity,
	Name nvarchar (255),
	Size int,
	ImageData varbinary(max)
)

create proc UploadImages
@name nvarchar (255),
@size int,
@imageData varbinary (max),
@newId int output
as
Begin
	Insert into tblImages
	values (@name,@size,@imageData)
	select @newId=SCOPE_IDENTITY()
End

create proc GetImagesById
@id int
as
Begin
	Select ImageData from tblImages where Id=@id
End

select * from tblImages