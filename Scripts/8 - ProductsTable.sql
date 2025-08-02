USE[GLOBAL]

DROP TYPE IF EXISTS ProductsTable;
GO

CREATE TYPE ProductsTable AS TABLE
(
    OrderId INT,
    ProductId INT,
    Quantity INT,
    Price DECIMAL(18,2)
);
GO