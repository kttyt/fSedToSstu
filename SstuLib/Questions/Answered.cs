using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SstuLib.Questions
{
    public class Answered : Explained
    {
        public override QuestionStatus Status => QuestionStatus.Answered;

        public Answered(Request req) : base(req)
        {
        }
    }
}
