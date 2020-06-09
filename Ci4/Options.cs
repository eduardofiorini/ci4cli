using CommandLine;
using System.Collections.Generic;

namespace Ci4
{
    class Options
    {
        [Option('n', "new", HelpText = "Command create new project, template or api." +
                        "\n■ NEW API: ci4 -n api [NAME]" +
                        "\n■ NEW PROJECT: ci4 -n project [NAME] [VERSION]" +
                        "\n■ NEW TRANSLATE: ci4 -n translate [ISO CODE]" +
                        "\n■ NEW TEMPLATE: ci4 -n template [TEMPLATE]")]
        public IEnumerable<string> NewCommand { get; set; }

        [Option('c', "create", HelpText = "Command create page, crud, model or helper." +
                        "\n■ CREATE PAGE: ci4 -c page [NAME]" +
                        "\n■ CREATE ROUTE: ci4 -c route [NAME URL] [CONTROLLER] [FUNCTION] [METHOD GET, POST or * FOR ALL]" +
                        "\n■ CREATE CRUD: ci4 -c crud [TABLE]" +
                        "\n■ CREATE MODEL: ci4 -c model [TABLE] or [* FOR ALL]" +
                        "\n■ CREATE HELPER: ci4 -c helper [NAME]")]
        public IEnumerable<string> CreateCommand { get; set; }

        [Option('l', "list", HelpText = "Command list itens." +
                        "\n■ LIST TRANSLATE: ci4 -l translate" +
                        "\n■ LIST TEMPLATE: ci4 -l template")]
        public IEnumerable<string> listCommand { get; set; }

        [Option('s', "server", HelpText = "Command start or stop server php." +
                        "\n■ START SERVER: ci4 -s start [PORT]" +
                        "\n■ STOP SERVER: ci4 -s stop")]
        public IEnumerable<string> ServerCommand { get; set; }
    }
}
