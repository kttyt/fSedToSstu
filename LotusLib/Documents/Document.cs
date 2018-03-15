using LotusLib.Auxiliary;

namespace LotusLib.Documents
{
    public abstract class Document
    {
        public string Id { get; }
        public string GroudId { get; protected set; }

        public Document[] Links { get; internal set; }
        public Attachment[] Attachments { get; internal set; }

        protected Document(string id)
        {
            this.Id = id;
        }
    }
}
