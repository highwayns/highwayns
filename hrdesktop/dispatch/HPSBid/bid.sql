USE [HPS]
GO

/****** Object:  Table [dbo].[機構]    Script Date: 2017/06/17 22:17:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[機構](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Cname] [nvarchar](255) NULL,
	[name] [nvarchar](50) NULL,
	[postcode] [nvarchar](8) NULL,
	[address] [nvarchar](100) NULL,
	[tel] [nvarchar](50) NULL,
	[FAX] [nvarchar](255) NULL,
	[kind] [nvarchar](255) NULL,
	[FORMAT] [nvarchar](255) NULL,
	[scale] [nvarchar](50) NULL,
	[CYMD] [datetime] NULL,
	[other] [nvarchar](255) NULL,
	[mail] [nvarchar](100) NULL,
	[web] [nvarchar](255) NULL,
	[jCNAME] [nvarchar](255) NULL,
	[createtime] [datetime] NULL,
	[subscripted] [nvarchar](1) NULL,
	[UserID] [varchar](20) NULL,
	[入札可能案件] [int] NULL,
	[案件登録数] [int] NULL,
	[入札結果数] [int] NULL,
 CONSTRAINT [Govement_PK] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[プロジェクト]    Script Date: 2017/06/18 8:44:31 ******/

CREATE TABLE [dbo].[プロジェクト](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Cname] [nvarchar](255) NULL,
	[name] [nvarchar](50) NULL,
	[postcode] [nvarchar](8) NULL,
	[address] [nvarchar](100) NULL,
	[tel] [nvarchar](50) NULL,
	[FAX] [nvarchar](255) NULL,
	[kind] [nvarchar](255) NULL,
	[FORMAT] [nvarchar](255) NULL,
	[scale] [nvarchar](50) NULL,
	[CYMD] [datetime] NULL,
	[other] [nvarchar](255) NULL,
	[mail] [nvarchar](100) NULL,
	[web] [nvarchar](255) NULL,
	[jCNAME] [nvarchar](255) NULL,
	[createtime] [datetime] NULL,
	[subscripted] [nvarchar](1) NULL,
	[UserID] [varchar](20) NULL,
	[入札形式] [nvarchar](50) NULL,
	[公示日] [datetime] NULL,
	[入札内容] [nvarchar](1000) NULL,
 CONSTRAINT [Project_PK] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
