CREATE TABLE test_2(
  id char(36) NULl,
  name varchar(128) NOT NULL DEFAULT 'DEFAULT',
  value int NOT NULL DEFAULT 100
);


DELIMITER ;;
CREATE TRIGGER test_2_id
  BEFORE INSERT ON test_2
  FOR EACH ROW
BEGIN
  IF new.id IS NULL THEN
    SET new.id = uuid();
  END IF;
END
;;

DELIMITER ;