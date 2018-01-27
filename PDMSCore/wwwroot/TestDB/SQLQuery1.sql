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

