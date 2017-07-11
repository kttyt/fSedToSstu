using System;
using System.Text;
using Newtonsoft.Json;
using SstuLib.Auxiliary;

namespace SstuLib.Questions
{
    public class Explained : InWork
    {
        [JsonProperty(PropertyName = "responseDate")]
        public DateTime ResponseDate { get; set; }

        [JsonProperty(PropertyName = "attachment")]
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
