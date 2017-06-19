USE [HPS]
GO

/****** Object:  Table [dbo].[会社]    Script Date: 2017/06/17 13:34:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[会社](
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
 CONSTRAINT [company_PK] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
