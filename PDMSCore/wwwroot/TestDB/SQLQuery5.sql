

retailer=	2
navid	=	6

exec GetPageContent	2, 6, 'en'

exec GetPanelsInfo	2, 6, 'en'
exec GetPageInfo	2, 2, 'en'



SELECT *
FROM	PanelsContent



exec MSTest 2, 6, 'en'

ALTER PROCEDURE MSTest
	@RetailerID int,
	@NavID int,
   	@LanguageID varchar(5)
AS
BEGIN
	DECLARE @PageID int

	SELECT	TOP 1 @PageID = pc.PageID
	FROM    PanelsContent pc
	WHERE   pc.RetailerID = @RetailerID
	AND		pc.NavID = @NavID

	exec GetPageContent	@RetailerID, @NavID, @LanguageID
	exec GetPanelsInfo	@RetailerID, @PageID, @LanguageID
	exec GetPageInfo	@RetailerID, @PageID, @LanguageID
END;


SELECT	*
FROM	RetailerPages rp

SELECT	*
FROM	Pages