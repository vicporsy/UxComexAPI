USE[GLOBAL]

DROP TYPE IF EXISTS ProductStockUpdateType 
                        GO
                        
                        CREATE TYPE ProductStockUpdateType AS TABLE
                        (
                            ProductId INT,
                            Quantity INT
                        );
                        GO