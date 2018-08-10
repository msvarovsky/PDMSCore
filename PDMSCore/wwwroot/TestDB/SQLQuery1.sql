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
	PageID		int,
	PageLabel	nvarchar(100),
	PanelID		int,
	PanelLabel	nvarchar(100),
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
	PredecessorFieldID	int
)
INSERT INTO @temp
SELECT	pf.PageID, '', pf.PanelID, '', pf.FieldID, 'label-tbd1' as 'Label', pf.FieldType
		,pf.StringValue, pf.IntValue, pf.DateValue, pf.FileValue, pf.MultiValue
		,pf.OtherRef ,null, null
FROM	ProjectFields pf
WHERE	pf.ProjectID = 1
AND		pf.RetailerID = 1
AND		pf.PageID = 1

INSERT INTO @temp
SELECT	t.PageID, '', t.PanelID, '', ms.LabelID as 'FieldID', 'fieldlabel-tbd' as 'FieldLabel', (RTRIM(LTRIM(t.FieldType)) + '-item') as 'FieldType'
		,NULL as 'StringValue',NULL as 'IntValue',NULL as 'DateValue',NULL as 'FileValue',NULL as 'MultiValue'
		,t.FieldID as 'OtherRef', ms.ItemID as 'MultiSelectItemID', NULL as 'PredecessorFieldID'
FROM	MultiSelection ms, @temp t
WHERE	ms.ReferenceID = t.OtherRef



SELECT	t.PageID, '', t.PanelID, '',  t.FieldID, l.Label, t.FieldType, t.StringValue, t.IntValue, t.DateValue, t.FileValue, t.MultiValue
		, t.OtherRef, t.MultiSelectItemID, t.PredecessorFieldID
FROM	@temp t
LEFT OUTER JOIN	Labels l ON t.FieldID= l.LabelID
AND		(l.LanguageID = 'cs'	OR l.LanguageID IS NULL)
AND		(l.CompanyID = 1		OR l.CompanyID IS NULL)
-----------


SELECT	DISTINCT PanelID
FROM	@temp t
LEFT OUTER JOIN	




SELECT * 
FROM	Pages pg
LEFT OUTER JOIN	Labels l ON pg.LabelID = l.LabelID
WHERE	l.LanguageID = 'cs'

SELECT * from PagesPanels
SELECT * from PanelFields
SELECT * from Projects
SELECT * from Fields
SELECT * from Labels



SELECT * from Panels
INSERT INTO Panels (LabelID, DescriptionLabelID) VALUES (9,10)

SELECT * from Labels
INSERT INTO Labels VALUES (10,'en', 'Description of the 1st panel',NULL)
INSERT INTO Labels VALUES (9,'cs', '1. panel',NULL)


SELECT * from Panels p
LEFT OUTER JOIN Labels l ON p.LabelID = l.LabelID

SELECT * from Panels p
LEFT OUTER JOIN Labels l ON p.DescriptionLabelID = l.LabelID


SELECT	DISTINCT(t.PageID), l.Label
FROM	@temp t
LEFT OUTER JOIN	Pages p	ON t.PageID = p.PageID
LEFT OUTER JOIN	Labels l ON p.LabelID = l.LabelID
WHERE	l.LanguageID = 'cs'


SELECT	p.PanelID, 'title', l.Label
FROM	Panels p	LEFT OUTER JOIN	Labels l ON p.LabelID = l.LabelID
WHERE	l.LanguageID = 'cs'
UNION
SELECT	p.PanelID, 'desc', l.Label
FROM	Panels p	LEFT OUTER JOIN	Labels l ON p.DescriptionLabelID = l.LabelID
WHERE	l.LanguageID = 'cs'


SELECT	pp.PanelID 
FROM	PagePanels pp
LEFT OUTER JOIN	Labels ON	pp.PanelID




SELECT	* 
FROM	ProjectFields


SELECT	* 
FROM	MultiSelection


SELECT	* 
FROM	PanelFields rp  



sp_helptext ProjectFields

CREATE VIEW ProjectFields AS    
  
SELECT rp.RetailerID, pp.PageID, pf.PanelID,     
  fv.FieldID, pf.FieldType, fv.ProjectID, fv.StringValue, fv.IntValue, fv.DateValue, fv.FileValue, fv.MultiValue,  
  fv.OtherRef, fv.ParentFieldID, fv.Effdt, fv.[Version]    
  ,pp.PredecessorPanelID, pf.PredecessorFieldID    
FROM RetailerPages rp    
LEFT OUTER JOIN PagePanels pp ON rp.PageID = pp.PageID    
LEFT OUTER JOIN PanelFields pf ON pp.PanelID = pf.PanelID    
LEFT OUTER JOIN FieldsValues fv ON pf.FieldID = fv.FieldID     
        AND fv.RetailerID = rp.RetailerID



SELECT	*	FROM	FieldsValues fv

IF EXISTS (	SELECT	1 
			FROM	FieldsValues fv
			WHERE	fv.RetailerID = 1
			AND		fv.ProjectID = 1
			AND		fv.FieldID = 19
			)
BEGIN


SELECT	*	FROM	Fields


SELECT	*	FROM	FieldsValues fv
	UPDATE	FieldsValues
	SET		MultiValue = 6
	WHERE	RetailerID = 1
	AND		ProjectID = 1
	AND		FieldID = 19
	SELECT	*	FROM	FieldsValues fv

	
END


SELECT	*	FROM	FieldsValues fv
begin tran
	exec AddOrUpdateMultiValue 1,1,19,6,1
	SELECT	*	FROM	FieldsValues fv
	exec AddOrUpdateMultiValue 1,1,19,7,1
	SELECT	*	FROM	FieldsValues fv
rollback tran



-----------
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


-----------------------------------------------------------------

SELECT	*	FROM	Fields	WHERE FieldID = 5

SELECT	*	FROM	FieldsValues fv
begin tran
	exec AddOrUpdateMultiValue 1,1,5,17,1
	SELECT	*	FROM	FieldsValues fv
rollback tran


SELECT	*	FROM	FieldsValues fv
WHERE	1=1
AND		fv.MultiValue like '%(17)%'
AND		fv.FieldID = 5



SELECT	*	FROM	Fields

SELECT	*	FROM	FieldsValues fv
begin tran
	exec RemoveMultiValue 1,1,5,16,1
	SELECT	*	FROM	FieldsValues fv

	exec AddOrUpdateMultiValue 1,1,5,16,1
	SELECT	*	FROM	FieldsValues fv

	exec AddOrUpdateMultiValue 1,1,5,17,1
	SELECT	*	FROM	FieldsValues fv

	exec RemoveMultiValue 1,1,5,17,1
	SELECT	*	FROM	FieldsValues fv

	exec RemoveMultiValue 1,1,5,17,1
	SELECT	*	FROM	FieldsValues fv

	exec RemoveMultiValue 1,1,5,16,1
	SELECT	*	FROM	FieldsValues fv
rollback tran



exec RemoveMultiValue 1,1,3,1,1
	SELECT	*	FROM	FieldsValues fv
	exec RemoveMultiValue 1,1,3,1,1
	SELECT	*	FROM	FieldsValues fv



-------------

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

-------------



begin tran
	SELECT	*	FROM	FieldsValues fv
exec AddOrUpdateMultiValue 1, 1, 5, 16, 1;	
	SELECT	*	FROM	FieldsValues fv
rollback tran

--delete from Navigation 
SELECT	*	FROM	Navigation nav

INSERT INTO 

begin tran
	INSERT INTO Navigation (LabelID, [URL], PredecessorNavID, Icon, TechnicalName)
	VALUES				(20, 'www.hovno.cz', NULL, convert(varbinary, 'obrazek hledej'), 'Search project')
	SELECT	SCOPE_IDENTITY()

rollback tran

UPDATE	Navigation
SET		PredecessorNavID = 1
WHERE	NavID = 2

-----------------------------------------------------

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

