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
SELECT	* FROM	Pages pg
SELECT	* FROM	PagesPanels 

SELECT	* 
FROM	Pages pg left join Labels l on pg.LabelID = l.LabelID
WHERE	pg.PageID = 

SELECT	* FROM	PagesPanels 
INSERT	 into PagesPanels VALUES (1,1,1)


SELECT	* FROM	Labels l
insert	 into Labels	values (5,'en','Project deletion')


DELETE from Fields
insert	 into Fields	VALUES ('tx',5)
insert	 into Fields	VALUES ('tx',6)
insert	 into Fields	VALUES ('rb',7)


SELECT * FROM Pages 
UPDATE	Pages 
SET		LabelID = 5
WHERE	PageID = 2
--DBCC CHECKIDENT('Fields', RESEED, 0)


SELECT	*
FROM	Pages pg
LEFT outer join Labels lb ON pg.LabelID = lb.LabelID
WHERE	1=1
AND		pg.PageID = 1
AND		lb.LanguageID = 'en'
AND		
(
		lb.CompanyID = 1
OR		lb.CompanyID IS NULL)




SELECT	*
FROM	Pages pg
LEFT outer join Labels lb ON pg.LabelID = lb.LabelID
LEFT outer join PagesPanels pp ON pg.PageID = pp.PageID
LEFT outer join Panels pn ON pp.PanelID = pn.PanelID
WHERE	1=1
AND		pg.PageID = 1
AND		lb.LanguageID = 'en'
AND		pp.RetailerID = 1




GetPanelFields 1, 1, 'en'
update Projects SET	FieldValue = '2. field' WHERE FieldID= 2
INSERT INTO Projects VALUES (1,1,3,'treti fields');
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
	ORDER BY	[PredecessorFieldID]
END;



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

