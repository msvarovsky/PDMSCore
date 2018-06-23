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

ALTER VIEW ProjectFields AS
SELECT	rp.RetailerID, pp.PageID, pf.PanelID, 
		fv.FieldID, fv.ProjectID, fv.StringValue, fv.IntValue, fv.DateValue, fv.FileValue, fv.OtherRef, fv.Effdt, fv.[Version]
		,pp.PredecessorPanelID, pf.PredecessorFieldID
FROM	RetailerPages rp
LEFT OUTER JOIN PagePanels pp ON rp.PageID = pp.PageID
LEFT OUTER JOIN PanelFields pf ON pp.PanelID = pf.PanelID
LEFT OUTER JOIN FieldsValues fv ON pf.FieldID = fv.FieldID 
								AND fv.RetailerID = rp.RetailerID



Select	*
FROM	ProjectFields pf
WHERE	pf.RetailerID	=	1
AND		pf.PageID		=	1
ORDER BY	pf.PredecessorPanelID, pf.PredecessorFieldID

