USE YGOShopDB;
GO

CREATE TABLE UserProfileImage (
    User_ID INT NOT NULL PRIMARY KEY,
    Image_ID INT NOT NULL,

    CONSTRAINT FK_UserProfileImage_User
        FOREIGN KEY (User_ID)
        REFERENCES Users(User_ID)
        ON DELETE CASCADE,

    CONSTRAINT FK_UserProfileImage_Image
        FOREIGN KEY (Image_ID)
        REFERENCES CardImages(Image_ID)
);
GO
