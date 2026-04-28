USE YGOShopDB;
GO

CREATE TABLE Users (
    User_ID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(60) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Saldo FLOAT DEFAULT 0
);

CREATE TABLE Cards (
    Card_ID INT PRIMARY KEY,               -- ID vindo da API (não é identity)
    Nome VARCHAR(600) NOT NULL,
    Texto VARCHAR(MAX) NOT NULL,
    Level INT NULL,
    Attribute VARCHAR(20) NULL,
    MonsterType VARCHAR(40) NULL,
    CardCategory VARCHAR(40) NOT NULL,      -- Monster / Spell / Trap
    Scales INT NULL,
    PendulumText VARCHAR(MAX) NULL,
    Ataque INT NULL,
    Defesa INT NULL
);

CREATE TABLE CardImages (
    Image_ID INT IDENTITY(1,1) PRIMARY KEY,
    Card_ID INT NOT NULL,
    Image_URL VARCHAR(500) NOT NULL,
    Image_Small_URL VARCHAR(500) NULL,
    Image_Cropped_URL VARCHAR(500) NULL,
    CONSTRAINT FK_CardImages_Cards
        FOREIGN KEY (Card_ID)
        REFERENCES Cards(Card_ID)
        ON DELETE CASCADE
);

CREATE TABLE Collection (
    Collection_ID INT IDENTITY(1,1) PRIMARY KEY,
    User_ID INT NOT NULL,
    Card_ID INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Collection_Users
        FOREIGN KEY (User_ID)
        REFERENCES Users(User_ID)
        ON DELETE CASCADE,

    CONSTRAINT FK_Collection_Cards
        FOREIGN KEY (Card_ID)
        REFERENCES Cards(Card_ID)
        ON DELETE CASCADE,

    CONSTRAINT UQ_User_Card UNIQUE (User_ID, Card_ID)
);

CREATE TABLE Vendas (
    Vendas_ID INT IDENTITY(1,1) PRIMARY KEY,
    User_ID INT NOT NULL,
    Card_ID INT NOT NULL,
    Price FLOAT NOT NULL,
    Copies INT NOT NULL,

    CONSTRAINT FK_Vendas_Users
        FOREIGN KEY (User_ID)
        REFERENCES Users(User_ID)
        ON DELETE CASCADE,

    CONSTRAINT FK_Vendas_Cards
        FOREIGN KEY (Card_ID)
        REFERENCES Cards(Card_ID)
        ON DELETE CASCADE
);

CREATE INDEX IDX_Cards_Nome ON Cards(Nome);
CREATE INDEX IDX_Collection_User ON Collection(User_ID);
CREATE INDEX IDX_Vendas_Card ON Vendas(Card_ID);

