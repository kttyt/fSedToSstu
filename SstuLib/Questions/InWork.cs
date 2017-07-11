using System;

namespace SstuLib.Questions
{
    public class InWork : Question
    {
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
