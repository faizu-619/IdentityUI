CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `Audit` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ActionType` int NOT NULL,
    `ObjectIdentifier` longtext NULL,
    `ObjectType` longtext NULL,
    `ObjectMetadata` longtext NULL,
    `SubjectType` int NOT NULL,
    `SubjectIdentifier` longtext NULL,
    `SubjectMetadata` longtext NULL,
    `GroupIdentifier` longtext NULL,
    `Host` longtext NULL,
    `RemoteIp` longtext NULL,
    `ResourceName` longtext NULL,
    `UserAgent` longtext NULL,
    `TraceIdentifier` longtext NULL,
    `AppVersion` longtext NULL,
    `Metadata` longtext NULL,
    `Created` datetime(6) NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Emails` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Subject` longtext NOT NULL,
    `Body` longtext NOT NULL,
    `Type` int NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Groups` (
    `Id` varchar(64) NOT NULL,
    `Name` longtext NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `_DeletedDate` datetime NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Permissions` (
    `Id` varchar(64) NOT NULL,
    `Name` longtext NOT NULL,
    `Description` longtext NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Roles` (
    `Id` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `Description` varchar(64) NULL,
    `Type` int NOT NULL DEFAULT 1,
    `Name` varchar(64) NULL,
    `NormalizedName` varchar(64) NULL,
    `ConcurrencyStamp` longtext NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Users` (
    `Id` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `FirstName` varchar(64) NULL,
    `LastName` varchar(64) NULL,
    `Enabled` tinyint(1) NOT NULL DEFAULT FALSE,
    `TwoFactor` int NOT NULL DEFAULT 0,
    `PasswordHash` longtext NULL,
    `ConcurrencyStamp` longtext NULL,
    `SecurityStamp` longtext NULL,
    `_DeletedDate` datetime NULL,
    `UserName` varchar(64) NULL,
    `NormalizedUserName` varchar(64) NULL,
    `Email` varchar(64) NULL,
    `NormalizedEmail` varchar(64) NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PhoneNumber` longtext NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `GroupAttributes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Key` varchar(64) NOT NULL,
    `Value` longtext NULL,
    `GroupId` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_GroupAttributes_Groups_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Invite` (
    `Id` varchar(64) NOT NULL,
    `Email` longtext NOT NULL,
    `Token` longtext NOT NULL,
    `Status` int NOT NULL,
    `RoleId` varchar(64) NULL,
    `GroupId` varchar(64) NULL,
    `GroupRoleId` varchar(64) NULL,
    `ExpiresAt` datetime NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Invite_Groups_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Invite_Roles_GroupRoleId` FOREIGN KEY (`GroupRoleId`) REFERENCES `Roles` (`Id`),
    CONSTRAINT `FK_Invite_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`)
);

CREATE TABLE `PermissionRole` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `PermissionId` varchar(64) NOT NULL,
    `RoleId` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PermissionRole_Permissions_PermissionId` FOREIGN KEY (`PermissionId`) REFERENCES `Permissions` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_PermissionRole_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RoleAssignments` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(64) NOT NULL,
    `CanAssigneRoleId` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RoleAssignments_Roles_CanAssigneRoleId` FOREIGN KEY (`CanAssigneRoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RoleAssignments_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `RoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `RoleId` varchar(64) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RoleClaims_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `GroupUsers` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `UserId` varchar(64) NOT NULL,
    `GroupId` varchar(64) NOT NULL,
    `RoleId` varchar(64) NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_GroupUsers_Groups_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Groups` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_GroupUsers_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE SET NULL,
    CONSTRAINT `FK_GroupUsers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Sessions` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `_DeletedDate` datetime NULL,
    `Ip` longtext NULL,
    `Code` longtext NULL,
    `LastAccess` datetime NOT NULL,
    `EndType` int NULL,
    `UserId` varchar(64) NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Sessions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
);

CREATE TABLE `UserAttributes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Key` varchar(64) NOT NULL,
    `Value` longtext NULL,
    `UserId` varchar(64) NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserAttributes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `UserId` varchar(64) NOT NULL,
    `ClaimType` longtext NULL,
    `ClaimValue` longtext NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserClaims_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserImage` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `BlobImage` longblob NOT NULL,
    `FileName` varchar(250) NOT NULL,
    `UserId` varchar(64) NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserImage_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserLogins` (
    `LoginProvider` varchar(64) NOT NULL,
    `ProviderKey` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `ProviderDisplayName` longtext NULL,
    `UserId` varchar(64) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_UserLogins_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserRoles` (
    `UserId` varchar(64) NOT NULL,
    `RoleId` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserTokens` (
    `UserId` varchar(64) NOT NULL,
    `LoginProvider` varchar(64) NOT NULL,
    `Name` varchar(64) NOT NULL,
    `_CreatedDate` datetime NULL,
    `_ModifiedDate` datetime NULL,
    `Value` longtext NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_UserTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_GroupAttributes_GroupId` ON `GroupAttributes` (`GroupId`);

CREATE INDEX `IX_GroupAttributes_Key` ON `GroupAttributes` (`Key`);

CREATE INDEX `IX_GroupUsers_GroupId` ON `GroupUsers` (`GroupId`);

CREATE INDEX `IX_GroupUsers_RoleId` ON `GroupUsers` (`RoleId`);

CREATE INDEX `IX_GroupUsers_UserId` ON `GroupUsers` (`UserId`);

CREATE INDEX `IX_Invite_GroupId` ON `Invite` (`GroupId`);

CREATE INDEX `IX_Invite_GroupRoleId` ON `Invite` (`GroupRoleId`);

CREATE INDEX `IX_Invite_RoleId` ON `Invite` (`RoleId`);

CREATE INDEX `IX_PermissionRole_PermissionId` ON `PermissionRole` (`PermissionId`);

CREATE INDEX `IX_PermissionRole_RoleId` ON `PermissionRole` (`RoleId`);

CREATE INDEX `IX_RoleAssignments_CanAssigneRoleId` ON `RoleAssignments` (`CanAssigneRoleId`);

CREATE INDEX `IX_RoleAssignments_RoleId` ON `RoleAssignments` (`RoleId`);

CREATE INDEX `IX_RoleClaims_RoleId` ON `RoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `Roles` (`NormalizedName`);

CREATE INDEX `IX_Sessions_UserId` ON `Sessions` (`UserId`);

CREATE INDEX `IX_UserAttributes_Key` ON `UserAttributes` (`Key`);

CREATE INDEX `IX_UserAttributes_UserId` ON `UserAttributes` (`UserId`);

CREATE INDEX `IX_UserClaims_UserId` ON `UserClaims` (`UserId`);

CREATE UNIQUE INDEX `IX_UserImage_UserId` ON `UserImage` (`UserId`);

CREATE INDEX `IX_UserLogins_UserId` ON `UserLogins` (`UserId`);

CREATE INDEX `IX_UserRoles_RoleId` ON `UserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `Users` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `Users` (`NormalizedUserName`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20241220225256_Initial', '8.0.11');

COMMIT;

