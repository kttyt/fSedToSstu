using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SstuLib;
using SstuLib.Auxiliary;
using SstuLib.Questions;

namespace UnitTests
{
    [TestClass]
    public class SstuTest
    {
        [TestMethod]
        public void SstuTestMethod()
        {
            var req = new Request
            {
                CreateDate = DateTime.Now.AddDays(-1),
                IsDirect = false,
                Name = "Иванов Иван Иванович",
                Number = "123/test"
            };
            req.Questions.Add(new InWork(req)
            {
                Code = "0001",
                IncomingDate = DateTime.Now,
                RegistrationDate = DateTime.Now
            });
            Assert.IsTrue(req.Validate());
            File.WriteAllText("InWork.json", Converter.Convert(req));

            var assambly = Assembly.GetExecutingAssembly();
            using (Stream stream = assambly.GetManifestResourceStream("UnitTests.Properties.Resources.resources"))
            using (BinaryReader reader = new BinaryReader(stream))
                req.Questions.Add(new Explained(req)
                {
                    Code = "0002",
                    IncomingDate = DateTime.Now,
                    RegistrationDate = DateTime.Now,
                    ResponseDate = DateTime.Now,
                    Attachment = new Attachment("test.pdf", reader.ReadBytes((int)stream.Length))
                });
            Assert.IsTrue(req.Validate());
            File.WriteAllText("Explained.json", Converter.Convert(req));

            var req1 = new Request
            {
                IsDirect = true,
                Name = "Иванов Иван Иванович",
                Number = "123/test"
            };
            req1.Questions.Add(new InWork(req)
            {
                Code = "0001",
                IncomingDate = DateTime.Now,
                RegistrationDate = DateTime.Now
            });
            Assert.IsTrue(req.Validate());
        }
    }
}
