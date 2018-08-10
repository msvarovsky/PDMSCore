
exec GetPageInfo 1, 1, 'en'
----------------------------
ALTER PROCEDURE GetPageInfo
	@RetailerID int,
    @PageID int,
	@LanguageID varchar(5)
AS
BEGIN
	SELECT	p.PageID, l.Label as 'PageLabel', p.[URL] as 'URL', p.NavID
	FROM	RetailerPages rp
	LEFT OUTER JOIN Pages p		ON p.PageID = rp.PageID
	LEFT OUTER JOIN Labels l	ON l.LabelID = p.LabelID
	WHERE	1 = 1
	AND		rp.RetailerID	= @RetailerID
	AND		rp.PageID		= @PageID
	AND		l.LanguageID	= @LanguageID
	AND		(l.CompanyID = @RetailerID OR l.CompanyID is NULL)
END;

--------------	Na hrani	-----------------------

SELECT	p.PageID, l.Label as 'PageLabel', p.[URL] as 'URL'
FROM	RetailerPages rp
LEFT OUTER JOIN Pages p		ON p.PageID = rp.PageID
LEFT OUTER JOIN Labels l	ON l.LabelID = p.LabelID
WHERE	1=1
AND		rp.RetailerID	= 1
AND		rp.PageID		= 1
AND		l.LanguageID	= 'en'
AND		(l.CompanyID = 1 OR l.CompanyID is NULL)

---------------------------------------------------------------------------------------------------------------

exec GetPanelsInfo 1,1,'en'
----------------------------
ALTER PROCEDURE GetPanelsInfo
	@RetailerID int,
    @PageID int,
	@LanguageID varchar(5)
AS
BEGIN
	WITH cte as
	(
		SELECT	pan.PanelID, l.Label as 'PanelLabel', pan.DescriptionLabelID	
		FROM	RetailerPages rp
		LEFT OUTER JOIN PagePanels pp	ON	rp.PageID = pp.PageID
		LEFT OUTER JOIN Panels pan		ON	pan.PanelID = pp.PanelID
		LEFT OUTER JOIN Labels l		ON	l.LabelID = pan.LabelID 
		WHERE	1=1
		AND		rp.RetailerID	= @RetailerID
		AND		rp.PageID		= @PageID
		AND		l.LanguageID	= @LanguageID
		AND		(l.CompanyID = @RetailerID OR l.CompanyID is NULL)
	)
	SELECT	cte.PanelID, cte.PanelLabel, l.Label as 'DescriptionLabel'
	FROM	cte
	LEFT OUTER JOIN Labels l	ON cte.DescriptionLabelID = l.LabelID
	WHERE	l.LanguageID	= @LanguageID
	AND		(l.CompanyID	= @RetailerID OR l.CompanyID is NULL)
END;

-----------------------		Na hrani	-----------------------

WITH cte as
(
	SELECT	pan.PanelID, l.Label as 'PanelLabel', pan.DescriptionLabelID	
	FROM	RetailerPages rp
	LEFT OUTER JOIN PagePanels pp	ON	rp.PageID = pp.PageID
	LEFT OUTER JOIN Panels pan		ON	pan.PanelID = pp.PanelID
	LEFT OUTER JOIN Labels l		ON	l.LabelID = pan.LabelID 
	WHERE	1=1
	AND		rp.RetailerID	= 1
	AND		rp.PageID		= 1
	AND		l.LanguageID	= 'en'
	AND		(l.CompanyID = 1 OR l.CompanyID is NULL)
)
SELECT	cte.PanelID, cte.PanelLabel, l.Label as 'DescriptionLabel'
FROM	cte
LEFT OUTER JOIN Labels l	ON cte.DescriptionLabelID = l.LabelID
WHERE	l.LanguageID	= 'en'
AND		(l.CompanyID	= 1 OR l.CompanyID is NULL)

---------------------------------------------------------------------------------------------------------------



exec GetPageFields 1,1,1,'en'
-----------------------------
ALTER PROCEDURE GetPageFields
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
		PredecessorFieldID	int,
		FieldType	nvarchar(100),
		ParentFieldID	int,
		StringValue nvarchar(100),
		FileValues VARBINARY (MAX),
		OtherRef	int,
		SelectedItemsIDs int
	)

	INSERT INTO @temp
	-- Ty zakladni (ne z multiselection) fields
	SELECT	pc.PanelID,
			pc.FieldID, NULL as 'FieldLabel', pc.PredecessorFieldID, pc.FieldType, NULL as 'ParentFieldID',
			NULL as 'StringValue', NULL as 'FileValue', NULL as 'OtherRef', NULL as 'SelectedItemsIDs'
	FROM	PanelsContent pc
	WHERE	1=1	--	Vyfiltruju pouze radky tykajici se meho RetailerID a PageID. 
	AND		pc.RetailerID	= @RetailerID
	AND		pc.PageID		= @PageID
		UNION
	-- Fields z multiselection
	SELECT	pc.PanelID,
			ms.ItemID, NULL as 'FieldLabel', ms.PredecessorItemID, f.FieldType as 'FieldType', ms.ParentFieldID 'ParentFieldID',
			NULL as 'StringValue', NULL as 'FileValue', NULL as 'OtherRef', NULL as 'SelectedItemsIDs'
	FROM	PanelsContent pc, MultiSelection ms, Fields f
	WHERE	ms.ParentFieldID = pc.FieldID
	AND		ms.ItemID = f.FieldID
	AND		1=1	--	Vyfiltruju pouze radky tykajici se meho RetailerID a PageID. 
	AND		pc.RetailerID	= @RetailerID
	AND		pc.PageID		= @PageID

	--		Dodam predklad fields
	UPDATE  t
	SET		t.FieldLabel = l.Label
	FROM	@temp t 
	LEFT OUTER JOIN Fields f ON t.FieldID = f.FieldID
	LEFT OUTER JOIN Labels l ON f.LabelID = l.LabelID
	WHERE	l.LanguageID = @LanguageID
	AND		(l.CompanyID = @RetailerID OR l.CompanyID is NULL)


	--		Dodam hodnoty vsech (non-Multiselection) fields z FieldsValues
	SELECT	t.PanelID,
			t.FieldID, t.FieldLabel, t.PredecessorFieldID, t.FieldType, t.ParentFieldID,
			fv.StringValue, fv.FileValue, fv.OtherRef, fv.MultiValue as 'SelectedItemsIDs'
	FROM	@temp t
	LEFT OUTER JOIN FieldsValues fv ON t.FieldID = fv.FieldID
	WHERE	fv.RetailerID is NULL
			OR
			(	--	RetailerID musim vyzadovat pouze v pripade norm fields. 
				--	Ne tech z multiselection. A to se pozna podle ParentFieldID.
				--	Update: Vlastne tomu nerozumim, ale funguje to.
					fv.RetailerID = @RetailerID 
				AND t.ParentFieldID is NULL
			)
	AND		fv.ProjectID = @ProjectID
END;

------------	Na Hrani	-----------------

DECLARE @temp TABLE
(
	PanelID		int,
	FieldID		int,
	FieldLabel	nvarchar(100),
	PredecessorFieldID	int,
	FieldType	nvarchar(100),
	ParentFieldID	int,
	StringValue nvarchar(100),
	FileValues VARBINARY (MAX),
	OtherRef	int,
	SelectedItemsIDs int
)

INSERT INTO @temp
-- Ty zakladni (ne z multiselection) fields
SELECT	pc.PanelID,
		pc.FieldID, NULL as 'FieldLabel', pc.PredecessorFieldID, pc.FieldType, NULL as 'ParentFieldID',
		NULL as 'StringValue', NULL as 'FileValue', NULL as 'OtherRef', NULL as 'SelectedItemsIDs'
FROM	PanelsContent pc
WHERE	1=1	--	Vyfiltruju pouze radky tykajici se meho RetailerID a PageID. 
AND		pc.RetailerID	= 1
AND		pc.PageID		= 1
	UNION
-- Fields z multiselection
SELECT	pc.PanelID,
		ms.ItemID, NULL as 'FieldLabel', ms.PredecessorItemID, f.FieldType as 'FieldType', ms.ParentFieldID 'ParentFieldID',
		NULL as 'StringValue', NULL as 'FileValue', NULL as 'OtherRef', NULL as 'SelectedItemsIDs'
FROM	PanelsContent pc, MultiSelection ms, Fields f
WHERE	ms.ParentFieldID = pc.FieldID
AND		ms.ItemID = f.FieldID
AND		1=1	--	Vyfiltruju pouze radky tykajici se meho RetailerID a PageID. 
AND		pc.RetailerID	= 1
AND		pc.PageID		= 1

--		Dodam predklad fields
UPDATE  t
SET		t.FieldLabel = l.Label
FROM	@temp t 
LEFT OUTER JOIN Fields f ON t.FieldID = f.FieldID
LEFT OUTER JOIN Labels l ON f.LabelID = l.LabelID
WHERE	l.LanguageID = 'en'
AND		(l.CompanyID = 1 OR l.CompanyID is NULL)

--		Dodam hodnoty vsech (non-Multiselection) fields z FieldsValues
SELECT	t.PanelID,
		t.FieldID, t.FieldLabel, t.PredecessorFieldID, t.FieldType, t.ParentFieldID,
		fv.StringValue, fv.FileValue, fv.OtherRef, fv.MultiValue as 'SelectedItemsIDs'
FROM	@temp t
LEFT OUTER JOIN FieldsValues fv ON t.FieldID = fv.FieldID
WHERE	fv.RetailerID is NULL
		OR
		(	--	RetailerID musim vyzadovat pouze v pripade norm fields. 
			--	Ne tech z multiselection. A to se pozna podle ParentFieldID.
			--	Update: Vlastne tomu nerozumim, ale funguje to.
				fv.RetailerID = 1 
			AND t.ParentFieldID is NULL
		)
AND		fv.ProjectID = 1



---------------------------------------------------------------------------------------------------------------

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

CREATE PROCEDURE AddFieldToPanel
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
----------------------------------------------------------------------

SELECT	*	FROM	FieldsValues fv
begin tran
	exec AddOrUpdateMultiValue 1,1,19,6,1
	SELECT	*	FROM	FieldsValues fv
	exec AddOrUpdateMultiValue 1,1,19,7,1
	SELECT	*	FROM	FieldsValues fv
rollback tran

--------------


ALTER PROCEDURE AddOrUpdateMultiValue
	@RetailerID int,
    @ProjectID int,
	@FieldID int,
	@SelectedFieldID int,
	@UserID int
AS
BEGIN
	DECLARE @SelectedFieldIDAsString nvarchar(10)
	SET		@SelectedFieldIDAsString = '(' + CONVERT(varchar(10), @SelectedFieldID) + ')'
	
	IF EXISTS --  ...Row with FieldID in FieldsValues
	(
		SELECT	1
		FROM	FieldsValues fv
		WHERE	fv.RetailerID = @RetailerID
		AND		fv.ProjectID = @ProjectID
		AND		fv.FieldID = @FieldID
	)
	BEGIN
		DECLARE	@typ nvarchar(10)
		SELECT	@typ = LTRIM(RTRIM(f.FieldType))
		FROM	Fields f
		WHERE	f.FieldID = @FieldID

		IF @typ = 'ddlb' OR @typ = 'rb'	--	dropdown lixtbox, radio buttons
		BEGIN
			UPDATE	FieldsValues
			SET		MultiValue	= @SelectedFieldIDAsString
			WHERE	RetailerID	= @RetailerID
			AND		ProjectID	= @ProjectID
			AND		FieldID		= @FieldID
		END
		IF @typ = 'cb'	--	checkbox
		BEGIN
			IF NOT EXISTS --  
			(
				SELECT	1
				FROM	FieldsValues fv
				WHERE	fv.RetailerID = @RetailerID
				AND		fv.ProjectID = @ProjectID
				AND		fv.FieldID = @FieldID
				AND		fv.MultiValue like '%' + @SelectedFieldIDAsString + '%'
			)
			BEGIN		--	Append the ID there
				UPDATE	FieldsValues
				SET		MultiValue = ISNULL (CONCAT(MultiValue,@SelectedFieldIDAsString),@SelectedFieldIDAsString)
				WHERE	RetailerID = @RetailerID
				AND		ProjectID = @ProjectID
				AND		FieldID = @FieldID
			END
		END
	END
	else
	BEGIN
		INSERT INTO FieldsValues (RetailerID, ProjectID, FieldID, MultiValue)
		VALUES				(@RetailerID, @ProjectID, @FieldID, @SelectedFieldIDAsString)
	END
END;

-------------------------------------	RemoveMultiValue	-------------------------------------

SELECT	*	FROM	FieldsValues fv
begin tran
	exec RemoveMultiValue 1,1,5,16,1		SELECT	*	FROM	FieldsValues fv
	exec AddOrUpdateMultiValue 1,1,5,16,1	SELECT	*	FROM	FieldsValues fv
	exec AddOrUpdateMultiValue 1,1,5,17,1	SELECT	*	FROM	FieldsValues fv
	exec RemoveMultiValue 1,1,5,17,1	SELECT	*	FROM	FieldsValues fv
	exec RemoveMultiValue 1,1,5,17,1	SELECT	*	FROM	FieldsValues fv
	exec RemoveMultiValue 1,1,5,16,1	SELECT	*	FROM	FieldsValues fv
rollback tran

------------------------

ALTER PROCEDURE RemoveMultiValue
	@RetailerID int,
    @ProjectID int,
	@FieldID int,
	@SelectedFieldID int,
	@UserID int
AS
BEGIN
	DECLARE @SelectedFieldIDAsString nvarchar(10)
	SET		@SelectedFieldIDAsString = '(' + CONVERT(varchar(10), @SelectedFieldID) + ')'
	
	IF EXISTS --  ...Row with FieldID in FieldsValues
	(
		SELECT	1
		FROM	FieldsValues fv
		WHERE	fv.RetailerID = @RetailerID
		AND		fv.ProjectID = @ProjectID
		AND		fv.FieldID = @FieldID
	)
	BEGIN
		DECLARE	@typ nvarchar(10)
		SELECT	@typ = LTRIM(RTRIM(f.FieldType))
		FROM	Fields f
		WHERE	f.FieldID = @FieldID

		IF @typ = 'ddlb' OR @typ = 'rb'	OR @typ = 'cb'	--	dropdown lixtbox, radio buttons and checkbox
		BEGIN			--	For RB and DDLB, I could as well only update the value without verifying if it exists before.
			IF EXISTS --  
			(
				SELECT	1
				FROM	FieldsValues fv
				WHERE	fv.RetailerID = @RetailerID
				AND		fv.ProjectID = @ProjectID
				AND		fv.FieldID = @FieldID
				AND		fv.MultiValue like '%' + @SelectedFieldIDAsString + '%'
			)
			BEGIN		--	Remove ID from there
				UPDATE	FieldsValues
				SET		MultiValue = ISNULL (REPLACE(MultiValue,@SelectedFieldIDAsString,''),NULL)
				WHERE	RetailerID = @RetailerID
				AND		ProjectID = @ProjectID
				AND		FieldID = @FieldID
			END
		END
	END
	--else  -- For the moment, if the row does not exist, let's not do anything.
	--BEGIN
		
	--END
END;

---------------------------------------------------------------------------------------------------------------


ALTER PROCEDURE AddOrUpdateStringValue
	@RetailerID int,
    @ProjectID int,
	@FieldID int,
	@NewValue nvarchar(MAX),
	@UserID int
AS
BEGIN
	IF EXISTS --  ...Row with FieldID in FieldsValues
	(
		SELECT	1
		FROM	FieldsValues fv
		WHERE	fv.RetailerID = @RetailerID
		AND		fv.ProjectID = @ProjectID
		AND		fv.FieldID = @FieldID
	)
	BEGIN
		UPDATE	FieldsValues
		SET		StringValue = @NewValue
		WHERE	RetailerID = @RetailerID
		AND		ProjectID = @ProjectID
		AND		FieldID = @FieldID
	END
	else
	BEGIN
		INSERT INTO FieldsValues (RetailerID, ProjectID, FieldID, StringValue)
		VALUES				(@RetailerID, @ProjectID, @FieldID, @NewValue)
	END
END;


--------------------------------------------------------------------------------

exec GetNavigation 2, 'en', 1
SELECT	*	FROM	Navigation nav
-----------------------------

ALTER PROCEDURE GetNavigation
	@NavID int,
	@LanguageID varchar(5),
	@UserID int
AS
BEGIN
	SELECT	nav.NavID, l.Label, nav.[URL], nav.ParentNavID, nav.Icon
	FROM	Navigation nav
	LEFT OUTER JOIN Labels l ON nav.LabelID = l.LabelID
	WHERE	l.LanguageID = @LanguageID
	AND		
	(
		nav.NavID = @NavID OR nav.SuperParentNavID = @NavID
	)
END;
