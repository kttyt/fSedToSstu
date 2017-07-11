using System.Collections.Generic;
using LotusLib.Auxiliary;

namespace LotusLib.Documents
{
    public abstract class Document
    {
        public string Id { get; }

        public Attachment[] Attachments { get; internal set; }

        internal Document(string id)
        {
            this.Id = id;
        }
    }
}
