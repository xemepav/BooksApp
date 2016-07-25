USE master
GO

CREATE DATABASE BooksApp
GO

USE BooksApp
GO

CREATE TABLE Books
(
     Id uniqueidentifier NOT NULL PRIMARY KEY
    ,ISBN char(17) NOT NULL
    ,Author nvarchar(256) NOT NULL
    ,Title nvarchar(1024) NOT NULL
)
GO

CREATE TABLE Providers
(
     Id uniqueidentifier NOT NULL PRIMARY KEY
    ,Name nvarchar(32) NOT NULL
    ,CONSTRAINT UniqueKey_Name UNIQUE(Name)
)
GO

CREATE TABLE BookProviders
(
     BookId uniqueidentifier FOREIGN KEY REFERENCES Books(Id)
    ,ProviderId uniqueidentifier FOREIGN KEY REFERENCES Providers(Id)
    ,CONSTRAINT UniqueKey_BookIdProviderId UNIQUE(BookId, ProviderId)
)
GO