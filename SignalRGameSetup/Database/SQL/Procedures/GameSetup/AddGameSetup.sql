CREATE PROCEDURE [dbo].AddGameSetup
(@GameCode VARCHAR(50), @Players VARCHAR(MAX),
@Watchers VARCHAR(MAX), @AllowAudience BIT)
AS
	INSERT INTO [dbo].GameSetup
	(GameCode, Players, Watchers, AllowAudience)
	VALUES
	(@GameCode, @Players, @Watchers, @AllowAudience);
GO