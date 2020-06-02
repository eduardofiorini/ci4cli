using CommandLine;
using System.Collections.Generic;

namespace Ci4
{
    class Options
    {
        [Option('n', "new", HelpText = "Command create new project, template or api." +
            "\nNEW PROJECT: ci4 -n project [NAME]" +
            "\nNEW TEMPLATE: ci4 -n template [TEMPLATE]" +
            "\nNEW API: ci4 -n api [NAME]")]
        public IEnumerable<string> NewCommand { get; set; }

        [Option('c', "create", HelpText = "Command create page, crud, model or helper." +
            "\nCREATE PAGE: ci4 -c page [NAME]" +
            "\nCREATE ROUTE: ci4 -c route [NAME URL] [CONTROLLER] [FUNCTION] [METHOD GET, POST or * FOR ALL]" +
            "\nCREATE CRUD: ci4 -c crud [TABLE]" +
            "\nCREATE MODEL: ci4 -c model [TABLE] or [* FOR ALL]" +
            "\nCREATE PAGE: ci4 -c helper [NAME]")]
        public IEnumerable<string> CreateCommand { get; set; }

        [Option('s', "server", HelpText = "Command start or stop server php." +
            "\nSTART SERVER: ci4 -s start [PORT]" +
            "\nSTOP SERVER: ci4 -s stop")]
        public IEnumerable<string> ServerCommand { get; set; }
    }
}
