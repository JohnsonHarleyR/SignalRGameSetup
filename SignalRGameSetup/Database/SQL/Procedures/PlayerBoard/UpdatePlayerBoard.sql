CREATE PROCEDURE [dbo].UpdatePlayerBoard
(@BoardId INT, @ParticipantId VARCHAR(50), @GameCode VARCHAR(50),
@Positions NVARCHAR(MAX), @Ships NVARCHAR(MAX))
AS
	UPDATE [dbo].GameSetup
	SET
	ParticipantId = @ParticipantId, GameCode = @GameCode,
	Positions = @Positions, Ships = @Ships
	WHERE
	BoardId = @BoardId;
GO