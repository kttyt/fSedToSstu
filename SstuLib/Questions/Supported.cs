namespace SstuLib.Questions
{
    public class Supported : Explained
    {
        public bool ActionsTaken { get; set; }
        public override QuestionStatus Status => QuestionStatus.Supported;

        public Supported(Request req) : base(req)
        {
        }
    }
}