USE pharmacyDB;
GO


IF OBJECT_ID('dbo.SaleItems','U') IS NOT NULL DROP TABLE dbo.SaleItems;
IF OBJECT_ID('dbo.Sales','U') IS NOT NULL DROP TABLE dbo.Sales;

IF OBJECT_ID('dbo.PurchaseItems','U') IS NOT NULL DROP TABLE dbo.PurchaseItems;
IF OBJECT_ID('dbo.Purchases','U') IS NOT NULL DROP TABLE dbo.Purchases;

IF OBJECT_ID('dbo.MedicineBatches','U') IS NOT NULL DROP TABLE dbo.MedicineBatches;
IF OBJECT_ID('dbo.Medicines','U') IS NOT NULL DROP TABLE dbo.Medicines;
IF OBJECT_ID('dbo.Categories','U') IS NOT NULL DROP TABLE dbo.Categories;

IF OBJECT_ID('dbo.Suppliers','U') IS NOT NULL DROP TABLE dbo.Suppliers;

IF OBJECT_ID('dbo.Users','U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Roles','U') IS NOT NULL DROP TABLE dbo.Roles;
GO


CREATE TABLE dbo.Roles
(
  RoleId   INT IDENTITY(1,1) PRIMARY KEY,
  RoleName NVARCHAR(50) NOT NULL UNIQUE
);
GO


CREATE TABLE dbo.Users
(
  UserId    INT IDENTITY(1,1) PRIMARY KEY,
  Username  NVARCHAR(50) NOT NULL UNIQUE,
  [Password] NVARCHAR(200) NOT NULL,
  FullName  NVARCHAR(100) NOT NULL,
  RoleId    INT NOT NULL,
  IsActive  BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT(1),
  CONSTRAINT FK_Users_Roles FOREIGN KEY(RoleId) REFERENCES dbo.Roles(RoleId)
);
GO


CREATE TABLE dbo.Categories
(
  CategoryId   INT IDENTITY(1,1) PRIMARY KEY,
  CategoryName NVARCHAR(100) NOT NULL UNIQUE
);
GO


CREATE TABLE dbo.Medicines
(
  MedicineId   INT IDENTITY(1,1) PRIMARY KEY,
  MedicineName NVARCHAR(150) NOT NULL,
  GenericName  NVARCHAR(150) NULL,
  CategoryId   INT NOT NULL,
  ReorderLevel INT NOT NULL CONSTRAINT DF_Medicines_Reorder DEFAULT(10),
  IsActive     BIT NOT NULL CONSTRAINT DF_Medicines_IsActive DEFAULT(1),

  CONSTRAINT FK_Medicines_Categories FOREIGN KEY(CategoryId) REFERENCES dbo.Categories(CategoryId)
);
GO


CREATE TABLE dbo.MedicineBatches
(
  BatchId         INT IDENTITY(1,1) PRIMARY KEY,
  MedicineId      INT NOT NULL,
  BatchNo         NVARCHAR(80) NOT NULL,
  ExpiryDate      DATE NOT NULL,
  PurchasePrice   DECIMAL(18,2) NOT NULL,
  SalePrice       DECIMAL(18,2) NOT NULL,
  CurrentStockQty INT NOT NULL CONSTRAINT DF_Batches_Stock DEFAULT(0),

  CONSTRAINT FK_Batches_Medicines FOREIGN KEY(MedicineId) REFERENCES dbo.Medicines(MedicineId),
  CONSTRAINT UQ_Batches_Medicine_BatchNo UNIQUE(MedicineId, BatchNo)
);
GO


CREATE TABLE dbo.Suppliers
(
  SupplierId   INT IDENTITY(1,1) PRIMARY KEY,
  SupplierName NVARCHAR(150) NOT NULL,
  Phone        NVARCHAR(30) NULL,
  [Address]    NVARCHAR(250) NULL
);
GO


CREATE TABLE dbo.Purchases
(
  PurchaseId   INT IDENTITY(1,1) PRIMARY KEY,
  SupplierId   INT NOT NULL,
  PurchaseDate DATETIME NOT NULL CONSTRAINT DF_Purchases_Date DEFAULT(GETDATE()),
  CreatedBy    INT NOT NULL,
  TotalAmount  DECIMAL(18,2) NOT NULL CONSTRAINT DF_Purchases_Total DEFAULT(0),

  CONSTRAINT FK_Purchases_Suppliers FOREIGN KEY(SupplierId) REFERENCES dbo.Suppliers(SupplierId),
  CONSTRAINT FK_Purchases_Users FOREIGN KEY(CreatedBy) REFERENCES dbo.Users(UserId)
);
GO


CREATE TABLE dbo.PurchaseItems
(
  PurchaseItemId INT IDENTITY(1,1) PRIMARY KEY,
  PurchaseId     INT NOT NULL,
  BatchId        INT NOT NULL,
  Quantity       INT NOT NULL,
  UnitCost       DECIMAL(18,2) NOT NULL,
  LineTotal      AS (CONVERT(DECIMAL(18,2), Quantity * UnitCost)) PERSISTED,

  CONSTRAINT FK_PurchaseItems_Purchases FOREIGN KEY(PurchaseId) REFERENCES dbo.Purchases(PurchaseId),
  CONSTRAINT FK_PurchaseItems_Batches FOREIGN KEY(BatchId) REFERENCES dbo.MedicineBatches(BatchId)
);
GO


CREATE TABLE dbo.Sales
(
  SaleId     INT IDENTITY(1,1) PRIMARY KEY,
  SaleDate   DATETIME NOT NULL CONSTRAINT DF_Sales_Date DEFAULT(GETDATE()),
  SoldBy     INT NOT NULL,
  SubTotal   DECIMAL(18,2) NOT NULL CONSTRAINT DF_Sales_Sub DEFAULT(0),
  Discount   DECIMAL(18,2) NOT NULL CONSTRAINT DF_Sales_Discount DEFAULT(0),
  VAT        DECIMAL(18,2) NOT NULL CONSTRAINT DF_Sales_VAT DEFAULT(0),
  NetTotal   DECIMAL(18,2) NOT NULL CONSTRAINT DF_Sales_Net DEFAULT(0),

  CONSTRAINT FK_Sales_Users FOREIGN KEY(SoldBy) REFERENCES dbo.Users(UserId)
);
GO


CREATE TABLE dbo.SaleItems
(
  SaleItemId INT IDENTITY(1,1) PRIMARY KEY,
  SaleId     INT NOT NULL,
  BatchId    INT NOT NULL,
  Quantity   INT NOT NULL,
  UnitPrice  DECIMAL(18,2) NOT NULL,
  LineTotal  AS (CONVERT(DECIMAL(18,2), Quantity * UnitPrice)) PERSISTED,

  CONSTRAINT FK_SaleItems_Sales FOREIGN KEY(SaleId) REFERENCES dbo.Sales(SaleId),
  CONSTRAINT FK_SaleItems_Batches FOREIGN KEY(BatchId) REFERENCES dbo.MedicineBatches(BatchId)
);
GO
