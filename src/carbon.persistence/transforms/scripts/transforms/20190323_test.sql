CREATE TABLE test(
  id char(36) NULL,
  name varchar(128) NOT NULL DEFAULT 'DEFAULT',
  value int NOT NULL DEFAULT 100
);

DELIMITER ;;
CREATE TRIGGER test_id
  BEFORE INSERT ON test
  FOR EACH ROW
BEGIN
  IF new.id IS NULL THEN
    SET new.id = uuid();
  END IF;
END
;;

DELIMITER ;