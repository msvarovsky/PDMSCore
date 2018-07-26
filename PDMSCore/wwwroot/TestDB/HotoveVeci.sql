
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
		PageID		int,
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
	SELECT	pf.PageID, pf.PanelID, pf.FieldID, NULL as 'FieldLabel', pf.FieldType
			,pf.StringValue, pf.IntValue, pf.DateValue, pf.FileValue, pf.MultiValue
			,pf.OtherRef ,null, null
	FROM	ProjectFields pf
	WHERE	pf.ProjectID = @ProjectID
	AND		pf.RetailerID = @RetailerID
	AND		pf.PageID = @PageID

	--Zde se pridaji obtions dropdown list boxu a radio buttonu
	INSERT INTO @temp
	SELECT	t.PageID, t.PanelID, ms.LabelID as 'FieldID', NULL as 'FieldLabel', (RTRIM(LTRIM(t.FieldType)) + '-item') as 'FieldType'
			,NULL as 'StringValue',NULL as 'IntValue',NULL as 'DateValue',NULL as 'FileValue',NULL as 'MultiValue'
			,t.FieldID as 'OtherRef', ms.ItemID as 'MultiSelectItemID', NULL as 'PredecessorFieldID'
	FROM	MultiSelection ms, @temp t
	WHERE	ms.ReferenceID = t.OtherRef

	--Zde se pridaji Labels ve spravnem jazyce
	SELECT	t.PageID, t.PanelID, t.FieldID, l.Label, t.FieldType, t.StringValue, t.IntValue, t.DateValue, t.FileValue, t.MulitValue
			, t.OtherRef, t.MultiSelectItemID, t.PredecessorFieldID
	FROM	@temp t
	LEFT OUTER JOIN	Labels l ON t.FieldID= l.LabelID
	AND		(l.LanguageID = @LanguageID	OR l.LanguageID IS NULL)
	AND		(l.CompanyID = @RetailerID	OR l.CompanyID IS NULL)
END;

------------------------------------------------------------------



exec GetPage 1,1,1,'en'
------------------------------------------------------------------
ALTER PROCEDURE GetPage
	@RetailerID int,
	@ProjectID int,
    @PageID int,
	@LanguageID varchar(5)
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
		MultiValue  VARCHAR(100),
		OtherRef	int,
		MultiSelectItemID	int,
		PredecessorFieldID	int,
		ParentFieldID	int
	)
	INSERT INTO @temp
	SELECT	pf.PageID, pf.PanelID, pf.FieldID, 'label-tbd1' as 'Label', pf.FieldType
			,pf.StringValue, pf.IntValue, pf.DateValue, pf.FileValue, pf.MultiValue
			,pf.OtherRef ,null, pf.PredecessorFieldID, pf.ParentFieldID 
	FROM	ProjectFields pf
	WHERE	pf.ProjectID = @ProjectID
	AND		pf.RetailerID = @RetailerID
	AND		pf.PageID = @PageID

	INSERT INTO @temp
	SELECT	t.PageID, t.PanelID, ms.LabelID as 'FieldID', 'fieldlabel-tbd' as 'FieldLabel', (RTRIM(LTRIM(t.FieldType)) + '-item') as 'FieldType'
			,NULL as 'StringValue',NULL as 'IntValue',NULL as 'DateValue',NULL as 'FileValue',NULL as 'MultiValue'
			,t.FieldID as 'OtherRef', ms.ItemID as 'MultiSelectItemID', NULL as 'PredecessorFieldID', ms.ParentFieldID
	FROM	MultiSelection ms, @temp t
	WHERE	ms.ParentFieldID = t.FieldID


	SELECT	DISTINCT(t.PageID), l.Label
	FROM	@temp t
	LEFT OUTER JOIN	Pages p	ON t.PageID = p.PageID
	LEFT OUTER JOIN	Labels l ON p.LabelID = l.LabelID
	WHERE	l.LanguageID = @LanguageID

	SELECT	DISTINCT(t.PanelID), 'Desc' as 'FieldType', l.Label
	FROM	@temp t
	LEFT OUTER JOIN	Panels p ON t.PanelID = p.PanelID
	LEFT OUTER JOIN	Labels l ON p.DescriptionLabelID = l.LabelID
	WHERE	l.LanguageID = @LanguageID
	UNION
	SELECT	DISTINCT(t.PanelID), 'Title' as 'FieldType', l.Label
	FROM	@temp t
	LEFT OUTER JOIN	Panels p	ON t.PanelID = p.PanelID
	LEFT OUTER JOIN	Labels l	ON p.LabelID = l.LabelID
	WHERE	l.LanguageID = @LanguageID

	select t.PageID, t.PanelID, t.FieldID, l.Label as 'FieldLabel', t.FieldType, 
			t.StringValue, t.IntValue, t.DateValue, t.FileValue, t.MultiValue, 
			t.OtherRef, t.MultiSelectItemID, t.PredecessorFieldID, t.ParentFieldID
	FROM	@temp t
	LEFT OUTER JOIN	Panels p	ON t.PanelID = p.PanelID
	LEFT OUTER JOIN	Labels l	ON t.FieldID = l.LabelID
	WHERE	l.LanguageID = @LanguageID
	ORDER BY t.PredecessorFieldID
END;

--------------

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
	MultiValue  VARCHAR(100),
	OtherRef	int,
	MultiSelectItemID	int,
	PredecessorFieldID	int,
	ParentFieldID	int
)
INSERT INTO @temp
SELECT	pf.PageID, pf.PanelID, pf.FieldID, 'label-tbd1' as 'Label', pf.FieldType
		,pf.StringValue, pf.IntValue, pf.DateValue, pf.FileValue, pf.MultiValue
		,pf.OtherRef, null, pf.PredecessorFieldID, pf.ParentFieldID 
FROM	ProjectFields pf
WHERE	pf.ProjectID = 1
AND		pf.RetailerID = 1
AND		pf.PageID = 1



INSERT INTO @temp
SELECT	t.PageID, t.PanelID, ms.LabelID as 'FieldID', 'fieldlabel-tbd' as 'FieldLabel', (RTRIM(LTRIM(t.FieldType)) + '-item') as 'FieldType'
		,NULL as 'StringValue',NULL as 'IntValue',NULL as 'DateValue',NULL as 'FileValue',NULL as 'MultiValue'
		,t.FieldID as 'OtherRef', ms.ItemID as 'MultiSelectItemID', NULL as 'PredecessorFieldID', ms.ParentFieldID 
FROM	MultiSelection ms, @temp t
WHERE	ms.ParentFieldID = t.FieldID

SELECT	* from @temp

SELECT	DISTINCT(t.PageID), l.Label
FROM	@temp t
LEFT OUTER JOIN	Pages p	ON t.PageID = p.PageID
LEFT OUTER JOIN	Labels l ON p.LabelID = l.LabelID
WHERE	l.LanguageID = 'cs'

SELECT	DISTINCT(t.PanelID), 'Desc' as 'FieldType', l.Label
FROM	@temp t
LEFT OUTER JOIN	Panels p ON t.PanelID = p.PanelID
LEFT OUTER JOIN	Labels l ON p.DescriptionLabelID = l.LabelID
WHERE	l.LanguageID = 'cs'
UNION
SELECT	DISTINCT(t.PanelID), 'Title' as 'FieldType', l.Label
FROM	@temp t
LEFT OUTER JOIN	Panels p	ON t.PanelID = p.PanelID
LEFT OUTER JOIN	Labels l	ON p.LabelID = l.LabelID
WHERE	l.LanguageID = 'cs'

select t.PageID, t.PanelID, t.FieldID, l.Label, t.FieldType, 
		t.StringValue, t.IntValue, t.DateValue, t.FileValue, t.MultiValue, 
		t.OtherRef, t.MultiSelectItemID, t.PredecessorFieldID, t.ParentFieldID
FROM	@temp t
LEFT OUTER JOIN	Panels p	ON t.PanelID = p.PanelID
LEFT OUTER JOIN	Labels l	ON t.FieldID = l.LabelID
WHERE	l.LanguageID = 'cs'
ORDER BY t.PredecessorFieldID
---------------

SELECT * FROM PagePanels
SELECT * FROM Panels
SELECT * FROM Labels


------------------------------------------------------------------------------

SELECT * from Labels
exec AddLabel 16, 'cs', 'Nazdar'
exec AddLabel null, 'en', 'Hello'


ALTER PROCEDURE AddLabel
	@LabelID INT = NULL,
	@LanguageID varchar(5),
	@Label varchar(100)
AS
BEGIN
	IF EXISTS (	SELECT	1 
				FROM	Labels l
				WHERE	l.LabelID = @LabelID
				AND		l.LanguageID = @LanguageID
				)
	BEGIN
		SELECT	l.LabelID as 'ExistingIDs', l.Label as 'ExistingLabels', l.LanguageID as 'LanguageID'
		FROM	Labels l
		WHERE	l.LabelID = @LabelID
		return -1
	END

	DECLARE @NewLabelID INT
	IF(@LabelID is NULL)
	BEGIN
		SET @NewLabelID = (SELECT MAX(LabelID) from Labels) +1
	END
	ELSE
	BEGIN
		SET @NewLabelID = @LabelID
	END

	INSERT INTO Labels	VALUES	(@NewLabelID,@LanguageID,@Label,null)

	SELECT @NewLabelID as 'LabelID', @Label as 'Label'
	return 1
END;

------------------------------------------------------------------------------


exec AddFieldToPanel 15,1,'tx'
SELECT	*
FROM	PanelFields pf

ALTER PROCEDURE AddFieldToPanel
	@FieldID INT,
	@PanelID INT,
	@FieldType varchar(10)
AS
BEGIN
	IF NOT EXISTS (	SELECT	1 FROM	Labels l WHERE	l.LabelID = @FieldID)
	BEGIN
		SELECT	@FieldID as 'Field', 'Unknown FieldID. No translation.' as 'Error'
		return -1
	END
	IF NOT EXISTS (	SELECT	1 FROM	Panels p WHERE	p.PanelID = @PanelID)
	BEGIN
		SELECT	@PanelID as 'Panel', 'Unknown PanelID ' as 'Error'
		return -1
	END
	IF NOT EXISTS (	SELECT	1 WHERE	@FieldType in ('tx', 'rb', 'cb', 'ddlb') )
	BEGIN
		SELECT	@FieldType as 'FileType', 'Unknown FieldType ' as 'Error'
		return -1
	END

	DECLARE @LastField INT
	SET @LastField =(	SELECT	TOP 1 pf.FieldID
						FROM	PanelFields pf
						WHERE	pf.PanelID = @PanelID
						ORDER BY	pf.PredecessorFieldID DESC )

	--BEGIN TRAN 
		INSERT INTO PanelFields	(PanelID, FieldID, PredecessorFieldID, FieldType)
		VALUES		(@PanelID,@FieldID,@LastField,@FieldType)
		
		---Not finished
		SELECT	pf.PanelID, pf.FieldID, pf.PredecessorFieldID, pf.FieldType
		FROM	PanelFields pf
		WHERE	pf.PanelID = @PanelID
		ORDER BY	pf.PredecessorFieldID ASC

	--ROLLBACK TRAN


	return 1
END;
