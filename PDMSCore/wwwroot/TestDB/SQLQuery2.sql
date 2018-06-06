select * from [PanelsFields]
WHERE	1 = 1
AND		PanelID = 1
AND		RetailerID = 1
ORDER BY	[PredecessorFieldID]

insert	 into [PanelsFields]
VALUES (2,3,1)
----------
UPDATE	Fields
SET		LabelID = 1;
WHERE	Id = 1


SELECT	* FROM Fields
INSERT	INTO Fields VALUES ('rb',7)

ALTER TABLE [dbo].[Fields] DROP COLUMN LAbelID;
----------
select * from Labels
WHERE	1 = 1
AND		PanelID = 1
AND		RetailerID = 1
ORDER BY	[PredecessorFieldID]

insert	 into Labels
VALUES (3,'cs','Datum vytvoreni')
-----

SELECT	* FROM	PanelsFields pf
insert	 into PanelsFields	VALUES (1,3,2)



SELECT	* FROM	Fields f
SELECT	* FROM	Labels l
SELECT	* FROM	Panels p

DELETE from Fields
insert	 into Fields	VALUES ('tx',5)
insert	 into Fields	VALUES ('tx',6)
insert	 into Fields	VALUES ('rb',7)

UPDATE	Fields	SET		ID = 1
WHERE	ID = 4
DBCC CHECKIDENT('Fields', RESEED, 0)



SELECT	f.FieldID, f.FieldType, l.Label
FROM	PanelsFields pf, Fields f, Labels l, Panels p
WHERE	1 = 1
AND		pf.FieldID = f.FieldID
AND		f.FieldID = l.LabelID
AND		p.PanelID = pf.PanelID
AND		p.CompanyID = 1
AND		pf.PanelID = 1
AND		l.LanguageID = 'en'
ORDER BY	[PredecessorFieldID]


GetPanelFields 1, 1, 'en'

ALTER PROCEDURE GetPanelFields
    @PanelID int,
    @CompanyID int,
	@LanguageID varchar(5)
AS
BEGIN
	SELECT	f.FieldID, f.FieldType, l.Label
	FROM	PanelsFields pf, Fields f, Labels l, Panels p
	WHERE	1 = 1
	AND		pf.FieldID = f.FieldID
	AND		f.FieldID = l.LabelID
	AND		p.PanelID = pf.PanelID
	AND		p.CompanyID = @CompanyID
	AND		pf.PanelID = @PanelID
	AND		l.LanguageID = @LanguageID
	ORDER BY	[PredecessorFieldID]
END;


GetPanelFields 1, 1, 'en'
SELECT * FROM Projects

update Projects SET	FieldValue = '2. field' WHERE FieldID= 2

INSERT INTO Projects VALUES (1,1,3,'treti fields');


SELECT	*
--SELECT	f.FieldID, f.FieldType, l.Label, pj.StringValue, pj.*
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
