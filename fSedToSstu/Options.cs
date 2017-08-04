using CommandLine;
using CommandLine.Text;

namespace fSedToSstu
{
    public class Options
    {
        [Option('q', "query", //example: [acceptance_date] >= 01/07/2017 and [acceptance_date] < 01/08/2017 and [Form] contains "doc_og"
         Required = true,
         HelpText = "Запрос для поиска выгружаемых дел")]
        public string SearchQuery { get; set; }

        [Option('o', "output",
            DefaultValue = "temp.zip",
            HelpText = "Имя файла выгрузки")]
        public string OutputFilename { get; set; }

        [Option('p', "password",
            DefaultValue = "123456",
            HelpText = "Пароль от уч. запись Lotus Domino")]
        public string Password { get; set; }

        [Option('s', "server",
            DefaultValue = @"ServerName/RND",
            HelpText = "Имя сервера Lotus Domino")]
        public string ServerName { get; set; }

        [Option("ogid",
            DefaultValue = @"4525807B00EEEEEE",
            HelpText = "ID базы Канцелярия ОГ")]
        public string OgReplicaId { get; set; }

        [Option("resultid",
            DefaultValue = @"4525807B00EEEEEE",
            HelpText = "ID базы Решения ОГ")]
        public string ResultReplicaId { get; set; }

        [Option("departmentid",
            DefaultValue = "00000000-0000-0000-0000-000000000000",
            HelpText = "ID органа в системе ССТУ")]
        public string DepartmentIdRaw { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
