CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spAddLogin`(IN thisid int , IN thisLoginProvider varchar(256) , IN thisProviderKey varchar(256) , IN thisDisplayName varchar(256))
BEGIN
	INSERT INTO ApplicationExternalLogin
		(UserId
		  , LoginProvider
		  , ProviderKey
		  , DisplayName
		)
		VALUES
		(thisid
		  , thisLoginProvider
		  , thisProviderKey
		  , thisDisplayName
		)
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spAddRole`(IN thisName varchar(256) , IN thisNormalizedName varchar(256) , OUT LID int)
BEGIN
	INSERT INTO ApplicationRole
		(Name
		  , NormalizedName
		)
		VALUES
		(thisName
		  , thisNormalizedName
		)
	;
	
	SET LID = LAST_INSERT_ID();
END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spAddToRole`(IN thisUserId int , IN thisRoleId int)
BEGIN
	INSERT IGNORE
	INTO
		ApplicationUserRole
		(UserId
		  , RoleId
		)
		VALUES
		(thisUserId
		  , thisRoleId
		)
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spAddUser`(IN thisUserName varchar(256) , IN thisNormalizedUserName varchar(256) , IN thisEmail varchar(256) , IN thisNormalizedEmail varchar(256) , IN thisEmailConfirmed bit , IN thisPasswordHash longtext , IN thisPhoneNumber varchar(50) , IN thisPhoneNumberConfirmed bit , IN thisTwoFactorEnabled bit , OUT LID int)
BEGIN
	INSERT INTO ApplicationUser
		(UserName
		  , NormalizedUserName
		  , Email
		  , NormalizedEmail
		  , EmailConfirmed
		  , PasswordHash
		  , PhoneNumber
		  , PhoneNumberConfirmed
		  , TwoFactorEnabled
		)
		VALUES
		(thisUserName
		  , thisNormalizedUserName
		  , thisEmail
		  , thisNormalizedEmail
		  , thisEmailConfirmed
		  , thisPasswordHash
		  , thisPhoneNumber
		  , thisPhoneNumberConfirmed
		  , thisTwoFactorEnabled
		)
	;
	
	SET LID = LAST_INSERT_ID();
END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spDeleteByLogin`(IN thisid int , IN loginProvider varchar(256) , IN providerKey varchar(256) )
BEGIN
	DELETE
	FROM
		ApplicationExternalLogin
	WHERE
		UserId            = thisid
		and LoginProvider = loginProvider
		and ProviderKey   = providerKey
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spDeleteRole`(IN thisId int)
BEGIN
	DELETE
	from
		ApplicationRole
	where
		Id                  = thisId
		and NormalizedName <> 'ADMIN'
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spDeleteUser`(IN thisid int)
BEGIN
	DELETE
	FROM
		ApplicationUser
	WHERE
		Id = thisid
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spDeleteUserRole`(IN thisUserId int , IN thisRoleId int)
BEGIN
	DELETE
	FROM
		ApplicationUserRole
	WHERE
		UserId     = thisUserId
		AND RoleId = thisRoleId
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spFindByEmail`(IN thisNormalizedEmail varchar(256))
BEGIN
	SELECT
		Id
	  , UserName
	  , NormalizedUserName
	  , Email
	  , NormalizedEmail
	  , EmailConfirmed
	  , PasswordHash
	  , PhoneNumber
	  , PhoneNumberConfirmed
	  , TwoFactorEnabled
	FROM
		ApplicationUser
	WHERE
		NormalizedEmail = thisNormalizedEmail
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spFindByLogin`(IN loginProvider varchar(256) , IN providerKey varchar(256))
BEGIN
	select
		au.Id
	  , au.UserName
	  , au.NormalizedUserName
	  , au.Email
	  , au.NormalizedEmail
	  , au.EmailConfirmed
	  , au.PasswordHash
	  , au.PhoneNumber
	  , au.PhoneNumberConfirmed
	  , au.TwoFactorEnabled
	from
		ApplicationUser au
		join
			ApplicationExternalLogin ael
			on
				ael.UserId = au.Id
	WHERE
		ael.LoginProvider   = loginProvider
		and ael.ProviderKey = providerKey
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spFindByUserId`(IN thisid int)
BEGIN
	select
		ael.UserId
	  , ael.LoginProvider
	  , ael.ProviderKey
	  , ael.DisplayName
	from
		ApplicationExternalLogin ael
	WHERE
		ael.UserId = thisid
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spFindRoleById`(IN thisid int)
BEGIN
	select
		ar.Id
	  , ar.Name
	  , ar.NormalizedName
	from
		ApplicationRole ar
	where
		ar.Id = thisid
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spFindRoleByName`(in thisname varchar(256))
BEGIN
	select
		ar.Id
	  , ar.Name
	  , ar.NormalizedName
	from
		ApplicationRole ar
	where
		ar.NormalizedName = thisname
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spFindUserById`(IN thisid int)
BEGIN
	SELECT
		Id
	  , UserName
	  , NormalizedUserName
	  , Email
	  , NormalizedEmail
	  , EmailConfirmed
	  , PasswordHash
	  , PhoneNumber
	  , PhoneNumberConfirmed
	  , TwoFactorEnabled
	FROM
		ApplicationUser
	WHERE
		Id = thisid
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spFindUserByName`(in thisname varchar(256))
BEGIN
	SELECT
		Id
	  , UserName
	  , NormalizedUserName
	  , Email
	  , NormalizedEmail
	  , EmailConfirmed
	  , PasswordHash
	  , PhoneNumber
	  , PhoneNumberConfirmed
	  , TwoFactorEnabled
	FROM
		ApplicationUser
	WHERE
		NormalizedUserName = thisname
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spGetCountOfRolesForUserId`(IN thisUserId int , IN thisRoleId int)
BEGIN
	SELECT
		COUNT(*) as 'cntRoles'
	FROM
		ApplicationUserRole
	WHERE
		UserId     = thisUserId
		AND RoleId = thisRoleId
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spGetRoleIdFromNormalizedName`(IN thisNormalizedName varchar(256))
BEGIN
	select
		Id
	From
		ApplicationRole
	WHERE
		NormalizedName = thisNormalizedName
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spGetRolesByUserId`(IN thisUserId int)
BEGIN
	SELECT
		r.Name
	FROM
		ApplicationRole r
		INNER JOIN
			ApplicationUserRole ur
			ON
				ur.RoleId = r.Id
	WHERE
		ur.UserId = thisUserId
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spGetUserIdFromEmail`(IN thisEmail tinytext)
BEGIN
	select
		Id as 'Id'
	from
		ApplicationUser
	where
		Email = thisEmail
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spGetUsersInRole`(IN thisroleName varchar(256))
BEGIN
	SELECT
		u.*
	FROM
		ApplicationUser u
		INNER JOIN
			ApplicationUserRole ur
			ON
				ur.UserId = u.Id
		INNER JOIN
			ApplicationRole r
			ON
				r.Id = ur.RoleId
	WHERE
		r.NormalizedName = thisroleName
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spUpdateRole`(IN thisName varchar(256) , IN thisNormalizedName varchar(256), IN thisId int)
BEGIN
	UPDATE
		ApplicationRole
	SET Name           = thisName
	  , NormalizedName = thisNormalizedName
	WHERE
		Id = thisId
	;

END;
CREATE DEFINER=`yargIdentityUser`@`%` PROCEDURE `yargIdentityDB`.`spUpdateUser`(IN thisid int , IN thisUserName varchar(256) , IN thisNormalizedUserName varchar(256) , IN thisEmail varchar(256) , IN thisNormalizedEmail varchar(256) , IN thisEmailConfirmed bit , IN thisPasswordHash longtext , IN thisPhoneNumber varchar(50) , IN thisPhoneNumberConfirmed bit , IN thisTwoFactorEnabled bit)
BEGIN
	update
		ApplicationUser
	set UserName             = thisUserName
	  , NormalizedUserName   = thisNormalizedUserName
	  , Email                = thisEmail
	  , NormalizedEmail      = thisNormalizedEmail
	  , EmailConfirmed       = thisEmailConfirmed
	  , PasswordHash         = thisPasswordHash
	  , PhoneNumber          = thisPhoneNumber
	  , PhoneNumberConfirmed = thisPhoneNumberConfirmed
	  , TwoFactorEnabled     = thisTwoFactorEnabled
	where
		Id = thisid
	;

END;