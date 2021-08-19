CREATE PROCEDURE [dbo].AddBattleShipsGame
(@GameCode VARCHAR(50), @PlayerOne VARCHAR(50), @PlayerOneBoard INT,
@PlayerTwo VARCHAR(50), @PlayerTwoBoard INT, @Information NVARCHAR(MAX))
AS
	INSERT INTO [dbo].Game
	(GameCode, PlayerOne, PlayerOneBoard,
	PlayerTwo, PlayerTwoBoard, Information)
	VALUES
	(@GameCode, @PlayerOne, @PlayerOneBoard,
	@PlayerTwo, @PlayerTwoBoard, @Information);
GO