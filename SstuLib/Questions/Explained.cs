using System;
using System.Text;
using SstuLib.Auxiliary;

namespace SstuLib.Questions
{
    public class Explained : InWork
    {
        public DateTime ResponseDate { get; set; }
        public Attachment Attachment { get; set; }

        public override QuestionStatus Status => QuestionStatus.Explained;

        public Explained(Request req) : base(req)
        {
        }

        protected override bool IntValidate()
        {
            return base.IntValidate()
                   && ResponseDate >= RegistrationDate
                   && Attachment != null
                   && !string.IsNullOrEmpty(Attachment.Name)
                   && !string.IsNullOrEmpty(Attachment.Content)
                   && Encoding.ASCII.GetByteCount(Attachment.Content) < 1e+7;
        }
    }
}
