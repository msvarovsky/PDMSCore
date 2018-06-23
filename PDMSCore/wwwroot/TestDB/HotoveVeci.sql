
exec GetPageFields 1,1,1,'en'
-------------------------------------
ALTER PROCEDURE GetPageFields
--	Retailer Project, Page, Language, 
	@RetailerID int,
	@ProjectID int,
    @PageID int,
	@LanguageID varchar(5)
AS
BEGIN
	DECLARE @temp TABLE
	(
		PanelID		int,
		FieldID		int,
		FieldLabel	nvarchar(100),
		FieldType	nvarchar(100),
		StringValue nvarchar(4000),
		IntValue	int,
		DateValue	Date,
		FileValue	VARBINARY (MAX),
		MulitValue  VARCHAR(100),
		OtherRef	int,
		MultiSelectItemID	int,
		PredecessorFieldID	int
	)
	--Ziskam hodnoty projektu
	INSERT INTO @temp
	SELECT	pf.PanelID, pf.FieldID, NULL as 'FieldLabel', pf.FieldType
			,pf.StringValue, pf.IntValue, pf.DateValue, pf.FileValue, pf.MultiValue
			,pf.OtherRef ,null, null
	FROM	ProjectFields pf
	WHERE	pf.ProjectID = @ProjectID
	AND		pf.RetailerID = @RetailerID

	--Zde se pridaji obtions dropdown list boxu a radio buttonu
	INSERT INTO @temp
	SELECT	t.PanelID, ms.LabelID as 'FieldID', NULL as 'FieldLabel', (RTRIM(LTRIM(t.FieldType)) + '-item') as 'FieldType'
			,NULL as 'StringValue',NULL as 'IntValue',NULL as 'DateValue',NULL as 'FileValue',NULL as 'MultiValue'
			,t.FieldID as 'OtherRef', ms.ItemID as 'MultiSelectItemID', NULL as 'PredecessorFieldID'
	FROM	MultiSelection ms, @temp t
	WHERE	ms.ReferenceID = t.OtherRef

	--Zde se pridaji Labels ve spravnem jazyce
	SELECT	t.PanelID, t.FieldID, l.Label, t.FieldType, t.StringValue, t.IntValue, t.DateValue, t.FileValue, t.MulitValue
			, t.OtherRef, t.MultiSelectItemID, t.PredecessorFieldID
	FROM	@temp t
	LEFT OUTER JOIN	Labels l ON t.FieldID= l.LabelID
	AND		(l.LanguageID = @LanguageID	OR l.LanguageID IS NULL)
	AND		(l.CompanyID = @RetailerID	OR l.CompanyID IS NULL)
END;

---- Na hrani ----
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
------------------