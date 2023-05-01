CREATE TABLE Greetings (
    Id INT NOT NULL AUTO_INCREMENT,
    Word VARCHAR(50) NOT NULL,
    `Language` VARCHAR(2) NOT NULL,
    PRIMARY KEY (Id)
);
INSERT INTO Greetings (Word, `Language`) VALUES ('Hello', 'en');
INSERT INTO Greetings (Word, `Language`) VALUES ('Bonjour', 'fr');
INSERT INTO Greetings (Word, `Language`) VALUES ('Hola', 'es');
INSERT INTO Greetings (Word, `Language`) VALUES ('Hallo', 'de');
INSERT INTO Greetings (Word, `Language`) VALUES ('Ciao', 'it');
INSERT INTO Greetings (Word, `Language`) VALUES ('Hej', 'dk');

CREATE TABLE Planets (
    Id INT NOT NULL AUTO_INCREMENT,
    `Name` NVARCHAR(50) NOT NULL,
    PRIMARY KEY (Id)
);
INSERT INTO Planets (Name) VALUES ('Mercury');
INSERT INTO Planets (Name) VALUES ('Venus');
INSERT INTO Planets (Name) VALUES ('Earth');
Insert INTO Planets (Name) VALUES ('Mars');
INSERT INTO Planets (Name) VALUES ('Jupiter');
INSERT INTO Planets (Name) VALUES ('Saturn');
INSERT INTO Planets (Name) VALUES ('Uranus');
INSERT INTO Planets (Name) VALUES ('Neptune');
INSERT INTO Planets (Name) VALUES ('Pluto');