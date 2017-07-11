using System;
using Newtonsoft.Json;

namespace SstuLib.Questions
{
    public class InWork : Question
    {
        [JsonProperty(PropertyName = "registrationDate")]
        public DateTime RegistrationDate { get; set; }

        public InWork(Request req) : base(req)
        {
        }

        public override QuestionStatus Status => QuestionStatus.InWork;

        protected override bool IntValidate()
        {
            return RegistrationDate >= base.IncomingDate;
        }
    }
}
