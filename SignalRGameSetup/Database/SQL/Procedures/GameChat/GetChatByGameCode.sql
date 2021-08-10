CREATE PROCEDURE [dbo].GetChatByGameCode
(@GameCode VARCHAR(50))
AS
	SELECT * FROM [dbo].GameChat
	WHERE GameCode = @GameCode;
GO