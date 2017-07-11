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
