select * from RetailerPages
select * from PagePanels
select * from PanelFields
select * from FieldsValues

select * from PanelFields
insert into PanelFields (PanelID, FieldID,PredecessorFieldID) VALUES	('','','')

select * from PagePanels
insert into PagePanels (PageID, PanelID, PredecessorPanelID) VALUES	(1,1,0)

insert into RetailerPages (RetailerID, PageID) VALUES (1,1)



SELECT	*
SELECT	rp.RetailerID, pp.PageID, pf.PanelID, 
		fv.FieldID, fv.ProjectID, fv.StringValue, fv.IntValue, fv.DateValue, fv.FileValue, fv.OtherRef, fv.Effdt, fv.[Version]
FROM	RetailerPages rp
LEFT OUTER JOIN PagePanels pp ON rp.PageID = pp.PageID
LEFT OUTER JOIN PanelFields pf ON pp.PanelID = pf.PanelID
LEFT OUTER JOIN FieldsValues fv ON pf.FieldID = fv.FieldID 
								AND fv.RetailerID = rp.RetailerID


SELECT * FROM FieldsValues

-------------------------------------------------------------------------
--drop view ProjectFields 
CREATE VIEW ProjectFields AS
SELECT	rp.RetailerID, pp.PageID, pf.PanelID, 
		fv.FieldID, pf.FieldType, fv.ProjectID, fv.StringValue, fv.IntValue, fv.DateValue, fv.FileValue, fv.MultiValue, fv.OtherRef, fv.Effdt, fv.[Version]
		,pp.PredecessorPanelID, pf.PredecessorFieldID
FROM	RetailerPages rp
LEFT OUTER JOIN PagePanels pp ON rp.PageID = pp.PageID
LEFT OUTER JOIN PanelFields pf ON pp.PanelID = pf.PanelID
LEFT OUTER JOIN FieldsValues fv ON pf.FieldID = fv.FieldID 
								AND fv.RetailerID = rp.RetailerID
-------------------------------------------------------------------------

SELECT * from Labels


SELECT	*
FROM	ProjectFields pf
WHERE	pf.RetailerID	=	1
AND		pf.PageID		=	1
ORDER BY	pf.PredecessorPanelID, pf.PredecessorFieldID


--	Retailer Project, Page, Language, 


CREATE PROCEDURE GetPageFields
	@RetailerID int,
	@ProjectID int,
    @PageID int,
	@LanguageID varchar(5),
AS
BEGIN


DECLARE @temp TABLE
(
	PageID		int,
	PanelID		int,
	FieldID		int,
	FieldLabel	nvarchar(100),
	FieldType	nvarchar(100),
	StringValue nvarchar(100),
	IntValue	int,
	DateValue	Date,
	FileValue	VARBINARY (MAX),
	MulitValue  VARCHAR(100),
	OtherRef	int,
	MultiSelectItemID	int,
	PredecessorFieldID	int
)
INSERT INTO @temp
SELECT	pf.PageID, pf.PanelID, pf.FieldID, 'label-tbd1' as 'Label', pf.FieldType
		,pf.StringValue, pf.IntValue, pf.DateValue, pf.FileValue, pf.MultiValue
		,pf.OtherRef ,null, null
FROM	ProjectFields pf
WHERE	pf.ProjectID = 1
AND		pf.RetailerID = 2

INSERT INTO @temp
SELECT	t.PageID, t.PanelID, ms.LabelID as 'FieldID', 'fieldlabel-tbd' as 'FieldLabel', (RTRIM(LTRIM(t.FieldType)) + '-item') as 'FieldType'
		,NULL as 'StringValue',NULL as 'IntValue',NULL as 'DateValue',NULL as 'FileValue',NULL as 'MultiValue'
		,t.FieldID as 'OtherRef', ms.ItemID as 'MultiSelectItemID', NULL as 'PredecessorFieldID'
FROM	MultiSelection ms, @temp t
WHERE	ms.ReferenceID = t.OtherRef

SELECT	t.PageID, t.PanelID, t.FieldID, l.Label, t.FieldType, t.StringValue, t.IntValue, t.DateValue, t.FileValue, t.MulitValue
		, t.OtherRef, t.MultiSelectItemID, t.PredecessorFieldID
FROM	@temp t
LEFT OUTER JOIN	Labels l ON t.FieldID= l.LabelID
AND		(l.LanguageID = 'cs'	OR l.LanguageID IS NULL)
AND		(l.CompanyID = 1		OR l.CompanyID IS NULL)





SELECT * from MultiSelection
SELECT * from Fields


----------------------------------------

