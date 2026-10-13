-- Run against AgriTechDB if Orders table is missing
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        OrderID INT IDENTITY(1,1) PRIMARY KEY,
        BuyerID INT NOT NULL,
        SellerID INT NOT NULL,
        ItemType NVARCHAR(50) NOT NULL,
        ItemID INT NOT NULL,
        ItemName NVARCHAR(200) NOT NULL,
        ContactNumber NVARCHAR(20) NULL,
        OrderStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
        OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
        RespondedDate DATETIME NULL
    );
END
GO
