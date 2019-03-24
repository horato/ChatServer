CREATE TABLE [system].[db_Changescripts]
(
	[Id] uniqueidentifier not null PRIMARY KEY,
	[Version] int not null,
	[PreviousVersion] int not null,
	[Note] nvarchar(1000) not null,
	[Author] nvarchar(100) not null,
	[Date] datetime not null
);

INSERT INTO [system].[db_Changescripts] ([Id], [Version], [PreviousVersion], [Note], [Author], [Date])
(SELECT NEWID(), 1, 0, 'Create changescripts table', 'horato', GETDATE())
