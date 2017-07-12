using System.Linq;
using Domino;
using LotusLib.Documents;

namespace LotusLib.Auxiliary
{
    internal static class Utils
    {
        internal static string GetFieldValue(this NotesDocument doc, string name)
        {
            foreach (NotesItem item in (object[])doc.Items)
            {
                if (item.Name.ToLower().Equals(name.ToLower()))
                {
                    object[] values = (object[])item.Values;
                    //return values[0].ToString();
                    return values.First(value => value != null)
                        .ToString()
                        .Trim();
                }
            }
            //throw new NoFieldFoundException();
            return null;
        }

        internal static bool LoadResultsFromSed(this OgDocument doc, Lotus lotus, string replicaId)
        {
            var results = lotus.SearchDocumentsByFormula(replicaId,
                $"[document_groupID] contains \"{doc.GroudId}\"");
            if (results.Count > 0)
            {
                var ogResult = OgResult.FromNotes(results.GetFirstDocument());
                doc.Results = new[] { ogResult };
                return true;
            }
            return false;
        }
    }
}