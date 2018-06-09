﻿select * from Fields

select * from Panels



SELECT	*

SELECT	*
FROM	Pages pg,
		Panels ps,
		Fields f,
		Labels l
WHERE	l.LanguageID = 'en'
AND		pg.Id = 1
AND		pg.OrganisationID = 1
AND		pg.PanelID = ps.Id
AND		ps.FieldID = f.Id
AND		f.Id = l.FieldID




GetPanelFromID 1, 1, 'cs'

ALTER PROCEDURE GetPanelFromID
    @PanelID int,
    @CompanyID int,
	@LanguageID varchar(5)
AS
BEGIN
	SELECT	distinct pg.PageID, pg.URL, lb.Label, lb.CompanyID
	FROM	Pages pg
	LEFT outer join Labels lb ON pg.LabelID = lb.LabelID
	WHERE	1=1
	AND		pg.PageID = @PanelID
	AND		lb.LanguageID = @LanguageID
	AND		
	(		lb.CompanyID = @CompanyID
	OR		lb.CompanyID IS NULL )
END;

