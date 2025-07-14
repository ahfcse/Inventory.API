USE [master]
GO
/****** Object:  Database [InventoryDB]    Script Date: 7/15/2025 1:24:17 AM ******/
CREATE DATABASE [InventoryDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InventoryDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\InventoryDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'InventoryDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\InventoryDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [InventoryDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InventoryDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InventoryDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InventoryDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InventoryDB] SET ANSI_PADDING OFF 

GO
ALTER DATABASE [InventoryDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [InventoryDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [InventoryDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InventoryDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InventoryDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InventoryDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InventoryDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InventoryDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InventoryDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InventoryDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InventoryDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [InventoryDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InventoryDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InventoryDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InventoryDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InventoryDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InventoryDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [InventoryDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InventoryDB] SET RECOVERY FULL 
GO
ALTER DATABASE [InventoryDB] SET  MULTI_USER 
GO
ALTER DATABASE [InventoryDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InventoryDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InventoryDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InventoryDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [InventoryDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [InventoryDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'InventoryDB', N'ON'
GO
ALTER DATABASE [InventoryDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [InventoryDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [InventoryDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 7/15/2025 1:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

GO
CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[LoyaltyPoints] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 7/15/2025 1:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Barcode] [nvarchar](max) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[StockQty] [decimal](18, 2) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SaleDetails]    Script Date: 7/15/2025 1:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleDetails](
	[SaleDetailId] [int] IDENTITY(1,1) NOT NULL,
	[SaleId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_SaleDetails] PRIMARY KEY CLUSTERED 
(
	[SaleDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 7/15/2025 1:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[SaleId] [int] IDENTITY(1,1) NOT NULL,
	[SaleDate] [datetime2](7) NOT NULL,
	[CustomerId] [int] NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[DueAmount] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[DiscountPercentage] [decimal](18, 2) NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[VatAmount] [decimal](18, 2) NOT NULL,
	[VatPercentage] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[SaleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/15/2025 1:24:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([CustomerId], [FullName], [Phone], [Email], [LoyaltyPoints], [IsDeleted]) VALUES (1, N'Akram', N'01612861021', N'ak@gmail.com', 200, 1)
INSERT [dbo].[Customers] ([CustomerId], [FullName], [Phone], [Email], [LoyaltyPoints], [IsDeleted]) VALUES (2, N'Jannat', N'64654654654', N'j@gmail.com', 100, 0)
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductId], [Name], [Barcode], [Price], [StockQty], [Category], [Status], [IsDeleted]) VALUES (1, N'Iphone 15', N'420', CAST(45000.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), N'Mobile', 0, 1)
INSERT [dbo].[Products] ([ProductId], [Name], [Barcode], [Price], [StockQty], [Category], [Status], [IsDeleted]) VALUES (2, N'Iphone 16 Pro', N'550', CAST(150000.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), N'Mobile', 1, 1)
INSERT [dbo].[Products] ([ProductId], [Name], [Barcode], [Price], [StockQty], [Category], [Status], [IsDeleted]) VALUES (3, N'Iphone 16 Pro max', N'5505', CAST(150000.00 AS Decimal(18, 2)), CAST(2.00 AS Decimal(18, 2)), N'Mobile', 0, 1)
INSERT [dbo].[Products] ([ProductId], [Name], [Barcode], [Price], [StockQty], [Category], [Status], [IsDeleted]) VALUES (4, N'Lenovo Slim Pad', N'888', CAST(150000.00 AS Decimal(18, 2)), CAST(99.00 AS Decimal(18, 2)), N'Laptop', 1, 0)
INSERT [dbo].[Products] ([ProductId], [Name], [Barcode], [Price], [StockQty], [Category], [Status], [IsDeleted]) VALUES (5, N'Wireless Mouse', N'MOUSE123', CAST(24.99 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), N'Computer Accessories', 1, 0)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[SaleDetails] ON 

INSERT [dbo].[SaleDetails] ([SaleDetailId], [SaleId], [ProductId], [Quantity], [Price]) VALUES (1, 1, 4, CAST(1.00 AS Decimal(18, 2)), CAST(150000.00 AS Decimal(18, 2)))
INSERT [dbo].[SaleDetails] ([SaleDetailId], [SaleId], [ProductId], [Quantity], [Price]) VALUES (2, 2, 4, CAST(1.00 AS Decimal(18, 2)), CAST(120000.00 AS Decimal(18, 2)))
INSERT [dbo].[SaleDetails] ([SaleDetailId], [SaleId], [ProductId], [Quantity], [Price]) VALUES (3, 3, 1, CAST(2.00 AS Decimal(18, 2)), CAST(24.99 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[SaleDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Sales] ON 

INSERT [dbo].[Sales] ([SaleId], [SaleDate], [CustomerId], [TotalAmount], [PaidAmount], [DueAmount], [DiscountAmount], [DiscountPercentage], [SubTotal], [VatAmount], [VatPercentage]) VALUES (1, CAST(N'2025-07-14T14:47:21.5922210' AS DateTime2), 2, CAST(150000.00 AS Decimal(18, 2)), CAST(150000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Sales] ([SaleId], [SaleDate], [CustomerId], [TotalAmount], [PaidAmount], [DueAmount], [DiscountAmount], [DiscountPercentage], [SubTotal], [VatAmount], [VatPercentage]) VALUES (2, CAST(N'2025-07-14T17:21:46.3784017' AS DateTime2), 2, CAST(120000.00 AS Decimal(18, 2)), CAST(103500.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(120000.00 AS Decimal(18, 2)), CAST(16200.00 AS Decimal(18, 2)), CAST(15.00 AS Decimal(18, 2)))
INSERT [dbo].[Sales] ([SaleId], [SaleDate], [CustomerId], [TotalAmount], [PaidAmount], [DueAmount], [DiscountAmount], [DiscountPercentage], [SubTotal], [VatAmount], [VatPercentage]) VALUES (3, CAST(N'2025-07-14T18:31:02.3839651' AS DateTime2), 1, CAST(49.98 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(-50.02 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(49.98 AS Decimal(18, 2)), CAST(3.60 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Sales] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Email], [Role], [IsDeleted]) VALUES (1, N'admin', N'240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', N'admin@inventory.com', N'Admin', 0)
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Email], [Role], [IsDeleted]) VALUES (2, N'Foysal', N'9e53dc32ed6c93deb6b3e308e5a98d8d3d8387d6433f519ce43fed17fed8dedd', N'F@example.com', N'User', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_SaleDetails_SaleId]    Script Date: 7/15/2025 1:24:18 AM ******/
CREATE NONCLUSTERED INDEX [IX_SaleDetails_SaleId] ON [dbo].[SaleDetails]
(
	[SaleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users_Username]    Script Date: 7/15/2025 1:24:18 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Username] ON [dbo].[Users]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0.0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0.0)) FOR [DiscountPercentage]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0.0)) FOR [SubTotal]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0.0)) FOR [VatAmount]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0.0)) FOR [VatPercentage]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'User') FOR [Role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SaleDetails]  WITH CHECK ADD  CONSTRAINT [FK_SaleDetails_Sales_SaleId] FOREIGN KEY([SaleId])
REFERENCES [dbo].[Sales] ([SaleId])
GO
ALTER TABLE [dbo].[SaleDetails] CHECK CONSTRAINT [FK_SaleDetails_Sales_SaleId]
GO
USE [master]
GO
ALTER DATABASE [InventoryDB] SET  READ_WRITE 
GO
