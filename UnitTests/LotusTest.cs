using System.IO;
using LotusLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class LotusTest
    {
        [TestMethod]
        public void LotusTestMethod()
        {
            var lotus = new Lotus(@"AppSrv74/U74/RND");
            lotus.Init("123456");
            var doc = lotus.SearchDocumentsByFormula("4525807B001CF66F",
                "[registration_number] contains \"1271-2/17-17\"").GetFirstDocument();
            foreach (var attachment in lotus.GetAttachments(doc, "ответ"))
            {
                Assert.IsTrue(File.Exists(attachment));
                File.Delete(attachment);
            }
        }

        [TestMethod]
        public void OgResultSearch()
        {
            var lotus = new Lotus(@"AppSrv74/U74/RND");
            lotus.Init("123456");
            var doc = lotus.SearchDocumentsByFormula("4525807B001D7F74",
                "[document_groupID] contains \"919DE51E60BD949E45258148002854EA\"").GetFirstDocument();
        }
    }
}
