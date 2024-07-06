-- yargIdentityDB.ApplicationExternalLogin definition
CREATE TABLE `ApplicationExternalLogin`
	(
		`Id`            int(11) NOT NULL AUTO_INCREMENT
	  , `UserId`        int(11) NOT NULL
	  , `LoginProvider` varchar(256) NOT NULL
	  , `ProviderKey`   varchar(256) NOT NULL
	  , `DisplayName`   varchar(256) NOT NULL
	  , PRIMARY KEY (`Id`)
	)
	ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargIdentityDB.ApplicationRole definition
CREATE TABLE `ApplicationRole`
	(
		`Id`             int(11) NOT NULL AUTO_INCREMENT
	  , `Name`           varchar(256) NOT NULL
	  , `NormalizedName` varchar(256) NOT NULL
	  , PRIMARY KEY (`Id`)
	  , KEY `ApplicationRole_NormalizedName_IDX` (`NormalizedName`) USING BTREE
	)
	ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargIdentityDB.ApplicationUser definition
CREATE TABLE `ApplicationUser`
	(
		`Id`                 int(11) NOT NULL AUTO_INCREMENT
	  , `UserName`           varchar(256) NOT NULL
	  , `NormalizedUserName` varchar(256) NOT NULL
	  , `Email`              varchar(256) DEFAULT NULL
	  , `NormalizedEmail`    varchar(256) DEFAULT NULL
	  , `EmailConfirmed`     bit(1) NOT NULL
	  , `PasswordHash` longtext DEFAULT NULL
	  , `PhoneNumber`          varchar(50) DEFAULT NULL
	  , `PhoneNumberConfirmed` bit(1) NOT NULL
	  , `TwoFactorEnabled`     bit(1) NOT NULL
	  , PRIMARY KEY (`Id`)
	  , KEY `ApplicationUser_NormalizedUserName_IDX` (`NormalizedUserName`) USING BTREE
	  , KEY `ApplicationUser_NormalizedEmail_IDX` (`NormalizedEmail`) USING BTREE
	)
	ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;

-- yargIdentityDB.ApplicationUserRole definition
CREATE TABLE `ApplicationUserRole`
	(
		`UserId` int(11) NOT NULL
	  , `RoleId` int(11) NOT NULL
	  , PRIMARY KEY (`UserId`,`RoleId`)
	  , KEY `ApplicationUserRole_FK_1` (`RoleId`)
	  , CONSTRAINT `ApplicationUserRole_FK` FOREIGN KEY (`UserId`) REFERENCES `ApplicationUser` (`Id`)
	  , CONSTRAINT `ApplicationUserRole_FK_1` FOREIGN KEY (`RoleId`) REFERENCES `ApplicationRole` (`Id`)
	)
	ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci
;