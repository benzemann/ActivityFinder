-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeppe Bentzen
-- Create date: 16-07-2017
-- Description:	Find activites close to the given latlong
-- =============================================
CREATE PROCEDURE FindCloseActivities 
	-- Add the parameters for the stored procedure here
	@latitude float, 
	@longitude float,
	@distance float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM(
		SELECT *,(((acos(sin((@latitude*pi()/180)) * 
		sin((Latitude*pi()/180))+cos((@latitude*pi()/180)) * 
		cos((Latitude*pi()/180)) * 
		cos(((@longitude - Longitude)*pi()/180))))*180/pi())*60*1.1515*1.609344) 
		as distance FROM dbo.Activities) t
	WHERE distance <= @distance

END
GO
