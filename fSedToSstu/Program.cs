using System;
using System.IO;
using System.IO.Compression;
using LotusLib;
using SstuLib;
using SstuLib.Auxiliary;
using SstuLib.Exceptions;

namespace fSedToSstu
{
    class Program
    {
        static void Main(string[] args)
        {
            var loader = new OgLoader(@"ServerName", "password")
            {
                OgReplicaId = "4525807B011CF66F",
                ResultReplicaId = "4525807C001D7F74"
            };
            loader.OnError += (o, foo) => WriteLog(foo);

            var documents = loader.Get("[acceptance_date] >=  01/06/2017 and [acceptance_date] <  01/07/2017 and [Form] contains \"doc_og\"");

            using (var ms = File.Create("temp.zip"))
            using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var document in documents)
                {
                    if (ms.Length > 1e+8)
                        throw new Exception("Слишком большой размер архива");
                    Request req;
                    try
                    {
                        req = document.CreateRequest();
                        var lowNum = req.Number.ToLower();
                        if (lowNum.StartsWith("a26") || lowNum.StartsWith("а26")) //en, ru
                        {
                            WriteLog($"Пропускается {document.Number} - причина неподдерживаемый номер {req.Number}");
                            continue;
                        }
                    }
                    catch (ValidationException)
                    {
                        WriteLog($"Ошибка при валидации документа {document.Number}");
                        continue;
                    }
                    catch (Exception ex)
                    {
                        WriteLog($"Ошибка при обработке документа {document.Number}; '{ex.Message}'");
                        continue;
                    }
                    var name = Path.ChangeExtension(Guid.NewGuid().ToString(), "json");
                    var entry = zipArchive.CreateEntry(name, CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    using (var sw = new StreamWriter(entryStream))
                    {
                        sw.Write(Converter.Convert(req));
                    }
                }
            }
        }

        public static void WriteLog(string msg)
        {
            var formatMsg = $"{DateTime.Now.ToString("s")}: {msg}";
            Console.WriteLine(formatMsg);
            File.AppendAllLines("log.txt", new [] { formatMsg });
        }
    }
}
