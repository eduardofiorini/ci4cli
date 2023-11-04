using CommandLine;
using System.Collections.Generic;

namespace Ci4
{
    class Options
    {
        //Menu New
        [Option('n', "new", HelpText = "The \"New\" command executes the creation of new projects:" +
                        "\n■ NEW PROJECT: ci4 -n project [NAME]"
            )]
        public IEnumerable<string> NewCommand { get; set; }

        //Menu Create
        [Option('c', "create", HelpText = "The \"Create\" command performs CI4 standard file creation:" +
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
                        "\n■ CREATE HELPERS: ci4 -c helper [NAME]" +
                        "\n■ CREATE MVC: ci4 -c mvc [NAME]"+
                        "\n■ CREATE scaffold: ci4 -c scaffold [NAME]"
            )]
        public IEnumerable<string> CreateCommand { get; set; }

        //Menu Make
        [Option('m', "make", HelpText = "The \"Make\" command performs automated tasks:" +
                        "\n■ MAKE LOGIN: ci4 -m login" +
                        "\n■ MAKE OAUTH: ci4 -m oauth [PROVIDER]" +
                        "\n■ MAKE CRUD: ci4 -m crud [TABLE]" +
                        "\n■ MAKE API BASIC: ci4 -m api [TABLE]" +
                        "\n■ MAKE API JWT: ci4 -m jwt [TABLE]" +
                        "\n■ MAKE DB TO MIGRATION: ci4 -m migration [TABLE]"
            )]
        public IEnumerable<string> MakeCommand { get; set; }

        //Menu Theme
        [Option('t', "theme", HelpText = "The \"Theme\" command downloads the free template below for your project:" +
                        "\n■ THEME 1: ci4 -t sbadmin2" +
                        "\n■ THEME 2: ci4 -t mazer" +
                        "\n■ THEME 3: ci4 -t elegant" +
                        "\n■ THEME 4: ci4 -t adminlte" +
                        "\n■ THEME 5: ci4 -t dashboardkit" +
                        "\n■ THEME 6: ci4 -t modernize" +
                        "\n■ THEME 7: ci4 -t sneat"
            )]
        public IEnumerable<string> ThemeCommand { get; set; }
    }
}