using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using fSedToSstu;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ZipTest
    {
        [TestMethod]
        public void TestZip()
        {
            Directory.CreateDirectory("dir");
            try
            {
                using (
                    Stream stream =
                        Assembly.GetExecutingAssembly()
                            .GetManifestResourceStream("UnitTests.Properties.Resources.resources"))
                using (BinaryReader reader = new BinaryReader(stream))
                using (var zip = new ZipArchiveParted(Path.Combine("dir", "template.zip"), 5e+7))
                {
                    var content = Convert.ToBase64String(reader.ReadBytes((int)stream.Length));
                    foreach (var i in Enumerable.Range(0, 300))
                    {
                        var name = Path.ChangeExtension(Guid.NewGuid().ToString(), "json");
                        var entry = zip.CreateEntry(name);
                        using (var entryStream = entry.Open())
                        using (var sw = new StreamWriter(entryStream))
                        {
                            sw.Write(content);
                        }
                    }
                    Assert.IsTrue(zip.PartsCount == Directory.GetFiles("dir").Length);
                }
            }
            finally
            {
                Directory.Delete("dir", true);
            }

        }
    }
}
