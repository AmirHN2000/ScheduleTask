IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(80) NOT NULL,
    [Family] nvarchar(80) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Times] (
    [Name] nvarchar(50) NOT NULL,
    [Hour] tinyint NOT NULL,
    [Minute] tinyint NOT NULL,
    CONSTRAINT [PK_Times] PRIMARY KEY ([Name])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Tasks] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(500) NOT NULL,
    [Status] int NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [CreateDate] datetime2 NOT NULL,
    [ModifyDate] datetime2 NULL,
    [FinishDate] datetime2 NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tasks_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'07351063-71c9-48bc-9a84-60c1e2711ca1', N'50c5804e-2839-4f3d-9cf0-cabf0c80488d', N'Admin', NULL),
(N'a597799a-fb1b-44b2-939f-48731dec318e', N'8f054f6a-e082-4db7-b2e6-8a5e3d80b43f', N'User', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Family', N'LockoutEnabled', N'LockoutEnd', N'Name', N'NormalizedUserName', N'PasswordHash', N'SecurityStamp', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] ON;
INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Family], [LockoutEnabled], [LockoutEnd], [Name], [NormalizedUserName], [PasswordHash], [SecurityStamp], [UserName])
VALUES (N'07351063-71c9-48bc-9a84-60c1e27111ca', 0, N'12b2486e-802d-48d2-9349-d4c48cbfa44e', N'admin', CAST(0 AS bit), NULL, N'admin', N'ADMIN', N'AQAAAAEAACcQAAAAEOacqtJX5VRYz8/FZ+fkvAncB9R4WARR2j32jMo3t1+oX2+Jb99KHJPRsV4ElRYWyA==', N'TJJ3A4MT57QJJUADYYUDL7ZMUVDQDXWR', N'admin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Family', N'LockoutEnabled', N'LockoutEnd', N'Name', N'NormalizedUserName', N'PasswordHash', N'SecurityStamp', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Name', N'Hour', N'Minute') AND [object_id] = OBJECT_ID(N'[Times]'))
    SET IDENTITY_INSERT [Times] ON;
INSERT INTO [Times] ([Name], [Hour], [Minute])
VALUES (N'LimitedTimeInsertTask', CAST(12 AS tinyint), CAST(0 AS tinyint));
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Name', N'Hour', N'Minute') AND [object_id] = OBJECT_ID(N'[Times]'))
    SET IDENTITY_INSERT [Times] OFF;
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Tasks_UserId] ON [Tasks] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211106184518_init', N'5.0.9');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Tasks] ADD [Hour] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);
GO

ALTER TABLE [Tasks] ADD [Minute] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);
GO

UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N'd3d64608-fc7c-4324-b114-24f905a9fbc3', [NormalizedName] = N'ADMIN'
WHERE [Id] = N'07351063-71c9-48bc-9a84-60c1e2711ca1';
SELECT @@ROWCOUNT;

GO

UPDATE [AspNetRoles] SET [ConcurrencyStamp] = N'34b85ea5-7dce-4248-be50-9f7f7fed1613', [NormalizedName] = N'USER'
WHERE [Id] = N'a597799a-fb1b-44b2-939f-48731dec318e';
SELECT @@ROWCOUNT;

GO

UPDATE [AspNetUsers] SET [ConcurrencyStamp] = N'2bd515e7-c0a8-46f0-8692-d0e2ace31255'
WHERE [Id] = N'07351063-71c9-48bc-9a84-60c1e27111ca';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211117070236_changeNewTask', N'5.0.9');
GO

COMMIT;
GO

