using System;
using System.IO;
using CommandLine;
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
            Guid departmentId;
            var options = ParseArgs(args, out departmentId);

            var loader = new OgLoader(options.ServerName, options.Password)
            {
                OgReplicaId = options.OgReplicaId,
                ResultReplicaId = options.ResultReplicaId
            };
            loader.OnError += (o, foo) => WriteLog(foo);

            var documents = loader.Get(options.SearchQuery);
            WriteLog($"Найдено {documents.Count} по запросу '{options.SearchQuery}'");

            using (var zipArchive = new ZipArchiveParted(options.OutputFilename, 5e+7))
            {
                foreach (var document in documents)
                {
                    Request req;
                    try
                    {
                        req = document.CreateRequest();
                        req.DepartmentId = departmentId;
                        var lowNum = req.Number.ToLower();
                        if (lowNum.StartsWith("a26") || lowNum.StartsWith("а26")) //en, ru
                        {
                            WriteLog($"WARNING: Пропускается {document.Number} - неподдерживаемый номер {req.Number}");
                            continue;
                        }
                    }
                    catch (ValidationException)
                    {
                        WriteLog($"ERROR: Ошибка при валидации документа {document.Number}");
                        continue;
                    }
                    catch (Exception ex)
                    {
                        WriteLog($"ERROR: Ошибка при обработке документа {document.Number} - {ex.Message}");
                        continue;
                    }
                    var name = Path.ChangeExtension(Guid.NewGuid().ToString(), "json");
                    var entry = zipArchive.CreateEntry(name);
                    using (var entryStream = entry.Open())
                    using (var sw = new StreamWriter(entryStream))
                    {
                        sw.Write(Converter.Convert(req));
                    }
                }
            }
        }

        private static Options ParseArgs(string[] args, out Guid departmentId)
        {
            var options = new Options();
            var isValid = Parser.Default.ParseArgumentsStrict(args, options);
            var isValidId = Guid.TryParse(options.DepartmentIdRaw, out departmentId);

            if (!isValid || !isValidId)
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(1);
            }
            return options;
        }

        public static void WriteLog(string msg)
        {
            var formatMsg = $"{DateTime.Now.ToString("s")}: {msg}";
            var color = Console.ForegroundColor;
            if(formatMsg.Contains("ERROR"))
                Console.ForegroundColor = ConsoleColor.Red;
            if (formatMsg.Contains("WARNING"))
                Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(formatMsg);
            Console.ForegroundColor = color;
            File.AppendAllLines("log.txt", new [] { formatMsg });
        }
    }
}
