CREATE PROCEDURE [dbo].UpdateBattleShipsGame
(@GameCode VARCHAR(50), @PlayerOne VARCHAR(50), @PlayerOneBoard INT,
@PlayerTwo VARCHAR(50), @PlayerTwoBoard INT, @Information NVARCHAR(MAX))
AS
	UPDATE [dbo].Game
	SET
	PlayerOne = @PlayerOne, PlayerOneBoard = @PlayerOneBoard,
	PlayerTwo = @PlayerTwo, PlayerTwoBoard = @PlayerTwoBoard, Information = @Information
	WHERE
	GameCode = @GameCode;
GO