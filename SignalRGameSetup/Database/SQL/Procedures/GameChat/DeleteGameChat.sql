CREATE PROCEDURE [dbo].DeleteGameChat
(@GameCode VARCHAR(50))
AS
	DELETE FROM [dbo].GameChat
	WHERE GameCode = @GameCode;
GO