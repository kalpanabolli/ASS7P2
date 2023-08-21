create database LibraryDB
use LibraryDB
create table Books
(BookId int identity(1,1) primary key,
Title nvarchar(50),
Author nvarchar(50),
Genre nvarchar(50),
quantity int)

insert into Books values('Harry Potter','Kiran Deshey','Narrative',5),
('Mahavir','R K Narayan','Novel',2)
drop table Books

select*from Books