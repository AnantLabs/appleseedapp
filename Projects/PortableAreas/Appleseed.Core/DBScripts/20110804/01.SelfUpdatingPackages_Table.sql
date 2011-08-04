/*
   jueves, 04 de agosto de 201111:30:35 a.m.
   User: sa
   Server: .
   Database: AppleseedGoogle
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.SelfUpdatingPackages
	(
	Id int NOT NULL IDENTITY (1, 1),
	PackageId nvarchar(MAX) NULL,
	PackageVersion nvarchar(MAX) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.SelfUpdatingPackages ADD CONSTRAINT
	PK_SelfUpdatingPackages PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.SelfUpdatingPackages SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.SelfUpdatingPackages', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.SelfUpdatingPackages', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.SelfUpdatingPackages', 'Object', 'CONTROL') as Contr_Per 