USE [BattleShips]
GO

/****** Object:  Table [dbo].[Game]    Script Date: 8/19/2021 7:07:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Game](
	[GameCode] [varchar](50) NOT NULL,
	[PlayerOne] [varchar](50) NOT NULL,
	[PlayerOneBoard] [int] NOT NULL,
	[PlayerTwo] [varchar](50) NOT NULL,
	[PlayerTwoBoard] [int] NOT NULL,
	[Information] [nvarchar](max) NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[GameCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


