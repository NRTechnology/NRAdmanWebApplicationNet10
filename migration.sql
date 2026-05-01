CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;
CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) NOT NULL,
    `Name` varchar(256) NULL,
    `NormalizedName` varchar(256) NULL,
    `ConcurrencyStamp` longtext NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) NOT NULL,
    `WargaId` char(36) NOT NULL,
    `ProfilePicture` longblob NOT NULL,
    `UserName` varchar(256) NULL,
    `NormalizedUserName` varchar(256) NULL,
    `Email` varchar(256) NULL,
    `NormalizedEmail` varchar(256) NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext NULL,
    `SecurityStamp` longtext NULL,
    `ConcurrencyStamp` longtext NULL,
    `PhoneNumber` longtext NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) NOT NULL,
    `ProviderKey` varchar(255) NOT NULL,
    `ProviderDisplayName` longtext NULL,
    `UserId` varchar(255) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) NOT NULL,
    `RoleId` varchar(255) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) NOT NULL,
    `LoginProvider` varchar(255) NOT NULL,
    `Name` varchar(255) NOT NULL,
    `Value` longtext NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260427030737_InitDB', '10.0.7');

ALTER TABLE `AspNetUsers` MODIFY `ProfilePicture` longblob NULL;

CREATE TABLE `nas` (
    `id` int NOT NULL AUTO_INCREMENT,
    `nasname` varchar(128) NOT NULL,
    `shortname` varchar(32) NULL,
    `type` varchar(30) NULL,
    `ports` int NULL,
    `secret` varchar(60) NOT NULL,
    `server` varchar(64) NULL,
    `community` varchar(50) NULL,
    `description` varchar(200) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `radacct` (
    `radacctid` bigint NOT NULL AUTO_INCREMENT,
    `acctsessionid` varchar(64) NOT NULL,
    `acctuniqueid` varchar(32) NOT NULL,
    `username` varchar(64) NOT NULL,
    `realm` varchar(64) NULL,
    `nasipaddress` varchar(15) NOT NULL,
    `nasportid` varchar(32) NULL,
    `nasporttype` varchar(32) NULL,
    `acctstarttime` datetime(6) NULL,
    `acctupdatetime` datetime(6) NULL,
    `acctstoptime` datetime(6) NULL,
    `acctinterval` int NULL,
    `acctsessiontime` int unsigned NULL,
    `acctauthentic` varchar(32) NULL,
    `connectinfo_start` varchar(128) NULL,
    `connectinfo_stop` varchar(128) NULL,
    `acctinputoctets` bigint NULL,
    `acctoutputoctets` bigint NULL,
    `calledstationid` varchar(50) NOT NULL,
    `callingstationid` varchar(50) NOT NULL,
    `acctterminatecause` varchar(32) NOT NULL,
    `servicetype` varchar(32) NULL,
    `framedprotocol` varchar(32) NULL,
    `framedipaddress` varchar(15) NOT NULL,
    `framedipv6address` varchar(45) NOT NULL,
    `framedipv6prefix` varchar(45) NOT NULL,
    `framedinterfaceid` varchar(44) NOT NULL,
    `delegatedipv6prefix` varchar(45) NOT NULL,
    `class` varchar(64) NULL,
    PRIMARY KEY (`radacctid`)
);

CREATE TABLE `radcheck` (
    `id` int unsigned NOT NULL AUTO_INCREMENT,
    `username` varchar(64) NOT NULL,
    `attribute` varchar(64) NOT NULL,
    `op` varchar(2) NOT NULL,
    `value` varchar(253) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `radgroupcheck` (
    `id` int unsigned NOT NULL AUTO_INCREMENT,
    `groupname` varchar(64) NOT NULL,
    `attribute` varchar(64) NOT NULL,
    `op` varchar(2) NOT NULL,
    `value` varchar(253) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `radgroupreply` (
    `id` int unsigned NOT NULL AUTO_INCREMENT,
    `groupname` varchar(64) NOT NULL,
    `attribute` varchar(64) NOT NULL,
    `op` varchar(2) NOT NULL,
    `value` varchar(253) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `radpostauth` (
    `id` int NOT NULL AUTO_INCREMENT,
    `username` varchar(64) NOT NULL,
    `pass` varchar(64) NOT NULL,
    `reply` varchar(32) NOT NULL,
    `authdate` datetime(6) NOT NULL,
    `class` varchar(64) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `radreply` (
    `id` int unsigned NOT NULL AUTO_INCREMENT,
    `username` varchar(64) NOT NULL,
    `attribute` varchar(64) NOT NULL,
    `op` varchar(2) NOT NULL,
    `value` varchar(253) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `radusergroup` (
    `id` int unsigned NOT NULL AUTO_INCREMENT,
    `username` varchar(64) NOT NULL,
    `groupname` varchar(64) NOT NULL,
    `priority` int NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE INDEX `nasname` ON `nas` (`nasname`);

CREATE UNIQUE INDEX `acctuniqueid` ON `radacct` (`acctuniqueid`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260427031418_RadiusDB', '10.0.7');

COMMIT;

