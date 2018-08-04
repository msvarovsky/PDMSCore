
--------	Vytvoreni noveho prekladu pro novy field	----------
--				LabelID(null=new label),	LanguageID, Label
exec AddLabel	15,							'en',		'Tralala'
SELECT * from Labels

-------------	Vlozeni noveho field do panelu  ----------------
--					FieldID,	PanelID,	FileType????? (to je divne)
exec AddFieldToPanel 15,1,'tx'
SELECT	*	FROM	PanelFields pf



SELECT	*	FROM	ProjectFields
SELECT	*	FROM	MultiSelection








SELECT * FROM ProjectFields

