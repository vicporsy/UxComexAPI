use[GLOBAL]

DECLARE @Nomes TABLE (Nome VARCHAR(100));
INSERT INTO @Nomes (Nome) VALUES 
('Ana'), ('Bruno'), ('Camila'), ('Daniel'), ('Eduarda'),
('Felipe'), ('Gabriela'), ('Henrique'), ('Isabela'), ('João'),
('Karla'), ('Lucas'), ('Mariana'), ('Nicolas'), ('Olívia'),('Victor'),
('Enzo'),('Roberto'),('Paulo'),('Jennifer'),('Maria'),('Paula'),('Laislla'),
('Giovanna'),('Eduardo'),('Felipe'),('Gabriel'),('Sergio'),('Jane'),('Rodrigo'),
('Gilberto'),('Humberto'),('Adalberto'),('Edgard'),('Flavio'),('Fabio'),('Pedro'),
('Giulia'),('Nathalia'),('Natan'),('Bruce'),('Bill'),('Bob'),('William'),
('Paulo'), ('Quésia'), ('Rafael'), ('Sabrina'), ('Thiago');

DECLARE @Sobrenomes TABLE (Sobrenome VARCHAR(100));
INSERT INTO @Sobrenomes (Sobrenome) VALUES 
('Silva'), ('Souza'), ('Oliveira'), ('Lima'), ('Costa'),
('Mendes'), ('Rocha'), ('Ferreira'), ('Almeida'), ('Teixeira'),
('Barros'), ('Lopes'), ('Ramos'), ('Pires'), ('Henrique'),
('Dias'), ('Monteiro'), ('Faria'), ('Brito'), ('Martins');

-- Lista de domínios de e-mail
DECLARE @Dominios TABLE (Dominio VARCHAR(100));
INSERT INTO @Dominios (Dominio) VALUES 
('gmail.com'), ('hotmail.com'), ('outlook.com'), 
('yahoo.com'), ('example.com');


DECLARE @i INT = 1;
WHILE @i <= 1000
BEGIN
    DECLARE @Nome VARCHAR(100), @Sobrenome VARCHAR(100), @Email VARCHAR(200), @Phone VARCHAR(20);

    -- Seleciona nome e sobrenome aleatórios
    SELECT TOP 1 @Nome = Nome FROM @Nomes ORDER BY NEWID();
    SELECT TOP 1 @Sobrenome = Sobrenome FROM @Sobrenomes ORDER BY NEWID();
    
    -- Cria e-mail: nome.sobrenome@dominio
    SELECT TOP 1 @Email = LOWER(@Nome + '.' + @Sobrenome + '@' + Dominio) FROM @Dominios ORDER BY NEWID();

    -- Gera telefone aleatório
    SET @Phone = '(' + CAST((10 + ABS(CHECKSUM(NEWID())) % 89) AS VARCHAR(2)) + ') ' +
                 CAST((90000 + ABS(CHECKSUM(NEWID())) % 10000) AS VARCHAR(5)) + '-' +
                 CAST((1000 + ABS(CHECKSUM(NEWID())) % 9000) AS VARCHAR(4));

    -- Insere na tabela CLIENT
    INSERT INTO CLIENT (Name, Email, Phone)
    VALUES (@Nome + ' ' + @Sobrenome, @Email, @Phone);

    SET @i += 1;
END