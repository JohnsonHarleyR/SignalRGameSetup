CREATE PROCEDURE [dbo].AddPlayerBoard
(@ParticipantId VARCHAR(50), @GameCode VARCHAR(50),
@Positions NVARCHAR(MAX), @Ships NVARCHAR(MAX))
AS
	INSERT INTO [dbo].PlayerBoard
	(ParticipantId, GameCode, Positions, Ships)
	VALUES
	(@ParticipantId, @GameCode, @Positions, @Ships);
GO