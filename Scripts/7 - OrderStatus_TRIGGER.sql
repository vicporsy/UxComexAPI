USE [GLOBAL]
GO

DROP TABLE IF EXISTS GLOBAL_LOG..OrdersStatusLog
GO
CREATE TABLE GLOBAL_LOG..OrdersStatusLog (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [OrderId] INT NOT NULL,
    [OldStatus] INT,
    [NewStatus] INT,
    [ChangedAt] DATETIME DEFAULT GETDATE(),
    [ChangedBy] NVARCHAR(128) DEFAULT SYSTEM_USER
);
GO

CREATE INDEX IDX_ORDERID_CHANGEDAT ON GLOBAL_LOG..OrdersStatusLog(OrderId, ChangedAt DESC);
GO

CREATE OR ALTER TRIGGER TRG_ORDERS_STATUSCHANGE
ON GLOBAL.DBO.ORDERS
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Inserir registro de log apenas quando o Status foi alterado
    INSERT INTO GLOBAL_LOG..OrdersStatusLog (OrderId, OldStatus, NewStatus, ChangedAt, ChangedBy)
    SELECT 
        i.Id,
        d.Status AS OldStatus,
        i.Status AS NewStatus,
        GETDATE(),
        SYSTEM_USER
    FROM inserted i
    INNER JOIN deleted d ON i.Id = d.Id
    WHERE ISNULL(i.Status, -1) <> ISNULL(d.Status, -1); 
END
GO