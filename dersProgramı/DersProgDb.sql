CREATE DATABASE DersProgDb;
GO

USE DersProgDb;
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE dbo.Users(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(10) NOT NULL
);
GO

CREATE TABLE dbo.Classes(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId INT NULL,
    Name NVARCHAR(50) NOT NULL,
    Capacity INT NULL,  -- Kapasite sayı olmalı
    CONSTRAINT FK_Classes_Users FOREIGN KEY(UserId) REFERENCES dbo.Users(Id)
);
GO

CREATE TABLE dbo.Departmans(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId INT NULL,
    Name NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Departmans_Users FOREIGN KEY(UserId) REFERENCES dbo.Users(Id)
);
GO

CREATE TABLE dbo.Subjects(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    UserId INT NOT NULL,
    Akts INT NULL,
    Saat INT NULL,
    CONSTRAINT FK_Subjects_Users FOREIGN KEY(UserId) REFERENCES dbo.Users(Id)
);
GO

-- Veri ekleme için IDENTITY_INSERT kullanmak gerekiyor, dikkat edin:

SET IDENTITY_INSERT dbo.Users ON;
INSERT INTO dbo.Users (Id, Username, Password) VALUES
(3, N'ek', N'123'),
(5, N'BatuhanMedetoglu', N'1234'),
(6, N'FatmaMumcuKucukcayli', N'1234'),
(7, N'HasanKebapci', N'1234'),
(8, N'KubilayYanik', N'1234'),
(9, N'EdaTelli', N'1234'),
(10, N'AtiyeAynaliAkdogan', N'1234'),
(11, N'OzgurDuden', N'1234'),
(12, N'ZelihaKaramanOkay', N'1234'),
(13, N'AyseTurkmen', N'1234'),
(14, N'MehtapAkkaya', N'1234'),
(15, N'OzlemKaplan', N'1234'),
(16, N'VeliSipahi', N'1234'),
(17, N'OguzhanErdogan', N'1234'),
(18, N'FatihKarabacak', N'1234'),
(19, N'HasanHuseyinUmutlu', N'1234'),
(20, N'ErcumentDogru', N'1234'),
(21, N'DeryaAgcadag', N'1234'),
(22, N'RanaIcmen', N'1234'),
(23, N'EkremEsrefKilinc', N'1234'),
(24, N'EmrahBeydilli', N'1234'),
(25, N'MustafaCatak', N'1234'),
(26, N'FatmaFeyzaKaya', N'1234'),
(27, N'SamiUlukus', N'1234'),
(28, N'TulayTuran', N'1234'),
(29, N'AliInanir', N'1234'),
(30, N'Test', N'1234'),
(31, N'CihanBicer', N'1234');
SET IDENTITY_INSERT dbo.Users OFF;
GO

SET IDENTITY_INSERT dbo.Classes ON;
INSERT INTO dbo.Classes (Id, UserId, Name, Capacity) VALUES
(4, 3, N'sınıf', 20),
(5, 20, N'201', 40),
(6, 9, N'202', 45),
(7, 18, N'206', 40),
(8, 22, N'101', 45),
(9, 18, N'102', 40),
(10, 23, N'103', 40),
(11, 9, N'104', 40);
SET IDENTITY_INSERT dbo.Classes OFF;
GO

SET IDENTITY_INSERT dbo.Departmans ON;
INSERT INTO dbo.Departmans (Id, UserId, Name) VALUES
(1, 3, N'bilgisayar Programcılığı'),
(2, 5, N'Test'),
(3, 9, N'Maliye'),
(4, 6, N'Büro Yönetimi ve Yönetici Asistanlığı'),
(5, 11, N'Posta Hizmetleri Programı'),
(6, 23, N'Bilişim Güvenliği Teknolojisi Programı'),
(7, 17, N'Turizm ve Seyahat Hizmetleri Programı'),
(8, 25, N'Ağ Sistemleri'),
(9, 25, N'İşletim Sistemleri'),
(10, 3, N'Yazılım Proje Değerlendirme'),
(11, 24, N'İleri Web Tasarım'),
(12, 24, N'Web Tasarım');
SET IDENTITY_INSERT dbo.Departmans OFF;
GO

SET IDENTITY_INSERT dbo.Subjects ON;
INSERT INTO dbo.Subjects (Id, Name, UserId, Akts, Saat) VALUES
(29, N'Mobil Programlama', 3, 4, 2),
(30, N'Yazılım Geliştirme', 3, 2, 5),
(31, N'Web Proje Geliştirme', 24, 5, 3),
(32, N'Ağ Sistemleri', 25, 3, 1),
(33, N'Elektronik Proje', 25, 6, 4),
(34, N'Makro İktisat', 3, 2, 2),
(35, N'Ticari Matematik', 10, 2, 6),
(36, N'Banka Muhasebesi', 5, 5, 3),
(37, N'Girişimcilik', 5, 1, 1),
(38, N'Kriptoloji', 5, 3, 4),
(39, N'Yapay Zeka Uygulamaları', 12, 6, 2),
(40, N'Mektup Postası', 16, 2, 5),
(41, N'Sermaye Piyasası İşlemleri', 22, 4, 1);
SET IDENTITY_INSERT dbo.Subjects OFF;
GO
