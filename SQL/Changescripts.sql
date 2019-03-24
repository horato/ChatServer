---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------
GO

DECLARE @sourceVersion int;
DECLARE @targetVersion int;
DECLARE @currentVersion int;

SET @sourceVersion = 1;
SET @targetVersion = 2;
SELECT TOP 1 @currentVersion = [Version] FROM [system].[db_Changescripts] ORDER BY [Version] desc;

if (@currentVersion = @sourceVersion) 
begin
	BEGIN TRANSACTION

	insert into [system].[db_Changescripts] (Id, [Version], PreviousVersion, Note, Author, [Date])
	select NEWID(), @targetVersion, @sourceVersion, 'Add domain entities', 'horato', GETDATE()

	CREATE TABLE [chatServer].[History_MessageHistory]
	(
	 [Id] uniqueidentifier NOT NULL PRIMARY KEY,
	 [Version] int NOT NULL,
	 [Message] nvarchar(max) COLLATE Czech_CI_AS NOT NULL,
	 [MessageType] int NOT NULL,
	 [CreatedOn] datetime2 NOT NULL,
	 [FromId] uniqueidentifier NULL,
	 [ToId] uniqueidentifier NULL,
	 [RoomId] uniqueidentifier NULL
	);

	CREATE TABLE [chatServer].[Security_Permission]
	(
	 [Id] uniqueidentifier NOT NULL PRIMARY KEY,
	 [Version] int NOT NULL,
	 [PermissionType] int NOT NULL,
	 [RoleId] uniqueidentifier NOT NULL,
	 [CreatedOn] datetime2 NOT NULL,
	 [CreatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL,
	 [UpdatedOn] datetime2 NOT NULL,
	 [UpdatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL
	);

	CREATE TABLE [chatServer].[Security_Role]
	(
	 [Id] uniqueidentifier NOT NULL PRIMARY KEY,
	 [Version] int NOT NULL,
	 [Name] nvarchar(100) COLLATE Czech_CI_AS NOT NULL,
	 [UserRoomMembershipId] uniqueidentifier NOT NULL,
	 [CreatedOn] datetime2 NOT NULL,
	 [CreatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL,
	 [UpdatedOn] datetime2 NOT NULL,
	 [UpdatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL
	);

	CREATE TABLE [chatServer].[Main_Room]
	(
	 [Id] uniqueidentifier NOT NULL PRIMARY KEY,
	 [Version] int NOT NULL,
	 [Name] nvarchar(100) COLLATE Czech_CI_AS NOT NULL,
	 [CreatedOn] datetime2 NOT NULL,
	 [CreatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL,
	 [UpdatedOn] datetime2 NOT NULL,
	 [UpdatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL
	);

	CREATE TABLE [chatServer].[Main_User]
	(
	 [Id] uniqueidentifier NOT NULL PRIMARY KEY,
	 [Version] int NOT NULL,
	 [Name] nvarchar(50) COLLATE Czech_CI_AS NOT NULL,
	 [Login] nvarchar(50) COLLATE Czech_CI_AS NOT NULL,
	 [Password] nvarchar(255) COLLATE Czech_CI_AS NOT NULL,
	 [CreatedOn] datetime2 NOT NULL,
	 [CreatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL,
	 [UpdatedOn] datetime2 NOT NULL,
	 [UpdatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL
	);

	CREATE TABLE [chatServer].[Main_UserRoomMembership]
	(
	 [Id] uniqueidentifier NOT NULL PRIMARY KEY,
	 [Version] int NOT NULL,
	 [UserId] uniqueidentifier NOT NULL,
	 [RoomId] uniqueidentifier NOT NULL,
	 [CreatedOn] datetime2 NOT NULL,
	 [CreatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL,
	 [UpdatedOn] datetime2 NOT NULL,
	 [UpdatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL
	);

	CREATE TABLE [chatServer].[Main_UserRoomRestriction]
	(
	 [Id] uniqueidentifier NOT NULL PRIMARY KEY,
	 [Version] int NOT NULL,
	 [RestrictionType] int NOT NULL,
	 [IsPermanent] bit NOT NULL,
	 [EffectiveTo] datetime2 NULL,
	 [UserRoomMembershipId] uniqueidentifier NOT NULL,
	 [CreatedOn] datetime2 NOT NULL,
	 [CreatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL,
	 [UpdatedOn] datetime2 NOT NULL,
	 [UpdatedBy] nvarchar(255) COLLATE Czech_CI_AS NOT NULL
	);

	ALTER TABLE [chatServer].[Main_User] ADD CONSTRAINT UQ__Main_Use__737584F65849A8B4 UNIQUE (Name)
	ALTER TABLE [chatServer].[Main_User] ADD CONSTRAINT UQ__Main_Use__5E55825B4E3A185F UNIQUE (Login)
	ALTER TABLE [chatServer].[History_MessageHistory] ADD CONSTRAINT FK_MessageHistory_Room FOREIGN KEY (RoomId) REFERENCES [chatServer].[Main_Room] ([Id])
	ALTER TABLE [chatServer].[History_MessageHistory] ADD CONSTRAINT FK_MessageHistory_From FOREIGN KEY (FromId) REFERENCES [chatServer].[Main_User] ([Id])
	ALTER TABLE [chatServer].[History_MessageHistory] ADD CONSTRAINT FK_MessageHistory_To FOREIGN KEY (ToId) REFERENCES [chatServer].[Main_User] ([Id])
	ALTER TABLE [chatServer].[Security_Permission] ADD CONSTRAINT FK_Permission_Role FOREIGN KEY (RoleId) REFERENCES [chatServer].[Security_Role] ([Id])
	ALTER TABLE [chatServer].[Security_Role] ADD CONSTRAINT FK_Role_UserRoomMembership FOREIGN KEY (UserRoomMembershipId) REFERENCES [chatServer].[Main_UserRoomMembership] ([Id])
	ALTER TABLE [chatServer].[Main_UserRoomMembership] ADD CONSTRAINT FK_UserRoomMembership_Room FOREIGN KEY (RoomId) REFERENCES [chatServer].[Main_Room] ([Id])
	ALTER TABLE [chatServer].[Main_UserRoomMembership] ADD CONSTRAINT FK_UserRoomMembership_User FOREIGN KEY (UserId) REFERENCES [chatServer].[Main_User] ([Id])
	ALTER TABLE [chatServer].[Main_UserRoomRestriction] ADD CONSTRAINT FK_UserRoomRestriction_UserRoomMembership FOREIGN KEY (UserRoomMembershipId) REFERENCES [chatServer].[Main_UserRoomMembership] ([Id])

	CREATE NONCLUSTERED INDEX IDX_MessageHistory_From ON [chatServer].[History_MessageHistory] (FromId ASC)
	CREATE NONCLUSTERED INDEX IDX_MessageHistory_To ON [chatServer].[History_MessageHistory] (ToId ASC)
	CREATE NONCLUSTERED INDEX IDX_MessageHistory_Room ON [chatServer].[History_MessageHistory] (RoomId ASC)
	CREATE NONCLUSTERED INDEX IDX_Permission_Role ON [chatServer].[Security_Permission] (RoleId ASC)
	CREATE NONCLUSTERED INDEX IDX_Role_UserRoomMembership ON [chatServer].[Security_Role] (UserRoomMembershipId ASC)
	CREATE NONCLUSTERED INDEX IDX_UserRoomMembership_User ON [chatServer].[Main_UserRoomMembership] (UserId ASC)
	CREATE NONCLUSTERED INDEX IDX_UserRoomMembership_Room ON [chatServer].[Main_UserRoomMembership] (RoomId ASC)
	CREATE NONCLUSTERED INDEX IDX_UserRoomRestriction_UserRoomMembership ON [chatServer].[Main_UserRoomRestriction] (UserRoomMembershipId ASC)

	COMMIT TRANSACTION
end

---------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------