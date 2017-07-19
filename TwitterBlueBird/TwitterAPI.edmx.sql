
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/19/2017 14:24:21
-- Generated from EDMX file: c:\users\kokiri\documents\visual studio 2017\Projects\TwitterBlueBird\TwitterBlueBird\TwitterAPI.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TwitterBlueBird_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Tweets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tweets];
GO
IF OBJECT_ID(N'[dbo].[Words]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Words];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Tweets'
CREATE TABLE [dbo].[Tweets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Mood] nvarchar(max)  NULL
);
GO

-- Creating table 'Words'
CREATE TABLE [dbo].[Words] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [HappyCount] int  NOT NULL,
    [AngryCount] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Tweets'
ALTER TABLE [dbo].[Tweets]
ADD CONSTRAINT [PK_Tweets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Words'
ALTER TABLE [dbo].[Words]
ADD CONSTRAINT [PK_Words]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------