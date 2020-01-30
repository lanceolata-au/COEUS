CREATE TABLE ScoutEvent(
  Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
  Name CHAR(254) NOT NULL
);

COMMIT;

# We now add a default event for previous data to live under
INSERT INTO ScoutEvent (Name) VALUES ('default');

COMMIT;

ALTER TABLE Applications
ADD EventId INT NOT NULL DEFAULT 1;

COMMIT;