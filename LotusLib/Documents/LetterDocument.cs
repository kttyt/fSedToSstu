using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domino;
using LotusLib.Auxiliary;

namespace LotusLib.Documents
{
    public class LetterDocument : Document
    {
        public string OutNum { get; internal set; }
        public DateTime OutDate { get; internal set; }
        public string Recipient { get; internal set; }
        public string RecipientId { get; internal set; }

        public LetterDocument(string id) : base(id)
        {
        }

        internal static LetterDocument FromNotes(NotesDocument document)
        {
            return new LetterDocument(document.GetFieldValue("id"))
            {
                OutNum = document.GetFieldValue("Out_RegistrationNumber"),
                Recipient = document.GetFieldValue("Recipient"),
                RecipientId = document.GetFieldValue("recipient_id"),
                OutDate = DateTime.Parse(document.GetFieldValue("Out_RegistrationDate")),
                GroudId = document.GetFieldValue("group_id")
            };
        }
    }
}
