create database dailypim 
COLLATE Latin1_General_100_CI_AS_SC_UTF8;

use dailypim

DROP TABLE IF EXISTS KeyWords;
DROP TABLE IF EXISTS Medias;
DROP TABLE IF EXISTS Items;
DROP TABLE IF EXISTS Volumes;
DROP TABLE IF EXISTS Tax;
DROP TABLE IF EXISTS Prices;
DROP TABLE IF EXISTS Providers;
DROP TABLE IF EXISTS Descriptive;
DROP TABLE IF EXISTS Users;


Create table Users(
idUser int identity(1,1),
CodeUser varchar(12) primary key,
NameUser varchar(50), 
LastNameUser varchar(50), 
Username varchar(30),
mailUser varchar(30), 
PasswordUser varchar(20),
)

 

create table Descriptive(
IdDescriptive int identity(1,1),
CodeDescriptive varchar(12) primary key,
CodeISO varchar(5),
DescriptiveShort nvarchar(max),
DescriptiveLong varchar(max)
)

 

create table Providers(
IdProvider int identity(1,1),
CodeProvider varchar(12) primary key,
BusinessName varchar(30),
Address varchar(20),
City varchar(20),
CodePostal varchar(10),
PhoneNumber varchar(10),
mail varchar(50),
CodeUser varchar(12) references Users(CodeUser)
)

 

create table Prices(
idPrice int identity(1,1),
CodePrice varchar(12) primary key, 
Currency varchar(20), 
PriceWT float,
QtyMinimal int, 
)


create table Tax(
idTax int identity(1,1),
CodeTax varchar(12) primary key,
Rate float,
)

 

Create table Volumes(
idVolume int identity(1,1),
CodeVolume varchar(12) primary key,
Descriptive varchar(max),
Weights float,
Dimensions varchar(20)
)


create table Items(
IdItem int identity(1,1),
CodeItem varchar(12) primary key,
CodeUniversel varchar(max),
WeightItem float,
OrigineItem varchar(10),
UniteVenteItem varchar(12),
DeclinationItem varchar(12) references Items(CodeItem),
CodeProvider varchar(12) references Providers(CodeProvider),
CodeTarif varchar(12) references Prices(CodePrice),
CodeTax varchar(12) references Tax(CodeTax),
CodeVolume varchar(12) references Volumes(CodeVolume),
CodeDescriptive varchar(12) references Descriptive(CodeDescriptive)
)


create table Medias(
IdMedias int identity(1,1) primary key,
CodeMedias varchar(12),
TypeMedias varchar(10),
FileMedias varchar(20),
CodeItem varchar(12) references Items(CodeItem)
)

 
create table KeyWords(
Id int identity(1,1) primary key,
CodeKeyWord varchar(max),
CodeItem varchar(12) references Items(CodeItem),
IdMedias int references Medias(IdMedias)
)

