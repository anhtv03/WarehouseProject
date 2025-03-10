--create database WarehouseDB
USE [WarehouseDB]
GO


-- T?o b?ng Roles
CREATE TABLE [dbo].[Roles](
    [RoleID] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
    [RoleName] [nvarchar](50) NOT NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
) 

-- T?o b?ng Categories
CREATE TABLE [dbo].[Categories](
    [CategoryID] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](max) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
) 


-- T?o b?ng Suppliers
CREATE TABLE [dbo].[Suppliers](
    [SupplierID] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Phone] [nvarchar](20) NULL,
    [Email] [nvarchar](100) NULL,
    [Address] [nvarchar](max) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
) 

-- T?o b?ng Users
CREATE TABLE [dbo].[Users](
    [UserID] [int] IDENTITY(1,1 ) PRIMARY KEY  NOT NULL,
    [Username] [nvarchar](50) NOT NULL,
    [Password] [nvarchar](100) NOT NULL,
    [FullName] [nvarchar](100) NULL,
    [Phone] [nvarchar](20) NULL,
    [Email] [nvarchar](100) NULL,
    [Address] [nvarchar](max) NULL,
    [Role] [int] NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL
) 

-- T?o b?ng Products
CREATE TABLE [dbo].[Products](
    [ProductID] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](max) NULL,
    [Images] [nvarchar](255) NULL,
    [Unit] [nvarchar](20) NULL,
    [Quantity] [int] NULL DEFAULT ((0)),
    [AvailableQuantity] [int] NULL DEFAULT ((0)),
    [Price] [decimal](10, 2) NOT NULL,
    [CostPrice] [decimal](10, 2) NOT NULL,
    [CategoryID] [int] NULL,
	[SupplierID] [int] NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
)

CREATE TABLE [dbo].[Customer](
    [CustomerID] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
    [FullName] [nvarchar](100) NULL,
    [Phone] [nvarchar](20) NULL,
    [Email] [nvarchar](100) NULL,
    [Address] [nvarchar](max) NULL,
    [Note] [nvarchar] (500) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
)

CREATE TABLE [dbo].[Order](
    [OrderID] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
    [Status] [nvarchar](100) NOT NULL,
    [UserID] [int] NULL,
	[CustomerID] [int] NULL,
	[SupplierID] [int] NULL,
	[OrderDate] [datetime] NOT NULL DEFAULT (GETDATE()),
	[Note] [nvarchar] (500) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
)

CREATE TABLE [dbo].[OrderDetail](
    [OrderDetailID] [int] IDENTITY(1,1) PRIMARY KEY  NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NULL DEFAULT ((0)),
	[TotalPrice] [decimal](10, 2) NOT NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
)





-- T?o các ràng bu?c khóa ngo?i
-- Khóa ngoại cho bảng Users
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT FK_Users_Roles
FOREIGN KEY (Role) REFERENCES [dbo].[Roles](RoleID);

-- Khóa ngoại cho bảng Products
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (CategoryID) REFERENCES [dbo].[Categories](CategoryID);

ALTER TABLE [dbo].[Products]
ADD CONSTRAINT FK_Products_Suppliers
FOREIGN KEY (SupplierID) REFERENCES [dbo].[Suppliers](SupplierID);

-- Khóa ngoại cho bảng Order
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT FK_Order_Users
FOREIGN KEY (UserID) REFERENCES [dbo].[Users](UserID);

ALTER TABLE [dbo].[Order]
ADD CONSTRAINT FK_Order_Customers
FOREIGN KEY (CustomerID) REFERENCES [dbo].[Customer](CustomerID);

ALTER TABLE [dbo].[Order]
ADD CONSTRAINT FK_Order_Suppliers
FOREIGN KEY (SupplierID) REFERENCES [dbo].[Suppliers](SupplierID);

-- Khóa ngoại cho bảng OrderDetail
ALTER TABLE [dbo].[OrderDetail]
ADD CONSTRAINT FK_OrderDetail_Order
FOREIGN KEY (OrderID) REFERENCES [dbo].[Order](OrderID);

ALTER TABLE [dbo].[OrderDetail]
ADD CONSTRAINT FK_OrderDetail_Product
FOREIGN KEY (ProductID) REFERENCES [dbo].[Products](ProductID);

