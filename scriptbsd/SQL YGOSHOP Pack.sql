USE YGOShopDB
GO


CREATE TABLE CardSets (
    CardSet_ID INT IDENTITY(1,1) PRIMARY KEY,
    Card_ID INT NOT NULL,
    Set_Name VARCHAR(100) NOT NULL,
    Set_Code VARCHAR(30),
    Rarity VARCHAR(50),
    Rarity_Code VARCHAR(10),
    Price FLOAT NULL,

    FOREIGN KEY (Card_ID)
        REFERENCES Cards(Card_ID)
        ON DELETE CASCADE
);
