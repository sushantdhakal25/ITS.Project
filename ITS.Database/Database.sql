

create database Jaildb;

Use jaildb

-- ************************************Tables*******************************
CREATE TABLE Officer(
    OfficerId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    IdentificationNumber VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(255) BINARY NOT NULL
);


CREATE TABLE Facility (
    FacilityId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Address VARCHAR(255) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    Phone VARCHAR(25) NULL
);


CREATE TABLE Inmate (
    InmateId INT AUTO_INCREMENT PRIMARY KEY,
    IdentificationNumber VARCHAR(50) NOT NULL UNIQUE,
    Name VARCHAR(100) NOT NULL,
    CurrentFacilityId INT,
    FOREIGN KEY (CurrentFacilityId) REFERENCES Facility(FacilityId)
);


CREATE TABLE Transfer (
    TransferId INT AUTO_INCREMENT PRIMARY KEY,
    InmateId INT NOT NULL,
    SourceFacilityId INT NOT NULL,
    DestinationFacilityId INT NOT NULL,
    DepartureTime DATETIME NOT NULL,
    ArrivalTime DATETIME NOT NULL,
    FOREIGN KEY (InmateId) REFERENCES Inmate(InmateId),
    FOREIGN KEY (SourceFacilityId) REFERENCES Facility(FacilityId),
    FOREIGN KEY (DestinationFacilityId) REFERENCES Facility(FacilityId)
);

-- *********************************Tables End **********************************

-- *********Inmate Procedures*************

DELIMITER //
CREATE PROCEDURE SpInmateSel(
    IN SearchText VARCHAR(100)
)
BEGIN

SELECT JSON_ARRAYAGG(JSON_OBJECT('InmateId', i.InmateId, 'Name', i.Name, 'IdentificationNumber', i.IdentificationNumber)) AS Json 
FROM INMATE AS i
INNER JOIN FACILITY AS f ON f.FacilityId = i.CurrentFacilityId 
WHERE (i.Name LIKE CONCAT('%', SearchText, '%') OR i.IdentificationNumber = SearchText)
      OR (f.Name = SearchText) OR SearchText IS NUll;
	
END; //
DELIMITER ;


DELIMITER //
CREATE PROCEDURE SpInmatesDelete(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS TempInmates;
    CREATE TEMPORARY TABLE TempInmates (
        InmateId INT
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempInmates (InmateId)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.inmateId')) AS inmateId
        FROM JSON_TABLE(param, '$[*]' COLUMNS (
            value JSON PATH '$'
        )) AS jt
        INNER JOIN Inmate i ON JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.inmateId')) = i.InmateId;
    ELSE
        INSERT INTO TempInmates (InmateId)
        SELECT JSON_UNQUOTE(JSON_EXTRACT(param, '$.inmateId'))
        WHERE EXISTS (
            SELECT 1 FROM Inmate i WHERE i.InmateId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.inmateId'))
        );
    END IF;

    DELETE i
    FROM Inmate i
    INNER JOIN TempInmates ti ON i.InmateId = ti.InmateId;

    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'inmateId', ti.InmateId
        )
    ) AS 'Json' INTO result
    FROM TempInmates ti;

END //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE SpInmatesUpdate(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS TempInmates;
    CREATE TEMPORARY TABLE TempInmates (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        InmateId INT,
        IdentificationNumber VARCHAR(50),
        Name VARCHAR(100),
        CurrentFacilityId INT
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempInmates (InmateId, IdentificationNumber, Name, CurrentFacilityId)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.inmateId')) AS InmateId,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.identificationNumber')) AS IdentificationNumber,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.name')) AS Name,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.currentFacilityId')) AS CurrentFacilityId
        FROM JSON_TABLE(param, '$[*]' COLUMNS (
            value JSON PATH '$'
        )) AS jt
        INNER JOIN Inmate i ON i.InmateId = JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.inmateId'));
        
    ELSE
        INSERT INTO TempInmates (InmateId, IdentificationNumber, Name, CurrentFacilityId)
        SELECT JSON_UNQUOTE(JSON_EXTRACT(param, '$.inmateId')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.identificationNumber')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.name')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.currentFacilityId'))
        WHERE EXISTS (
            SELECT 1 FROM Inmate i WHERE i.InmateId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.inmateId'))
        );
    END IF;

    UPDATE Inmate i
    INNER JOIN TempInmates ti ON i.InmateId = ti.InmateId
    set i.IdentificationNumber = ti.IdentificationNumber,
    i.Name = ti.Name,
    i.CurrentFacilityId = ti.CurrentFacilityId;
    

    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'inmateId', ti.InmateId,
            'identificationNumber', ti.IdentificationNumber,
            'name', ti.Name,
            'currentFacilityId', ti.CurrentFacilityId
        )
    ) AS 'Json' INTO result
    FROM TempInmates ti;

END //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE SpInmateInsert(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    
    DECLARE MaxId INT;
    DECLARE MinId INT;
    DROP TEMPORARY TABLE IF EXISTS TempInmates;
    DROP TEMPORARY TABLE IF EXISTS TempInmatesId;

    CREATE TEMPORARY TABLE TempInmates (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        IdentificationNumber VARCHAR(50) NOT NULL,
        Name VARCHAR(100) NOT NULL,
        CurrentFacilityId INT,
        InmateId INT
    );

    CREATE TEMPORARY TABLE TempInmatesId (
        InmateId INT NOT NULL
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempInmates (IdentificationNumber, Name, CurrentFacilityId)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.identificationNumber')) AS IdentificationNumber,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.name')) AS Name,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.currentFacilityId')) AS CurrentFacilityId
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt;
    ELSE
        INSERT INTO TempInmates (IdentificationNumber, Name, CurrentFacilityId)
        VALUES (
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.identificationNumber')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.name')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.currentFacilityId'))
        );
    END IF;

    SELECT MIN(Id) INTO MinId FROM TempInmates;
    SELECT MAX(Id) INTO MaxId FROM TempInmates;
    WHILE MinId <= MaxId DO
	
        INSERT INTO Inmate (IdentificationNumber, Name, CurrentFacilityId)
        SELECT 
            ti.IdentificationNumber,
            ti.Name,
            ti.CurrentFacilityId
        FROM TempInmates as ti
        left join Inmate as i on i.IdentificationNumber=ti.IdentificationNumber
        WHERE ti.id = MinId and i.InmateId is null;

        INSERT INTO TempInmatesId (InmateId)
        SELECT LAST_INSERT_ID();

        SET MinId = MinId + 1;
    END WHILE;

   SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'inmateId', i.InmateId,
            'identificationNumber', i.IdentificationNumber,
            'name', i.Name,
            'currentFacilityId', i.CurrentFacilityId,
            'facilityName', f.Name
        )
    ) AS 'Json' INTO result
    FROM TempInmatesId ti
    INNER JOIN Inmate i ON i.InmateId = ti.InmateId
    INNER JOIN Facility f ON f.FacilityId = i.CurrentFacilityId;

END //
DELIMITER ;

-- *****************Inmate Procedure end**************************

-- *****************Officer Procedure start***********************


DELIMITER //
CREATE PROCEDURE SpOfficerLoginSel(
    IN IdentificationNumber VARCHAR(100),
    IN Password NVARCHAR(25)
)
BEGIN

SELECT u.OfficerId,u.Name,u.IdentificationNumber
FROM OFFICER AS u 
WHERE u.IdentificationNumber = IdentificationNumber
AND u.Password = Password;
	
END; //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE SpOfficerSel(
    IN SearchText VARCHAR(100)
)
BEGIN

SELECT JSON_ARRAYAGG(JSON_OBJECT('officerId', i.OfficerId, 'name', i.Name, 'identificationNumber', i.IdentificationNumber)) AS Json 
FROM OFFICER AS i
WHERE (i.Name LIKE CONCAT('%', SearchText, '%') OR i.IdentificationNumber = SearchText)
       OR SearchText IS NUll;
	
END; //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE SpOfficerInsert(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    DECLARE MaxId INT;
    DECLARE MinId INT;
    DROP TEMPORARY TABLE IF EXISTS TempOfficers;
    DROP TEMPORARY TABLE IF EXISTS TempOfficerIds;

    CREATE TEMPORARY TABLE TempOfficers (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        IdentificationNumber VARCHAR(50) NOT NULL,
        Name VARCHAR(50) NOT NULL,
        Password NVARCHAR(255) NOT NULL,
        OfficerId INT
    );

    CREATE TEMPORARY TABLE TempOfficerIds (
        OfficerId INT NOT NULL
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempOfficers (IdentificationNumber, Name, Password)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.identificationNumber')) AS IdentificationNumber,
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.name')) AS Name,
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.password')) AS Password
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt;
    ELSE
        INSERT INTO TempOfficers (IdentificationNumber, Name, Password)
        VALUES (
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.identificationNumber')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.name')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.password'))
        );
    END IF;

    SELECT MIN(Id) INTO MinId FROM TempOfficers;
    SELECT MAX(Id) INTO MaxId FROM TempOfficers;

    WHILE MinId <= MaxId DO
        INSERT INTO Officer (IdentificationNumber, Name, Password)
        SELECT 
            tof.IdentificationNumber,
            tof.Name,
            tof.Password
        FROM TempOfficers as tof
        LEFT JOIN Officer as o ON o.IdentificationNumber = tof.IdentificationNumber
        WHERE tof.id = MinId AND o.OfficerId IS NULL;

        INSERT INTO TempOfficerIds (OfficerId)
        SELECT LAST_INSERT_ID();

        SET MinId = MinId + 1;
    END WHILE;

    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'officerId', o.OfficerId,
            'identificationNumber', o.IdentificationNumber,
            'name', o.Name
        )
    ) AS 'Json' INTO result
    FROM Officer o
    INNER JOIN TempOfficerIds toi ON toi.OfficerId = o.OfficerId;

END //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE SpOfficerUpdate(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    
    DROP TEMPORARY TABLE IF EXISTS TempOfficers;

    CREATE TEMPORARY TABLE TempOfficers (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        OfficerId INT,
        IdentificationNumber VARCHAR(50),
        Name VARCHAR(50),
        Password NVARCHAR(255)
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempOfficers (OfficerId, IdentificationNumber, Name, Password)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.officerId')) AS OfficerId,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.identificationNumber')) AS IdentificationNumber,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.name')) AS Name,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.password')) AS Password
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt
        INNER JOIN Officer as o on o.OfficerId = JSON_UNQUOTE(JSON_EXTRACT(value, '$.officerId'));
    ELSE
        INSERT INTO TempOfficers (OfficerId, IdentificationNumber, Name, Password)
        SELECT JSON_UNQUOTE(JSON_EXTRACT(param, '$.officerId')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.identificationNumber')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.name')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.currentFacilityId'))
        WHERE EXISTS (
            SELECT 1 FROM Officer as o WHERE o.OfficerId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.officerId'))
        );
    END IF;
    
    UPDATE Officer o
    INNER JOIN TempOfficers ti ON ti.OfficerId = o.OfficerId
    set o.IdentificationNumber = ti.IdentificationNumber,
    o.Name = ti.Name,
    o.Password = ti.Password;
    
    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'officerId', ti.OfficerId,
            'identificationNumber', ti.IdentificationNumber,
            'name', ti.Name,
            'password', ti.Password
        )
    ) AS 'Json' INTO result
    FROM TempOfficers ti;  

END //
DELIMITER ;




DELIMITER //
CREATE PROCEDURE SpOfficerDelete(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    
    DROP TEMPORARY TABLE IF EXISTS TempOfficers;

    CREATE TEMPORARY TABLE TempOfficers (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        OfficerId INT
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempOfficers (OfficerId)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.officerId')) AS OfficerId
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt
        INNER JOIN Officer as o on o.OfficerId = JSON_UNQUOTE(JSON_EXTRACT(value, '$.officerId'));
    ELSE
        INSERT INTO TempOfficers (OfficerId)
        SELECT 
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.officerId'))
        WHERE EXISTS (
            SELECT 1 FROM Officer as o WHERE o.OfficerId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.officerId'))
        );
    END IF;
    
    DELETE i
    FROM Officer i
    INNER JOIN TempOfficers ti ON ti.OfficerId = i.OfficerId;
    
    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'officerId', ti.OfficerId
        )
    ) AS 'Json' INTO result
    FROM TempOfficers ti;  

END //
DELIMITER ;

-- ******************Officer prodecure end********************

-- ******************Faciltiy procedure start*****************

DELIMITER //
CREATE PROCEDURE SpFacilitySel(
IN SearchText VARCHAR(100)
)
BEGIN
    SELECT JSON_ARRAYAGG(JSON_OBJECT('FacilityId', f.FacilityId, 'Name', f.Name, 'Address', f.Address, 'Email', f.Email, 'Phone', f.Phone)) AS Json
    FROM Facility AS f 
    WHERE f.Name LIKE CONCAT('%', SearchText, '%') OR f.Address LIKE CONCAT(SearchText, '%') OR SearchText IS NULL;
END //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE SpFacilityUpdate(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    
    DROP TEMPORARY TABLE IF EXISTS TempFacilities;

    CREATE TEMPORARY TABLE TempFacilities (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        FacilityId INT,
        Name VARCHAR(100),
		Address VARCHAR(255),
		Email VARCHAR(50),
		Phone VARCHAR(25)
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempFacilities (FacilityId, Name, Address, Email, Phone)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.facilityId')) AS FacilityId,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.name')) AS Name,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.address')) AS Address,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.email')) AS Email,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.phone')) AS Phone
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt
        INNER JOIN Facility as f on f.FacilityId = JSON_UNQUOTE(JSON_EXTRACT(value, '$.facilityId'));
    ELSE
        INSERT INTO TempFacilities (FacilityId, Name, Address, Email, Phone)
        SELECT 
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.facilityId')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.name')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.address')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.email')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.phone'))
        WHERE EXISTS (
            SELECT 1 FROM Facility as f WHERE f.FacilityId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.facilityId'))
        );
    END IF;
    
    UPDATE Facility f
    INNER JOIN TempFacilities ti ON ti.FacilityId = f.FacilityId
    set 
    f.Name = ti.Name,
    f.Address = ti.Address,
    f.Email = ti.Email,
    f.Phone = ti.Phone;
    
    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'facilityId', ti.FacilityId,
            'name', ti.Name,
            'address', ti.Address,
            'email', ti.Email,
            'phone', ti.Phone
        )
    ) AS 'Json' INTO result
    FROM TempFacilities ti;  

END //
DELIMITER ;




DELIMITER //
CREATE PROCEDURE SpFacilityInsert(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    DECLARE MaxId INT;
    DECLARE MinId INT;
    DROP TEMPORARY TABLE IF EXISTS TempFacilities;
    DROP TEMPORARY TABLE IF EXISTS TempFacilityId;

   CREATE TEMPORARY TABLE TempFacilities (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        Name VARCHAR(100) NOT NULL,
		Address VARCHAR(255) NOT NULL,
		Email VARCHAR(50) NOT NULL,
		Phone VARCHAR(25)
    );

    CREATE TEMPORARY TABLE TempFacilityId (
        FacilityId INT NOT NULL
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempFacilities (Name, Address, Email, Phone)
        SELECT
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.name')) AS Name,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.address')) AS Address,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.email')) AS Email,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.phone')) AS Phone
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt;
    ELSE
        INSERT INTO TempFacilities (Name, Address, Email, Phone)
        SELECT 
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.name')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.address')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.email')),
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.phone'));
    END IF;

    SELECT MIN(Id) INTO MinId FROM TempFacilities;
    SELECT MAX(Id) INTO MaxId FROM TempFacilities;

    WHILE MinId <= MaxId DO
        INSERT INTO Facility (FacilityId, Name, Address, Email, Phone)
        SELECT 
            tt.FacilityId,
            tt.Name,
            tt.Address,
            tt.Email,
            tt.Phone
        FROM TempFacilities AS tt
        LEFT JOIN Facility AS t ON t.FacilityId = tt.FacilityId
        WHERE tt.id = MinId AND t.FacilityId IS NULL;

        INSERT INTO TempFacilityId (FacilityId)
        SELECT LAST_INSERT_ID();

        SET MinId = MinId + 1;
    END WHILE;

    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'facilityId', ti.FacilityId,
            'name', ti.Name,
            'address', ti.Address,
            'email', ti.Email,
            'phone', ti.Phone
        )
    ) AS 'Json' INTO result
    FROM TempFacilityId as t
    INNER JOIN Facility as tt on tt.FacilityId = t.FacilityId;

END //
DELIMITER ;





DELIMITER //
CREATE PROCEDURE SpFacilityDelete(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    
    DROP TEMPORARY TABLE IF EXISTS TempFacilities;

    CREATE TEMPORARY TABLE TempFacilities (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        FacilityId INT
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempFacilities (FacilityId)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.facilityId')) AS FacilityId
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt
        INNER JOIN Facility as f on f.FacilityId = JSON_UNQUOTE(JSON_EXTRACT(value, '$.facilityId'));
    ELSE
        INSERT INTO TempFacilities (FacilityId)
        SELECT 
        JSON_UNQUOTE(JSON_EXTRACT(param, '$.facilityId'))
        WHERE EXISTS (
            SELECT 1 FROM Facility as f WHERE f.FacilityId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.facilityId'))
        );
    END IF;
    
    DELETE i
    FROM Facility i
    INNER JOIN TempFacilities ti ON ti.FacilityId = i.FacilityId;
    
    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'facilityId', ti.FacilityId
        )
    ) AS 'Json' INTO result
    FROM TempFacilities ti;  

END //
DELIMITER ;

-- ******************Faciltiy procedure end*****************

-- ******************Transfer procedure start*****************


DELIMITER //
CREATE PROCEDURE SpTransferSel(
IN searchText VARCHAR(100)
)
BEGIN
    SELECT JSON_ARRAYAGG(JSON_OBJECT(
        'transferId', t.TransferId, 
        'inmateId', t.InmateId,
        'inmateName', i.Name
        'sourceFacilityId', t.SourceFacilityId, 
        'sourceFacilityName', sf.Name,
        'destinationFacilityId', t.DestinationFacilityId, 
        'destinationFacilityName', df.Name,
        'departureTime', t.DepartureTime, 
        'arrivalTime', t.ArrivalTime
    )) AS Json
    FROM Transfer AS t
    INNER JOIN Inmate AS i ON i.InmateId = t.InmateId
    INNER JOIN Facility AS sf ON sf.FacilityId = t.SourceFacilityId
    INNER JOIN Facility AS df ON df.FacilityId = t.DestinationFacilityId
    WHERE sf.Name LIKE CONCAT('%', searchText, '%')
       OR df.Name LIKE CONCAT('%', searchText, '%')
       OR i.Name LIKE CONCAT('%', searchText, '%')
       OR searchText IS NULL;
END //
DELIMITER ;




DELIMITER //
CREATE PROCEDURE SpTransferUpdate(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    
    DROP TEMPORARY TABLE IF EXISTS TempTransfers;

    CREATE TEMPORARY TABLE TempTransfers (
        TransferId INT,
        InmateId INT NOT NULL,
        SourceFacilityId INT NOT NULL,
        DestinationFacilityId INT NOT NULL,
        DepartureTime DATETIME NOT NULL,
        ArrivalTime DATETIME NOT NULL
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempTransfers (TransferId, InmateId, SourceFacilityId, DestinationFacilityId, DepartureTime, ArrivalTime)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.transferId')) AS TransferId,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.inmateId')) AS InmateId,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.sourceFacilityId')) AS SourceFacilityId,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.destinationFacilityId')) AS DestinationFacilityId,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.departureTime')) AS DepartureTime,
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.arrivalTime')) AS ArrivalTime
        FROM JSON_TABLE(param, '$[*]' COLUMNS (
            value JSON PATH '$'
        )) AS jt
        INNER JOIN Transfer as t on t.TransferId = JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.transferId'));
    ELSE
        INSERT INTO TempTransfers (TransferId, InmateId, SourceFacilityId, DestinationFacilityId, DepartureTime, ArrivalTime)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.transferId')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.inmateId')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.sourceFacilityId')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.destinationFacilityId')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.departureTime')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.arrivalTime'))
        WHERE EXISTS (
            SELECT 1 FROM Transfer as t WHERE t.TransferId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.transferId'))
        );
    END IF;

    UPDATE Transfer t
    INNER JOIN TempTransfers tt ON t.TransferId = tt.TransferId
    SET 
        t.InmateId = tt.InmateId,
        t.SourceFacilityId = tt.SourceFacilityId,
        t.DestinationFacilityId = tt.DestinationFacilityId,
        t.DepartureTime = tt.DepartureTime,
        t.ArrivalTime = tt.ArrivalTime;

    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'transferId', t.TransferId,
            'inmateId', t.InmateId,
            'sourceFacilityId', t.SourceFacilityId,
            'destinationFacilityId', t.DestinationFacilityId,
            'departureTime', t.DepartureTime,
            'arrivalTime', t.ArrivalTime,
            'sourceFacilityName', sf.Name,
            'destinationFacilityName', df.Name
        )
    ) AS 'Json' INTO result
    FROM TempTransfers t
    INNER JOIN Facility sf ON sf.FacilityId = t.SourceFacilityId
    INNER JOIN Facility df ON df.FacilityId = t.DestinationFacilityId;

END //
DELIMITER ;



DELIMITER //
CREATE PROCEDURE SpTransferInsert(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    DECLARE MaxId INT;
    DECLARE MinId INT;
    DROP TEMPORARY TABLE IF EXISTS TempTransfers;
    DROP TEMPORARY TABLE IF EXISTS TempTransfersId;

    CREATE TEMPORARY TABLE TempTransfers (
        Id INT AUTO_INCREMENT PRIMARY KEY,
        InmateId INT NOT NULL,
        SourceFacilityId INT NOT NULL,
        DestinationFacilityId INT NOT NULL,
        DepartureTime DATETIME NOT NULL,
        ArrivalTime DATETIME NOT NULL
    );

    CREATE TEMPORARY TABLE TempTransfersId (
        TransferId INT NOT NULL
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempTransfers (InmateId, SourceFacilityId, DestinationFacilityId, DepartureTime, ArrivalTime)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.inmateId')) AS InmateId,
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.sourceFacilityId')) AS SourceFacilityId,
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.destinationFacilityId')) AS DestinationFacilityId,
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.departureTime')) AS DepartureTime,
            JSON_UNQUOTE(JSON_EXTRACT(value, '$.arrivalTime')) AS ArrivalTime
        FROM JSON_TABLE(param, '$[*]' COLUMNS (value JSON PATH '$')) AS jt;
    ELSE
        INSERT INTO TempTransfers (InmateId, SourceFacilityId, DestinationFacilityId, DepartureTime, ArrivalTime)
        VALUES (
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.InmateId')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.SourceFacilityId')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.DestinationFacilityId')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.DepartureTime')),
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.ArrivalTime'))
        );
    END IF;

    SELECT MIN(Id) INTO MinId FROM TempTransfers;
    SELECT MAX(Id) INTO MaxId FROM TempTransfers;

    WHILE MinId <= MaxId DO
        INSERT INTO Transfer (InmateId, SourceFacilityId, DestinationFacilityId, DepartureTime, ArrivalTime)
        SELECT 
            tt.InmateId,
            tt.SourceFacilityId,
            tt.DestinationFacilityId,
            tt.DepartureTime,
            tt.ArrivalTime
        FROM TempTransfers AS tt
        LEFT JOIN Transfer AS t ON t.TransferId = tt.TransferId
        WHERE tt.id = MinId AND t.TransferId IS NULL;

        INSERT INTO TempTransfersId (TransferId)
        SELECT LAST_INSERT_ID();

        SET MinId = MinId + 1;
    END WHILE;

    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'transferId', t.TransferId,
            'inmateId', t.InmateId,
            'sourceFacilityId', t.SourceFacilityId,
            'destinationFacilityId', t.DestinationFacilityId,
            'departureTime', t.DepartureTime,
            'arrivalTime', t.ArrivalTime,
            'sourceFacilityName', sf.Name,
            'destinationFacilityName', df.Name
        )
    ) AS 'Json' INTO result
    FROM TempTransfersId tt
    INNER JOIN Transfers t ON t.TransferId = tt.TransferId
    INNER JOIN Facility sf ON sf.FacilityId = t.SourceFacilityId
    INNER JOIN Facility df ON df.FacilityId = t.DestinationFacilityId;

END //
DELIMITER ;




DELIMITER //
CREATE PROCEDURE SpTransferDelete(IN param LONGTEXT, OUT result LONGTEXT)
BEGIN
    
    DROP TEMPORARY TABLE IF EXISTS TempTransfers;

    CREATE TEMPORARY TABLE TempTransfers (
        TransferId INT
    );

    IF JSON_TYPE(param) = 'ARRAY' THEN
        INSERT INTO TempTransfers (TransferId)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.transferId')) AS TransferId
        FROM JSON_TABLE(param, '$[*]' COLUMNS (
            value JSON PATH '$'
        )) AS jt
        INNER JOIN Transfer as t on t.TransferId = JSON_UNQUOTE(JSON_EXTRACT(jt.value, '$.transferId'));
    ELSE
        INSERT INTO TempTransfers (TransferId)
        SELECT 
            JSON_UNQUOTE(JSON_EXTRACT(param, '$.transferId'))
        WHERE EXISTS (
            SELECT 1 FROM Transfer as t WHERE t.TransferId = JSON_UNQUOTE(JSON_EXTRACT(param, '$.transferId'))
        );
    END IF;
    
    DELETE i
    FROM Transfer i
    INNER JOIN TempTransfers ti ON ti.TransferId = i.TransferId;

    SELECT JSON_ARRAYAGG(
        JSON_OBJECT(
            'transferId', t.TransferId
        )
    ) AS 'Json' INTO result
    FROM TempTransfers t;

END //
DELIMITER ;



-- ******************Transfer procedure end*****************














