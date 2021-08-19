CREATE PROCEDURE [dbo].GetPlayerBoardByInfo
(@ParticipantId VARCHAR(50), @GameCode VARCHAR(50))
AS
	SELECT * FROM [dbo].PlayerBoard
	WHERE ParticipantId = @ParticipantId AND GameCode = @GameCode;
GO