CREATE PROCEDURE [dbo].DeleteGameSetup
(@GameCode VARCHAR(50))
AS
	DELETE FROM [dbo].GameSetup
	WHERE GameCode = @GameCode;
GO