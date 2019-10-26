CREATE TABLE Applications(
  Id INT NOT NULL,
  UserId CHAR(36) NOT NULL,
  Status TINYINT NOT NULL,
  DateOfBirth DATETIME NOT NULL,
  PhoneNo char(30) NOT NULL,
  RegistrationNo char(30) NOT NULL,
  State INT NOT NULL,
  Country INT NOT NULL,
  Formation INT NOT NULL,
  Locked TINYINT(1) NOT NULL
);