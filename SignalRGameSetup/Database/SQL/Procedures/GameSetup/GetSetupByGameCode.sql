CREATE PROCEDURE [dbo].GetSetupByGameCode
(@GameCode VARCHAR(50))
AS
	SELECT * FROM [dbo].GameSetup
	WHERE GameCode = @GameCode;
GO