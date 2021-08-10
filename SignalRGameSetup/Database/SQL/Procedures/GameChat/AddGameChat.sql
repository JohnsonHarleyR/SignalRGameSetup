CREATE PROCEDURE [dbo].AddGameChat
(@GameCode VARCHAR(50), @ChatHtml NVARCHAR(MAX))
AS
	INSERT INTO [dbo].GameChat
	(GameCode, ChatHtml)
	VALUES
	(@GameCode, @ChatHtml);
GO