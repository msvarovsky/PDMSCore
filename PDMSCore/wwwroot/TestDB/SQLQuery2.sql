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



SELECT	* 
SELECT	
FROM	PanelsFields pf, Fields f, Labels l, Panels p
WHERE	1 = 1
AND		pf.FieldID = f.Id
AND		f.Id = l.ID
AND		p.Id = pf.PanelID
AND		p.CompanyID = 1
AND		pf.PanelID = 1
AND		p.CompanyID = 1
AND		l.LanguageID = 'en'
ORDER BY	[PredecessorFieldID]

