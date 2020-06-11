USE [master]

IF db_id('TabloidMVC') IS NULl
  CREATE DATABASE [TabloidMVC]
GO

USE [TabloidMVC]
GO


DROP TABLE IF EXISTS [PostReaction];
DROP TABLE IF EXISTS [Reaction];
DROP TABLE IF EXISTS [PostTag];
DROP TABLE IF EXISTS [Tag];
DROP TABLE IF EXISTS [Comment];
DROP TABLE IF EXISTS [Post];
DROP TABLE IF EXISTS [Category];
DROP TABLE IF EXISTS [Subscription];
DROP TABLE IF EXISTS [UserProfile];
DROP TABLE IF EXISTS [UserType];
GO


CREATE TABLE [UserType] (
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(20)
)

CREATE TABLE [UserProfile] (
  [Id] integer PRIMARY KEY IDENTITY,
  [DisplayName] nvarchar(50),
  [FirstName] nvarchar(50),
  [LastName] nvarchar(50),
  [Email] nvarchar(555),
  [CreateDataTime] datetime,
  [LastLoginDateTime] datetime,
  [ImageLocation] nvarchar(255),
  [UserTypeId] integer,

  CONSTRAINT [FK_User_UserType] FOREIGN KEY ([UserTypeId]) REFERENCES [UserType] ([Id])
)

CREATE TABLE [Subscription] (
  [Id] integer PRIMARY KEY IDENTITY,
  [SubscriberUserProfileId] integer,
  [ProviderUserProfileId] integer,
  [BeginDateTime] datetime,
  [EndDateTime] datetime,

  CONSTRAINT [FK_Subscription_UserProfile_Subscriber] FOREIGN KEY ([SubscriberUserProfileId])
	REFERENCES [UserProfile] ([Id]),

  CONSTRAINT [FK_Subscription_UserProfile_Provider] FOREIGN KEY ([ProviderUserProfileId])
	REFERENCES [UserProfile] ([Id])
)

CREATE TABLE [Category] (
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(50)
)

CREATE TABLE [Post] (
  [Id] integer PRIMARY KEY IDENTITY,
  [Title] nvarchar(255),
  [Content] text,
  [ImageLocation] nvarchar(255),
  [CreateDataTime] datetime,
  [PublishDateTime] datetime,
  [IsApproved] bit,
  [CategoryId] integer,
  [UserProfileId] integer,

  CONSTRAINT [FK_Post_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]),
  CONSTRAINT [FK_Post_UserProfile] FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
)

CREATE TABLE [Comment] (
  [Id] integer PRIMARY KEY IDENTITY,
  [PostId] integer,
  [UserProfileId] integer,
  [Subject] nvarchar(255),
  [Content] text,
  [CreateDataTime] datetime,

  CONSTRAINT [FK_Comment_Post] FOREIGN KEY ([PostId]) REFERENCES [Post] ([Id]),
  CONSTRAINT [FK_Comment_UserProfile] FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
)

CREATE TABLE [Tag] (
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(50)
)

CREATE TABLE [PostTag] (
  [id] integer PRIMARY KEY IDENTITY,
  [PostId] integer,
  [TagId] integer,
  
  CONSTRAINT [FK_PostTag_Post] FOREIGN KEY ([PostId]) REFERENCES [Post] ([Id]),
  CONSTRAINT [FK_PostTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag] ([Id])
)

CREATE TABLE [Reaction] (
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(50),
  [ImageLocation] nvarchar(255)
)

CREATE TABLE [PostReaction] (
  [Id] integer PRIMARY KEY IDENTITY,
  [PostId] integer,
  [ReactionId] integer,
  [UserProfileId] integer,

  CONSTRAINT [FK_PostReaction_Post] FOREIGN KEY ([PostId]) REFERENCES [Post] ([Id]),
  CONSTRAINT [FK_PostReaction_Reaction] FOREIGN KEY ([ReactionId]) REFERENCES [Reaction] ([Id]),
  CONSTRAINT [FK_PostReaction_UserProfile] FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
)
GO
