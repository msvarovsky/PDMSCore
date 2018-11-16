using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.Models
{
    public class NewProject
    {

        //SELECT a.*, l.Label
        //FROM
        //(
        //    SELECT pc.FieldID, pc.FieldType, pc.LabelID, pc.PredecessorFieldID, NULL as [ParentFieldID]
        //    FROM    PanelsContent pc
        //    WHERE   pc.RetailerID = 2

        //    AND pc.NavID = 21

        //    UNION
        //    SELECT  ms.ItemID as [FieldID], f.FieldType, f.LabelID, ms.PredecessorItemID, ms.ParentFieldID
        //    FROM    MultiSelection ms

        //    LEFT OUTER JOIN Fields f ON ms.ItemID = f.FieldID

        //    WHERE ms.ParentFieldID in
        //            (
        //                SELECT pc.FieldID
        //                FROM    PanelsContent pc

        //                WHERE pc.RetailerID = 2

        //                AND pc.NavID = 21
        //            )
        //) as a
        //LEFT OUTER JOIN Labels l ON a.LabelID = l.LabelID
        //WHERE   l.LanguageID = 'en'


    }
}
