use dailypim	

-- Insertions dans la table Users
INSERT INTO Users (CodeUser, NameUser, LastNameUser, Username, mailUser, PasswordUser)
VALUES ('U001', 'John', 'Doe', 'johndoe', 'johndoe@example.com', 'password1');

INSERT INTO Users (CodeUser, NameUser, LastNameUser, Username, mailUser, PasswordUser)
VALUES ('U002', 'Jane', 'Smith', 'janesmith', 'janesmith@example.com', 'password2');

-- Insertions dans la table Descriptive
INSERT INTO Descriptive (CodeDescriptive, CodeISO, DescriptiveShort, DescriptiveLong)
VALUES ('D001', 'ISO1', N'Short description 1', 'Long description 1');

INSERT INTO Descriptive (CodeDescriptive, CodeISO, DescriptiveShort, DescriptiveLong)
VALUES ('D002', 'ISO2', N'Short description 2', 'Long description 2');

-- Insertions dans la table Providers
INSERT INTO Providers (CodeProvider,BusinessName, Address, City, CodePostal, PhoneNumber, mail, CodeUser)
VALUES ('P001', 'Microsoft' ,'123 Main St', 'New York', '12345', '1234567890', 'provider1@example.com', 'U001');

INSERT INTO Providers (CodeProvider, BusinessName, Address, City, CodePostal, PhoneNumber, mail, CodeUser) 
VALUES ('P002', 'Microsoft' , '456 Elm St', 'Paris','75000', '9876543210', 'provider2@example.com', 'U002');

-- Insertions dans la table Prices
INSERT INTO Prices (CodePrice, Currency, PriceWT, QtyMinimal)
VALUES ('PR001', 'USD', 10.5, 5);

INSERT INTO Prices (CodePrice, Currency, PriceWT, QtyMinimal)
VALUES ('PR002', 'EUR', 8.99, 10);

-- Insertions dans la table Tax
INSERT INTO Tax (CodeTax, Rate)
VALUES ('T001', 0.08);

INSERT INTO Tax (CodeTax, Rate)
VALUES ('T002', 0.1);

-- Insertions dans la table Volumes
INSERT INTO Volumes (CodeVolume, Descriptive, Weights, Dimensions)
VALUES ('V001', 'Volume 1', 2.5, '10x10x10');

INSERT INTO Volumes (CodeVolume, Descriptive, Weights, Dimensions)
VALUES ('V002', 'Volume 2', 5.2, '15x15x15');

-- Insertions dans la table Items
INSERT INTO Items (CodeItem, CodeUniversal, WeightItem, OriginItem, UniteVenteItem, DeclinationItem, CodeProvider, CodePrice, CodeTax, CodeVolume, CodeDescriptive)
VALUES ('I001', 'UNI001', 1.2, 'Origin1', 'Unit1', NULL, 'P001', 'PR001', 'T001', 'V001', 'D001');

INSERT INTO Items (CodeItem, CodeUniversal, WeightItem, OriginItem, UniteVenteItem, DeclinationItem, CodeProvider, CodePrice, CodeTax, CodeVolume, CodeDescriptive)
VALUES ('I002', 'UNI002', 0.8, 'Origin2', 'Unit2', NULL, 'P002', 'PR002', 'T002', 'V002', 'D002');

-- Insertions dans la table Medias
INSERT INTO Medias (CodeMedias, TypeMedias, FileMedias, CodeItem)
VALUES ('M001', 'Type1', 'file1.jpg', 'I001');

INSERT INTO Medias (CodeMedias, TypeMedias, FileMedias, CodeItem)
VALUES ('M002', 'Type2', 'file2.jpg', 'I002');

-- Insertions dans la table KeyWords
INSERT INTO KeyWords (CodeKeyWord, CodeItem, IdMedias)
VALUES ('Keyword1', 'I001', NULL);

INSERT INTO KeyWords (CodeKeyWord, CodeItem, IdMedias)
VALUES ('Keyword2', 'I002', NULL);
