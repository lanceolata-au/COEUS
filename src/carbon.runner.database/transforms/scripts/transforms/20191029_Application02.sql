CREATE TABLE Countries(
  Id INT NOT NULL,
  ShortCode CHAR(2) NOT NULL,
  FullName char(128) NOT NULL
);

CREATE TABLE States(
  Id INT NOT NULL,
  CountryId INT NOT NULL,
  ShortCode CHAR(2) NOT NULL,
  FullName char(128) NOT NULL
);

CREATE TABLE Formations(
   Id INT NOT NULL,
   StateId INT NOT NULL,
   FullName char(255) NOT NULL
);