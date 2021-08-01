CREATE PROCEDURE [dbo].UpdateGameSetup
(@GameCode VARCHAR(50), @Players VARCHAR(MAX),
@Watchers VARCHAR(MAX), @AllowAudience BIT)
AS
	UPDATE [dbo].GameSetup
	SET
	Players = @Players, Watchers = @Watchers,
	AllowAudience = @AllowAudience
	WHERE
	GameCode = @GameCode;
GO