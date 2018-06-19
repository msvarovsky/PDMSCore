select * from Fields

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





SELECT	distinct pg.PageID, pg.URL, lb.Label, lb.CompanyID
FROM	Pages pg
LEFT outer join Labels lb ON pg.LabelID = lb.LabelID
WHERE	1=1
AND		pg.PageID = 1
AND		lb.LanguageID = 'en'
AND		
(		lb.CompanyID = 1
OR		lb.CompanyID IS NULL )


SELECT * from Labels
INSERT INTO Labels	VALUES	(8,'en','Bazar',null)
INSERT INTO Labels	VALUES	(8,'cs','Ostatni',null)


INSERT INTO Labels	VALUES	(8,'cs','Ostatni',null)

SELECT * from MultiSelection
INSERT INTO MultiSelection	VALUES (1,3,8)

--delete from MultiSelection where LabelID = 7

UPDATE	Projects
SET		IntValue = 2
WHERE	FieldID = 3

SELECT * from Projects
SELECT * from Fields

SELECT	*
--SELECT	f.FieldID, f.FieldType, l.Label, pj.StringValue, pj.IntValue, pj.DateValue, pj.FileValue, pj.OtherRef
--, '-' as [--] , pj.*
FROM	PanelsFields pf, Fields f, Labels l, Panels p, Projects pj
WHERE	1 = 1
AND		pf.FieldID = f.FieldID
AND		f.FieldID = l.LabelID
AND		p.PanelID = pf.PanelID
AND		pj.FieldID = f.FieldID
AND		pj.RetailerID = p.CompanyID
AND		p.CompanyID = 1
AND		pf.PanelID = 1
AND		l.LanguageID = 'en'
ORDER BY	[PredecessorFieldID]


insert into PanelsFields values	(1,4,3)
SELECT	*	from PanelsFields 


SELECT	*
--SELECT	f.FieldID, f.FieldType, l.Label, pj.StringValue, pj.IntValue, pj.DateValue, pj.FileValue, pj.OtherRef
--, '-' as [--] , pj.*
FROM	PanelsFields pf
LEFT OUTER JOIN Fields f ON	pf.FieldID = f.FieldID
LEFT OUTER JOIN Labels l ON f.FieldID = l.LabelID
LEFT OUTER JOIN Panels p ON p.PanelID = pf.PanelID
LEFT OUTER JOIN Projects pj ON (pj.RetailerID	= p.CompanyID
							AND pj.FieldID		= f.FieldID)
WHERE	1 = 1
AND		p.CompanyID = 1
AND		pf.PanelID = 1
AND		l.LanguageID = 'en'
ORDER BY	[PredecessorFieldID]




DECLARE @temp TABLE
(
    FieldID		int,
    FieldType	nvarchar(100),
	Label		nvarchar(100),
	StringValue nvarchar(100),
	IntValue	int,
	DateValue	Date,
	FileValue	VARBINARY (MAX),
	OtherRef	int,
	MultiSelectItemID	int,
	PredecessorFieldID	int
)
INSERT INTO @temp
	SELECT	pf.FieldID, ISNULL(f.FieldType,'-na-') AS [FieldType], ISNULL(l.Label,'-na:fid' +  CAST(pf.FieldID AS VARCHAR(16))) AS 'Label', ISNULL(pj.StringValue,'') AS 'StringValue', pj.IntValue, pj.DateValue, pj.FileValue, pj.OtherRef,NULL,pf.PredecessorFieldID
	FROM	PanelsFields pf
	LEFT OUTER JOIN Fields f ON	pf.FieldID = f.FieldID
	LEFT OUTER JOIN Labels l ON f.FieldID = l.LabelID
	LEFT OUTER JOIN Panels p ON p.PanelID = pf.PanelID
	LEFT OUTER JOIN Projects pj ON (pj.RetailerID	= p.CompanyID
								AND pj.FieldID		= f.FieldID)
	WHERE	1 = 1
	AND		p.CompanyID = 1
	AND		pf.PanelID = 1
	AND		(l.LanguageID = 'en' OR l.LanguageID is null)
INSERT INTO @temp
	SELECT	NULL, (RTRIM(LTRIM(t.FieldType)) + '-item') AS 'FieldType', '-',l.Label, NULL, NULL, NULL, ms.ReferenceID, ms.ItemID, t.PredecessorFieldID
	--SELECT	NULL, t.FieldType, '-',l.Label, NULL, NULL, NULL, ms.ReferenceID, ms.ItemID
	FROM	@temp t
	FULL JOIN		MultiSelection ms ON t.OtherRef = ms.ReferenceID
	LEFT OUTER JOIN Labels l ON ms.LabelID = l.LabelID
	WHERE	l.LanguageID = 'en'
	
SELECT * from @temp
ORDER BY [PredecessorFieldID]





-----------

SELECT * from PanelsFields
SELECT * from Projects
SELECT * from Fields
SELECT * from Labels

