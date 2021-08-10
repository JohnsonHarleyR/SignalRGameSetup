CREATE PROCEDURE [dbo].UpdateGameChat
(@GameCode VARCHAR(50), @ChatHtml NVARCHAR(MAX))
AS
	UPDATE [dbo].GameChat
	SET
	ChatHtml = @ChatHtml
	WHERE
	GameCode = @GameCode;
GO