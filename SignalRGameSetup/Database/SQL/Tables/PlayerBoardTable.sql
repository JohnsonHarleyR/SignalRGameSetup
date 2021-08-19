USE [BattleShips]
GO

/****** Object:  Table [dbo].[PlayerBoard]    Script Date: 8/19/2021 5:51:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlayerBoard](
	[BoardId] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantId] [varchar](50) NOT NULL,
	[GameCode] [varchar](50) NOT NULL,
	[Positions] [nvarchar](max) NULL,
	[Ships] [nvarchar](max) NULL,
 CONSTRAINT [PK_PlayerBoard] PRIMARY KEY CLUSTERED 
(
	[BoardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


