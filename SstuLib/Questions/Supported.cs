using Newtonsoft.Json;

namespace SstuLib.Questions
{
    public class Supported : Explained
    {
        [JsonProperty(PropertyName = "actionsTaken")]
        public bool ActionsTaken { get; set; }
        public override QuestionStatus Status => QuestionStatus.Supported;

        public Supported(Request req) : base(req)
        {
        }
    }
}