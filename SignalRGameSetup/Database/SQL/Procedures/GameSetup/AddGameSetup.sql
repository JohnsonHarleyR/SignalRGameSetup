CREATE PROCEDURE [dbo].AddGameSetup
(@GameCode VARCHAR(50), @Players VARCHAR(MAX),
@Watchers VARCHAR(MAX), @AllowAudience BIT,
@LeaveInDatabase BIT)
AS
	INSERT INTO [dbo].GameSetup
	(GameCode, Players, Watchers, AllowAudience, LeaveInDatabase)
	VALUES
	(@GameCode, @Players, @Watchers, @AllowAudience, @LeaveInDatabase);
GO