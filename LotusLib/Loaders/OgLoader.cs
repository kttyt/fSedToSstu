using System;
using System.Collections.Generic;
using System.Linq;
using LotusLib.Auxiliary;
using LotusLib.Documents;
using LotusLib.Exceptions;

namespace LotusLib.Loaders
{
    public class OgLoader : Loader
    {
        public string OgReplicaId { get; set; }
        public string ResultReplicaId { get; set; }

        public OgLoader(string server, string pass)
            : base(server, pass) { }

        public IList<OgDocument> Get(string query)
        {
            if(!Lotus.IsInitialize)
                Lotus.Init(LotusPass);

            var docs = Lotus.SearchDocumentsByFormula(OgReplicaId, query);

            var ogDocuments = new List<OgDocument>();
            for (var count = 1; count <= docs.Count; count++)
            {
                var doc = docs.GetNthDocument(count);
                OgDocument ogDoc = null;
                try
                {
                    ogDoc = OgDocument.FromNotes(doc);
                }
                catch (NotImplementedException ex)
                {
                    RaseNewMessage($"ERROR: Неожиданное значение при формировании документа. '{ex}'"); 
                    continue;
                }

                try
                {
                    ogDoc.Attachments = Lotus.GetAttachments(doc, "ответ")
                        .Select(path => new Attachment(path))
                        .ToArray();
                }
                catch (NoAttachmentException)
                {
                    RaseNewMessage($"WARNING: {ogDoc.Number} - отсутствуют вложения");
                    //continue;
                }
                RaseNewMessage($"{ogDoc.Number} - получен");
                ogDocuments.Add(ogDoc);
            }

            foreach (var ogDoc in ogDocuments)
            {
                try
                {
                    ogDoc.LoadResultsFromSed(Lotus, ResultReplicaId);
                    RaseNewMessage($"Получен результат для {ogDoc.Number}");
                }
                catch (DocumentsNotFoundException)
                {
                    RaseNewMessage($"WARNING: {ogDoc.Number} - результат отсутствует");
                }
                catch (Exception)
                {
                    RaseNewMessage($"ERROR: {ogDoc.Number} - ошибка при загрузке результата");
                }
            }

            foreach (var ogDoc in ogDocuments)
            {
                if(!ogDoc.ValidateResults())
                    RaseNewMessage($"ERROR: {ogDoc.Number} - ошибка валидации результата");
            }
            return ogDocuments;
        }
    }
}
