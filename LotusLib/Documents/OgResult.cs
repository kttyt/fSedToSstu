using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domino;
using LotusLib.Auxiliary;

namespace LotusLib.Documents
{
    public class OgResult : Document
    {
        public DateTime CloseDate       { get; private set; }
        public string ReferenceNumber   { get; private set; }
        public string DocumentGroupId   { get; private set; }

        internal OgResult(string id) : base(id)
        {
        }

        internal static OgResult FromNotes(NotesDocument document)
        {
            return new OgResult(document.GetFieldValue("id"))
            {
                CloseDate = DateTime.Parse(document.GetFieldValue("date_fact")),
                ReferenceNumber = document.GetFieldValue("reference_number"),
                DocumentGroupId = document.GetFieldValue("document_groupID")
            };
        }
    }
}