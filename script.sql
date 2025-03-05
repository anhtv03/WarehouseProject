create database WarehouseDB
USE [WarehouseDB]
GO

-- Xóa các b?ng n?u dã t?n t?i
IF OBJECT_ID('dbo.InventoryHistory', 'U') IS NOT NULL DROP TABLE [dbo].[InventoryHistory];
IF OBJECT_ID('dbo.InventoryQuota', 'U') IS NOT NULL DROP TABLE [dbo].[InventoryQuota];
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE [dbo].[Orders];
IF OBJECT_ID('dbo.Shipping', 'U') IS NOT NULL DROP TABLE [dbo].[Shipping];
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE [dbo].[Products];
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL DROP TABLE [dbo].[Categories];
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE [dbo].[Users];
IF OBJECT_ID('dbo.Suppliers', 'U') IS NOT NULL DROP TABLE [dbo].[Suppliers];
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE [dbo].[Customers];
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE [dbo].[Roles];
GO

-- T?o b?ng Roles
CREATE TABLE [dbo].[Roles](
    [RoleID] [int] IDENTITY(1,1) NOT NULL,
    [RoleName] [nvarchar](50) NOT NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([RoleID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng Categories
CREATE TABLE [dbo].[Categories](
    [CategoryID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](max) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([CategoryID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng Customers
CREATE TABLE [dbo].[Customers](
    [CustomerID] [int] IDENTITY(1,1) NOT NULL,
    [FullName] [nvarchar](100) NULL,
    [Phone] [nvarchar](20) NULL,
    [Email] [nvarchar](100) NULL,
    [Address] [nvarchar](max) NULL,
    [Note] [nvarchar](max) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([CustomerID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng Suppliers
CREATE TABLE [dbo].[Suppliers](
    [SupplierID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Phone] [nvarchar](20) NULL,
    [Email] [nvarchar](100) NULL,
    [Address] [nvarchar](max) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([SupplierID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng Users
CREATE TABLE [dbo].[Users](
    [UserID] [int] IDENTITY(1,1) NOT NULL,
    [Username] [nvarchar](50) NOT NULL,
    [Password] [nvarchar](100) NOT NULL,
    [FullName] [nvarchar](100) NULL,
    [Phone] [nvarchar](20) NULL,
    [Email] [nvarchar](100) NULL,
    [Address] [nvarchar](max) NULL,
    [Role] [int] NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [UQ__Users__Username] UNIQUE NONCLUSTERED ([Username] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng Products
CREATE TABLE [dbo].[Products](
    [ProductID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](max) NULL,
    [Images] [nvarchar](255) NULL,
    [Unit] [nvarchar](20) NULL,
    [Quantity] [int] NULL DEFAULT ((0)),
    [AvailableQuantity] [int] NULL DEFAULT ((0)),
    [Price] [decimal](10, 2) NOT NULL,
    [CostPrice] [decimal](10, 2) NOT NULL,
    [CategoryID] [int] NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng Shipping
CREATE TABLE [dbo].[Shipping](
    [ShippingID] [int] IDENTITY(1,1) NOT NULL,
    [Carrier] [nvarchar](100) NULL,
    [ShippingCost] [decimal](10, 2) NULL,
    [ShippingDate] [datetime] NULL,
    [EstimatedDeliveryDate] [datetime] NULL,
    [Status] [int] NULL,
    [TrackingNumber] [nvarchar](50) NULL,
    [Note] [nvarchar](max) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([ShippingID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng Orders
CREATE TABLE [dbo].[Orders](
    [OrderID] [int] IDENTITY(1,1) NOT NULL,
    [ProductID] [int] NOT NULL,
    [CustomerID] [int] NULL,
    [SupplierID] [int] NULL,
    [Quantity] [int] NOT NULL,
    [UnitPrice] [decimal](10, 2) NOT NULL,
    [TotalPrice] [decimal](10, 2) NOT NULL,
    [OrderDate] [datetime] NULL DEFAULT (GETDATE()),
    [OrderType] [int] NULL,
    [Status] [int] NULL,
    [UserID] [int] NOT NULL,
    [ShippingID] [int] NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([OrderID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng InventoryQuota
CREATE TABLE [dbo].[InventoryQuota](
    [QuotaID] [int] IDENTITY(1,1) NOT NULL,
    [ProductID] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    [Length] [decimal](5, 2) NULL,
    [Width] [decimal](5, 2) NULL,
    [Height] [decimal](5, 2) NULL,
    [Note] [nvarchar](max) NULL,
    [CreatedDate] [datetime] NULL DEFAULT (GETDATE()),
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([QuotaID] ASC)
) ON [PRIMARY]
GO

-- T?o b?ng InventoryHistory
CREATE TABLE [dbo].[InventoryHistory](
    [HistoryID] [int] IDENTITY(1,1) NOT NULL,
    [ProductID] [int] NOT NULL,
    [OrderID] [int] NULL,
    [ChangeType] [int] NULL,
    [QuantityChanged] [int] NOT NULL,
    [PreviousQuantity] [int] NOT NULL,
    [NewQuantity] [int] NOT NULL,
    [ChangeDate] [datetime] NULL DEFAULT (GETDATE()),
    [UserID] [int] NOT NULL,
    [Note] [nvarchar](max) NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime] NULL,
    PRIMARY KEY CLUSTERED ([HistoryID] ASC)
) ON [PRIMARY]
GO

-- T?o các ràng bu?c khóa ngo?i
ALTER TABLE [dbo].[InventoryHistory] WITH CHECK ADD FOREIGN KEY([OrderID]) REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[InventoryHistory] WITH CHECK ADD FOREIGN KEY([ProductID]) REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[InventoryHistory] WITH CHECK ADD FOREIGN KEY([UserID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[InventoryQuota] WITH CHECK ADD FOREIGN KEY([ProductID]) REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Orders] WITH CHECK ADD FOREIGN KEY([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders] WITH CHECK ADD FOREIGN KEY([ProductID]) REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Orders] WITH CHECK ADD FOREIGN KEY([ShippingID]) REFERENCES [dbo].[Shipping] ([ShippingID])
GO
ALTER TABLE [dbo].[Orders] WITH CHECK ADD FOREIGN KEY([SupplierID]) REFERENCES [dbo].[Suppliers] ([SupplierID])
GO
ALTER TABLE [dbo].[Orders] WITH CHECK ADD FOREIGN KEY([UserID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Products] WITH CHECK ADD FOREIGN KEY([CategoryID]) REFERENCES [dbo].[Categories] ([CategoryID])
GO

-- T?o các ràng bu?c CHECK
ALTER TABLE [dbo].[InventoryHistory] WITH CHECK ADD CHECK (([ChangeType]=(3) OR [ChangeType]=(2) OR [ChangeType]=(1)))
GO
ALTER TABLE [dbo].[Orders] WITH CHECK ADD CHECK (([OrderType]=(2) OR [OrderType]=(1)))
GO
ALTER TABLE [dbo].[Orders] WITH CHECK ADD CHECK (([Status]=(2) OR [Status]=(1)))
GO
ALTER TABLE [dbo].[Shipping] WITH CHECK ADD CHECK (([Status]=(3) OR [Status]=(2) OR [Status]=(1)))
GO
ALTER TABLE [dbo].[Users] WITH CHECK ADD CHECK (([Role]=(2) OR [Role]=(1))) -- Ch? có 2 vai trò: Ch? kho (1), Nhân viên (2)
GO
ALTER TABLE [dbo].[Users] WITH CHECK ADD FOREIGN KEY([Role]) REFERENCES [dbo].[Roles] ([RoleID])
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, N'Ch? kho', GETDATE(), NULL),
(2, N'Nhân viên', GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([CategoryID], [Name], [Description], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, N'Ði?n t?', N'Các s?n ph?m di?n t?', GETDATE(), NULL),
(2, N'Th?i trang', N'Qu?n áo, giày dép', GETDATE(), NULL),
(3, N'Th?c ph?m', N'Th?c ph?m và d? u?ng', GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 
GO
INSERT [dbo].[Customers] ([CustomerID], [FullName], [Phone], [Email], [Address], [Note], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, N'Nguyen Van D', N'0912345678', N'nguyend@example.com', N'Hanoi, Vietnam', N'Khách hàng thân thi?t', GETDATE(), NULL),
(2, N'Tran Thi E', N'0938765432', N'trane@example.com', N'HCMC, Vietnam', NULL, GETDATE(), NULL),
(3, N'Le Van Kien', N'0988489099', N'lekienhg24@gmail.com', N'Ha Giang', N'Khách VIP', GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 
GO
INSERT [dbo].[Suppliers] ([SupplierID], [Name], [Phone], [Email], [Address], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, N'Công ty ABC', N'0241234567', N'abc@example.com', N'Hanoi, Vietnam', GETDATE(), NULL),
(2, N'Công ty XYZ', N'0289876543', N'xyz@example.com', N'HCMC, Vietnam', GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [Username], [Password], [FullName], [Phone], [Email], [Address], [Role], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, N'admin', N'password123', N'Nguyen Van A', N'0987654321', N'admin@example.com', N'Hanoi, Vietnam', 1, GETDATE(), NULL), -- Ch? kho
(2, N'employee1', N'password123', N'Tran Thi B', N'0971234567', N'employee1@example.com', N'HCMC, Vietnam', 2, GETDATE(), NULL), -- Nhân viên
(3, N'employee2', N'password123', N'Le Van C', N'0969876543', N'employee2@example.com', N'Da Nang, Vietnam', 2, GETDATE(), NULL); -- Nhân viên
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([ProductID], [Name], [Description], [Images], [Unit], [Quantity], [AvailableQuantity], [Price], [CostPrice], [CategoryID], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, N'Laptop Dell', N'Laptop van phòng', 'https://surfacecity.vn/wp-content/uploads/L715BL-front-500x500.jpg', N'Cái', 50, 45, CAST(15000000.00 AS Decimal(10, 2)), CAST(12000000.00 AS Decimal(10, 2)), 1, GETDATE(), NULL),
(2, N'Áo so mi', N'Áo so mi nam', 'https://bizweb.dktcdn.net/thumb/compact/100/449/458/products/thiet-ke-chua-co-ten-45-810dc05b-2575-4385-a16c-0c875869b54d.jpg?v=1690532320407', N'Cái', 100, 95, CAST(300000.00 AS Decimal(10, 2)), CAST(200000.00 AS Decimal(10, 2)), 2, GETDATE(), NULL),
(3, N'Bánh quy', N'Bánh quy nh?p kh?u', 'https://cdn.shopify.com/s/files/1/0563/5745/4002/products/8_3.jpg?v=1623398241', N'H?p', 200, 190, CAST(50000.00 AS Decimal(10, 2)), CAST(30000.00 AS Decimal(10, 2)), 3, GETDATE(), NULL),
(4, N'Máy tính b?ng Samsung', N'Máy tính b?ng cao c?p', 'https://th.bing.com/th/id/OIP.BSyAfHvunVKkAYCtGEa-EwHaEt?rs=1&pid=ImgDetMain', N'Cái', 30, 28, CAST(8000000.00 AS Decimal(10, 2)), CAST(6000000.00 AS Decimal(10, 2)), 1, GETDATE(), NULL),
(5, N'Áo thun', N'Áo thun th? thao', 'https://th.bing.com/th/id/R.425c773d0d6298e9f6ac53a8f6921afe?rik=AEPF%2b8npDf9SYw&pid=ImgRaw&r=0', N'Cái', 150, 145, CAST(200000.00 AS Decimal(10, 2)), CAST(150000.00 AS Decimal(10, 2)), 2, GETDATE(), NULL),
(6, N'K?o d?o', N'K?o d?o nh?p kh?u', 'https://th.bing.com/th/id/OIP.7-Pi-AicnJxFscKraBn_AAHaHa?rs=1&pid=ImgDetMain', N'Gói', 300, 290, CAST(30000.00 AS Decimal(10, 2)), CAST(20000.00 AS Decimal(10, 2)), 3, GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Shipping] ON 
GO
INSERT [dbo].[Shipping] ([ShippingID], [Carrier], [ShippingCost], [ShippingDate], [EstimatedDeliveryDate], [Status], [TrackingNumber], [Note], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, N'Viettel Post', CAST(50000.00 AS Decimal(10, 2)), CAST(N'2025-02-25T00:00:00.000' AS DateTime), CAST(N'2025-03-01T00:00:00.000' AS DateTime), 1, N'VT12345', NULL, GETDATE(), NULL),
(2, N'GHN', CAST(60000.00 AS Decimal(10, 2)), CAST(N'2025-02-26T00:00:00.000' AS DateTime), CAST(N'2025-03-02T00:00:00.000' AS DateTime), 2, N'GHN67890', N'Giao hàng nhanh', GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[Shipping] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 
GO
INSERT [dbo].[Orders] ([OrderID], [ProductID], [CustomerID], [SupplierID], [Quantity], [UnitPrice], [TotalPrice], [OrderDate], [OrderType], [Status], [UserID], [ShippingID], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, 1, 1, NULL, 2, CAST(15000000.00 AS Decimal(10, 2)), CAST(30000000.00 AS Decimal(10, 2)), CAST(N'2025-02-25T00:00:00.000' AS DateTime), 2, 1, 1, 1, GETDATE(), NULL),
(2, 2, NULL, 2, 50, CAST(200000.00 AS Decimal(10, 2)), CAST(10000000.00 AS Decimal(10, 2)), CAST(N'2025-02-26T00:00:00.000' AS DateTime), 1, 2, 2, NULL, GETDATE(), NULL),
(3, 1, 1, 2, 50, CAST(200000.00 AS Decimal(10, 2)), CAST(10000000.00 AS Decimal(10, 2)), CAST(N'2025-03-02T03:31:51.023' AS DateTime), 1, 2, 2, 1, GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[InventoryQuota] ON 
GO
INSERT [dbo].[InventoryQuota] ([QuotaID], [ProductID], [Quantity], [Length], [Width], [Height], [Note], [CreatedDate], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, 1, 10, CAST(30.50 AS Decimal(5, 2)), CAST(20.00 AS Decimal(5, 2)), CAST(2.50 AS Decimal(5, 2)), N'Hàng trung bày', CAST(N'2025-02-20T00:00:00.000' AS DateTime), GETDATE(), NULL),
(2, 2, 20, CAST(10.00 AS Decimal(5, 2)), CAST(5.00 AS Decimal(5, 2)), CAST(1.00 AS Decimal(5, 2)), NULL, CAST(N'2025-02-21T00:00:00.000' AS DateTime), GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[InventoryQuota] OFF
GO
SET IDENTITY_INSERT [dbo].[InventoryHistory] ON 
GO
INSERT [dbo].[InventoryHistory] ([HistoryID], [ProductID], [OrderID], [ChangeType], [QuantityChanged], [PreviousQuantity], [NewQuantity], [ChangeDate], [UserID], [Note], [CreatedAt], [UpdatedAt]) 
VALUES 
(1, 1, 1, 2, 2, 50, 48, CAST(N'2025-02-25T00:00:00.000' AS DateTime), 1, N'Bán hàng', GETDATE(), NULL),
(2, 2, 2, 1, 50, 100, 150, CAST(N'2025-02-26T00:00:00.000' AS DateTime), 2, N'Nh?p hàng t? nhà cung c?p', GETDATE(), NULL);
SET IDENTITY_INSERT [dbo].[InventoryHistory] OFF
GO