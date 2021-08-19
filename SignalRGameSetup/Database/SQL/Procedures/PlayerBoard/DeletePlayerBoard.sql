CREATE PROCEDURE [dbo].DeletePlayerBoard
(@BoardId INT)
AS
	DELETE FROM [dbo].PlayerBoard
	WHERE BoardId = @BoardId;
GO