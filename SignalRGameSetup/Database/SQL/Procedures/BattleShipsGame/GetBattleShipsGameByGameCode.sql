CREATE PROCEDURE [dbo].GetBattleShipsGameByGameCode
(@GameCode VARCHAR(50))
AS
	SELECT * FROM [dbo].Game
	WHERE GameCode = @GameCode;
GO