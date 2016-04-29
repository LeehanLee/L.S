
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/09/2016 10:00:20
-- Generated from EDMX file: F:\Lex\Lex.Model\DBModel\CommonCore.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LSContext];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Dep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dep];
GO
IF OBJECT_ID(N'[dbo].[DepUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dep_User];
GO
IF OBJECT_ID(N'[dbo].[Menu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menu];
GO
IF OBJECT_ID(N'[dbo].[MenuDep]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menu_Dep];
GO
IF OBJECT_ID(N'[dbo].[MenuRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menu_Role];
GO
IF OBJECT_ID(N'[dbo].[MenuUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menu_User];
GO
IF OBJECT_ID(N'[dbo].[Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role];
GO
IF OBJECT_ID(N'[dbo].[RoleUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role_User];
GO
IF OBJECT_ID(N'[dbo].[SysUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sys_User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Dep_Users'
CREATE TABLE [dbo].[DepUser] (
    [ID] nvarchar(36)  NOT NULL,
    [DepID] nvarchar(16)  NOT NULL,
    [UserID] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'Menus'
CREATE TABLE [dbo].[Menu] (
    [ID] varchar(36)  NOT NULL,
    [Text] nvarchar(50)  NULL,
    [Url] nvarchar(50)  NULL,
    [Position] int  NULL,
    [IsEnable] bit  NULL,
    [AddDate] datetime  NULL,
    [AddBy] nvarchar(10)  NULL,
    [LastUpdateDate] datetime  NULL,
    [LastUpdateBy] nvarchar(10)  NULL,
    [ParentID] varchar(50)  NULL
);
GO

-- Creating table 'Menu_Deps'
CREATE TABLE [dbo].[MenuDep] (
    [ID] nvarchar(36)  NOT NULL,
    [MenuID] nvarchar(16)  NOT NULL,
    [DepID] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'Menu_Roles'
CREATE TABLE [dbo].[MenuRole] (
    [ID] nvarchar(36)  NOT NULL,
    [MenuID] nvarchar(16)  NOT NULL,
    [RoleID] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'Menu_Users'
CREATE TABLE [dbo].[MenuUser] (
    [ID] nvarchar(36)  NOT NULL,
    [MenuID] nvarchar(16)  NOT NULL,
    [UserID] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Role] (
    [ID] nvarchar(36)  NOT NULL,
    [Name] nvarchar(10)  NOT NULL,
    [Remark] nvarchar(20)  NULL
);
GO

-- Creating table 'Role_User'
CREATE TABLE [dbo].[RoleUser] (
    [ID] nvarchar(36)  NOT NULL,
    [RoleID] nvarchar(16)  NOT NULL,
    [UserID] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'Sys_Users'
CREATE TABLE [dbo].[SysUser] (
    [ID] nvarchar(36)  NOT NULL,
    [LoginName] nvarchar(10)  NULL,
    [Password] nvarchar(50)  NULL,
    [Mobile] nvarchar(13)  NULL,
    [Birthday] datetime  NULL,
    [Address] nvarchar(50)  NULL,
    [IsEnable] bit  NULL,
    [Remark] nvarchar(50)  NULL
);
GO

-- Creating table 'Deps'
CREATE TABLE [dbo].[Dep] (
    [ID] nvarchar(36)  NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Remark] nvarchar(100)  NOT NULL,
    [ParentID] nvarchar(20)  NULL,
    [MenuID] nvarchar(300)  NULL,
    [MenuName] nvarchar(300)  NULL,
    [UserID] nvarchar(300)  NULL,
    [UserName] nvarchar(300)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Dep_Users'
ALTER TABLE [dbo].[DepUser]
ADD CONSTRAINT [PK_DepUser]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Menus'
ALTER TABLE [dbo].[Menu]
ADD CONSTRAINT [PK_Menu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Menu_Deps'
ALTER TABLE [dbo].[MenuDep]
ADD CONSTRAINT [PK_MenuDep]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Menu_Roles'
ALTER TABLE [dbo].[MenuRole]
ADD CONSTRAINT [PK_MenuRole]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Menu_Users'
ALTER TABLE [dbo].[MenuUser]
ADD CONSTRAINT [PK_MenuUser]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Roles'
ALTER TABLE [dbo].[Role]
ADD CONSTRAINT [PK_Role]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Role_User'
ALTER TABLE [dbo].[RoleUser]
ADD CONSTRAINT [PK_RoleUser]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Sys_Users'
ALTER TABLE [dbo].[SysUser]
ADD CONSTRAINT [PK_SysUser]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Deps'
ALTER TABLE [dbo].[Dep]
ADD CONSTRAINT [PK_Dep]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------