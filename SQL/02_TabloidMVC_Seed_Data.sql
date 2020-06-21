USE [TabloidMVC]
GO

SET IDENTITY_INSERT [UserType] ON
INSERT INTO [UserType] ([ID], [Name]) VALUES (1, 'Author'), (2, 'Admin');
SET IDENTITY_INSERT [UserType] OFF


SET IDENTITY_INSERT [Category] ON
INSERT INTO [Category] ([Name]) 
VALUES ('Technology'), ('Close Magic'), ('Politics'), ('Science'), ('Improv'), 
	   ('Cthulhu Sightings'), ('History'), ('Home and Garden'), ('Entertainment'), 
	   ('Cooking'), ('Music'), ('Movies'), ('Regrets');
SET IDENTITY_INSERT [Category] OFF


INSERT INTO [Tag] ([Name])
VALUES ('C#'), ('JavaScript'), ('Cyclopean Terrors'), ('Family');

INSERT INTO [UserProfile] (
	[FirstName], [LastName], [DisplayName], [Email], [CreateDateTime], [ImageLocation], [UserTypeId])
VALUES ('Admina', 'Strator', 'admin', 'admin@example.com', '06-14-2020', NULL, 1);

INSERT INTO [Post] (
	[Title], [Content], [ImageLocation], [CreateDateTime], [PublishDateTime], [IsApproved], [CategoryId], [UserProfileId])
VALUES (
	'C# is the Best Language', 
'There are those' + char(10) + 'who do not believe' + char(10) + 'C# is the best.' + char(10) + 'They are wrong.',
    'https://gizmodiva.com/wp-content/uploads/2017/10/SCOTT-A-WOODWARD_1SW1943-1170x689.jpg','06-14-2020', '06-14-2020', 1, 1, 1);
