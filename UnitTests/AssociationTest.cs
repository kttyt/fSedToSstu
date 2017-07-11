using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LotusLib;
using LotusLib.Auxiliary;
using LotusLib.Documents;
using LotusLib.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class AssociationTest
    {
        [TestMethod]
        public void AssociationTestMethod()
        {
            var lotus = new Lotus(@"AppSrv74/U74/RND");
            lotus.Init("123456");
            var docs = lotus.SearchDocumentsByFormula("4525807B001CF66F",
                "[StatusId] contains \"in_file\" and [acceptance_date] >  01/06/2017 and [Form] contains \"doc_og\"");
            var totalCount = docs.Count;
            var ogDocuments = new List<OgDocument>();
            for (var count = 1; count <= totalCount; count++)
            {
                var doc = docs.GetNthDocument(count);
                var ogDoc = OgDocument.FromNotes(doc);
                try
                {
                    ogDoc.Attachments = lotus.GetAttachments(doc, "ответ")
                        .Select(path => new Attachment(path))
                        .ToArray();
                }
                catch (NoAttachmentException)
                {
                    File.AppendAllText("log.txt", $"{ogDoc.Number} - отсутствуют вложения\n");
                }

                ogDocuments.Add(ogDoc);
            }
            foreach (var ogDoc in ogDocuments)
            {
                try
                {
                    ogDoc.LoadResultsFromSed(lotus, "");
                }
                catch (Exception)
                {
                    File.AppendAllText("log.txt", $"{ogDoc.Number} - ошибка при загрузке результатов\n");
                }
            }
            foreach (var ogDoc in ogDocuments)
            {
                var isValid = ogDoc.ValidateResults();
                if(!isValid)
                    File.AppendAllText("log.txt", $"{ogDoc.Number} - ошибка при валидации результатов\n");

            }
            Assert.IsTrue(ogDocuments.All(doc => doc.ValidateResults()));
        }
    }
}
