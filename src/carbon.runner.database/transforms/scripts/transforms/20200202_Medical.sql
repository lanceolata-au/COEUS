CREATE TABLE ApplicationsMedical (
    Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    ApplicationId INT NOT NULL DEFAULT 0
);

COMMIT;

DELIMITER $$

CREATE PROCEDURE AppMedicalInsert()
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE aaaid INT;
    DECLARE cur CURSOR FOR SELECT Id FROM Applications;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN cur;

    read_loop: LOOP
        FETCH cur INTO aaaid;
        IF done THEN
            LEAVE read_loop;
        END IF;
        INSERT INTO ApplicationsMedical (ApplicationId) VALUES (aaaid);
    END LOOP;

    CLOSE cur;
END

$$

DELIMITER ;

CALL AppMedicalInsert();