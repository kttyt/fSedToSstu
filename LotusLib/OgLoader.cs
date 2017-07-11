using System;
using System.Collections.Generic;
using System.Linq;
using LotusLib.Auxiliary;
using LotusLib.Documents;
using LotusLib.Exceptions;

namespace LotusLib
{
    public class OgLoader
    {
        public string LotusPass { get; set; }
        public string OgReplicaId { get; set; }
        public string ResultReplicaId { get; set; }
        public string Server { get; set; }

        private readonly Lotus _lotus;

        public delegate void MyEventHandler(object obj, string foo);

        public event MyEventHandler OnError;

        public OgLoader(string server, string pass)
        {
            Server = server;
            LotusPass = pass;
            _lotus = new Lotus(Server);
        }

        private void RaseNewMessage(string msg)
        {
            OnError?.Invoke(this, msg);
        }

        public IList<OgDocument> Get(string query)
        {
            if(!_lotus.IsInitialize)
                _lotus.Init(LotusPass);

            var docs = _lotus.SearchDocumentsByFormula(OgReplicaId, query);

            var ogDocuments = new List<OgDocument>();
            for (var count = 1; count <= docs.Count; count++)
            {
                var doc = docs.GetNthDocument(count);
                OgDocument ogDoc = null;
                try
                {
                    ogDoc = OgDocument.FromNotes(doc);
                }
                catch (NotImplementedException)
                {
                    RaseNewMessage("ERROR: Неожиданное значение при формировании документа. Переход к следующему."); 
                    continue;
                }

                try
                {
                    ogDoc.Attachments = _lotus.GetAttachments(doc, "ответ")
                        .Select(path => new Attachment(path))
                        .ToArray();
                }
                catch (NoAttachmentException)
                {
                    RaseNewMessage($"ERROR: {ogDoc.Number} - отсутствуют вложения");
                    continue;
                }
                RaseNewMessage($"{ogDoc.Number} - получен");
                ogDocuments.Add(ogDoc);
            }

            foreach (var ogDoc in ogDocuments)
            {
                try
                {
                    ogDoc.LoadResultsFromSed(_lotus, ResultReplicaId);
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
