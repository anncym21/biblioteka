CREATE TABLE [dbo].[autorzy]
(
	[id] INT NOT NULL PRIMARY KEY,
	[imie] TEXT NULL,
	[nazwisko] TEXT NULL
)

CREATE TABLE [dbo].[ksiazka]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [tytul] TEXT NULL, 
    [kategoria] TEXT NULL,
	[id_autor] INT NOT NULL FOREIGN KEY (id_autor) REFERENCES dbo.autorzy(id)
)

CREATE TABLE [dbo].[egzemplarze]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [id_ksiazki] INT NOT NULL FOREIGN KEY (id_ksiazki) REFERENCES dbo.ksiazka(id), 
    [do_wyporzyczenia] TEXT NULL,

)

CREATE TABLE [dbo].[czytelnicy]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [nazwisko] TEXT NULL, 
    [imie] TEXT NULL, 
    [adres] TEXT NULL, 
    [miasto] TEXT NULL, 
    [kod_pocztowy] TEXT NULL, 
    [data_zapisania] DATE NULL,

)

CREATE TABLE [dbo].[wyporzyczenia]
(
	[id_egzemplarz] INT NOT NULL FOREIGN KEY (id_egzemplarz) REFERENCES dbo.egzemplarze(id), 
    [id_czytelnik] INT NOT NULL FOREIGN KEY (id_czytelnik) REFERENCES dbo.czytelnicy(id), 
    [data_wyp] DATE NULL, 
    [data_zwrot] DATE NULL,

)