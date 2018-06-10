

-----------------------------------------------------

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


SELECT	distinct pg.PageID, pg.URL, lb.Label, lb.CompanyID
FROM	Pages pg
LEFT outer join Labels lb ON pg.LabelID = lb.LabelID
WHERE	1=1
AND		pg.PageID = 1
AND		lb.LanguageID = 'en'
AND		
(		lb.CompanyID = 1
OR		lb.CompanyID IS NULL )

----------------------------------------------------------------

GetPanelFields 1, 1, 'en'

ALTER PROCEDURE GetPanelFields  
	@PanelID int,  
	@CompanyID int,  
	@LanguageID varchar(5)  
AS  
BEGIN  
	SELECT	f.FieldID, f.FieldType, l.Label, pj.StringValue, pj.IntValue, pj.DateValue, pj.FileValue, pj.OtherRef  
	FROM	PanelsFields pf, Fields f, Labels l, Panels p, Projects pj  
	WHERE	1 = 1  
	AND		pf.FieldID = f.FieldID  
	AND		f.FieldID = l.LabelID  
	AND		p.PanelID = pf.PanelID  
	AND		pj.FieldID = f.FieldID  
	AND		pj.RetailerID = p.CompanyID  
	AND		p.CompanyID = @CompanyID  
	AND		pf.PanelID = @PanelID  
	AND		l.LanguageID = @LanguageID  
	ORDER BY [PredecessorFieldID]  
END;