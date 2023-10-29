using CommandLine;
using System.Collections.Generic;

namespace Ci4
{
    class Options
    {
        [Option('n', "new", HelpText = "Commands for creating new projects:" +
                        "\n■ NEW PROJECT: ci4 -n project [NAME]"
            )]
        public IEnumerable<string> NewCommand { get; set; }

        [Option('c', "create", HelpText = "Commands to create files in the project:" +
                        "\n■ CREATE CELLS: ci4 -c cell [NAME]" +
                        "\n■ CREATE COMMANDS: ci4 -c command [NAME]" +
                        "\n■ CREATE CONFIGS: ci4 -c config [NAME]" +
                        "\n■ CREATE CONTROLLERS: ci4 -c controller [NAME]" +
                        "\n■ CREATE ENTITIES: ci4 -c entitie [NAME]" +
                        "\n■ CREATE FILTERS: ci4 -c filter [NAME]" +
                        "\n■ CREATE MIGRATIONS: ci4 -c migration [NAME]" +
                        "\n■ CREATE SEEDS: ci4 -c seed [NAME]" +
                        "\n■ CREATE MODELS: ci4 -c model [NAME]" +
                        "\n■ CREATE VALIDATIONS: ci4 -c validation [NAME]" +
                        "\n■ CREATE HELPERS: ci4 -c helper [NAME]"
            )]
        public IEnumerable<string> CreateCommand { get; set; }

        [Option('l', "list", HelpText = "Command list itens." +
                        "\n■ LIST TRANSLATE: ci4 -l translate" +
                        "\n■ LIST TEMPLATE: ci4 -l template"
            )]
        public IEnumerable<string> listCommand { get; set; }

        [Option('s', "server", HelpText = "Command start or stop server php." +
                        "\n■ START SERVER: ci4 -s start [PORT]" +
                        "\n■ STOP SERVER: ci4 -s stop"
            )]
        public IEnumerable<string> ServerCommand { get; set; }
    }
}