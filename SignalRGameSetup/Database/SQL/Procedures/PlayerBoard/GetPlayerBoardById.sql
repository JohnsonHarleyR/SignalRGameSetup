CREATE PROCEDURE [dbo].GetPlayerBoardById
(@BoardId INT)
AS
	SELECT * FROM [dbo].PlayerBoard
	WHERE BoardId = @BoardId;
GO