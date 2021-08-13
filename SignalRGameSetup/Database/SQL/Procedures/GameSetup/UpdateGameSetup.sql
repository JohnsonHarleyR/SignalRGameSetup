CREATE PROCEDURE [dbo].UpdateGameSetup
(@GameCode VARCHAR(50), @Players VARCHAR(MAX),
@Watchers VARCHAR(MAX), @AllowAudience BIT,
@LeaveInDatabase BIT)
AS
	UPDATE [dbo].GameSetup
	SET
	Players = @Players, Watchers = @Watchers,
	AllowAudience = @AllowAudience,
	LeaveInDatabase = @LeaveInDatabase
	WHERE
	GameCode = @GameCode;
GO