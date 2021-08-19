CREATE PROCEDURE [dbo].DeleteBattleShipsGame
(@GameCode VARCHAR(50))
AS
	DELETE FROM [dbo].Game
	WHERE GameCode = @GameCode;
GO