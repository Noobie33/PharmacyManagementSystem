USE pharmacyDB;
GO


IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName='Admin')
  INSERT INTO dbo.Roles(RoleName) VALUES ('Admin');

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName='Pharmacist')
  INSERT INTO dbo.Roles(RoleName) VALUES ('Pharmacist');

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName='Cashier')
  INSERT INTO dbo.Roles(RoleName) VALUES ('Cashier');
GO

DECLARE @AdminRoleId INT = (SELECT RoleId FROM dbo.Roles WHERE RoleName='Admin');
DECLARE @PharmRoleId INT = (SELECT RoleId FROM dbo.Roles WHERE RoleName='Pharmacist');
DECLARE @CashRoleId  INT = (SELECT RoleId FROM dbo.Roles WHERE RoleName='Cashier');


IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username='admin')
  INSERT INTO dbo.Users(Username,[Password],FullName,RoleId,IsActive)
  VALUES ('admin','admin123','System Admin',@AdminRoleId,1);

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username='pharmacist')
  INSERT INTO dbo.Users(Username,[Password],FullName,RoleId,IsActive)
  VALUES ('pharmacist','pharm123','Inventory Pharmacist',@PharmRoleId,1);

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username='cashier')
  INSERT INTO dbo.Users(Username,[Password],FullName,RoleId,IsActive)
  VALUES ('cashier','cash123','Sales Cashier',@CashRoleId,1);
GO
