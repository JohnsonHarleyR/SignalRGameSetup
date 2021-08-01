USE [SignalRGame]
GO

/****** Object:  Table [dbo].[GameSetup]    Script Date: 8/1/2021 12:13:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GameSetup](
	[GameCode] [varchar](50) NOT NULL,
	[Players] [varchar](max) NULL,
	[Watchers] [varchar](max) NULL,
	[AllowAudience] [bit] NOT NULL,
 CONSTRAINT [PK_GameSetup] PRIMARY KEY CLUSTERED 
(
	[GameCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


