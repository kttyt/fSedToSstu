using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SstuLib
{
    public abstract class Question
    {
        public string Code { get; set; }
        public DateTime IncomingDate { get; set; }

        protected Request ParentReq { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public abstract QuestionStatus Status { get; }

        protected Question(Request req)
        {
            this.ParentReq = req;
        }

        public bool Validate()
        {
            return (ParentReq.IsDirect || this.IncomingDate >= ParentReq.CreateDate)
                && !string.IsNullOrEmpty(Code)
                && IntValidate();
        }

        protected abstract bool IntValidate();
    }
}
