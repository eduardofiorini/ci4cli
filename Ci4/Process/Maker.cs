using Colorify;
using System;
using System.IO;

namespace Ci4.Process
{
    class Maker
    {
        public static void Cell(string name)
        {
            Build(name, "CellView", @"\app\Cells", "< !--Your HTML here-- >");
            Build(name, "CellController", @"\app\Cells", "// Your Code here");
        }

        public static void Command(string name)
        {
            Build(name, "Command", @"\app\Commands", "// Your Code here");
        }

        public static void Config(string name)
        {
            Build(name, "Config", @"\app\Config", "// Your Code here");
        }

        public static void Controller(string name)
        {
            Build(name, "Controller", @"\app\Controllers", "// Your Code here");
        }

        public static void Entitie(string name)
        {
            Build(name, "Entitie", @"\app\Entities", "// Your Code here");
        }

        public static void Filter(string name)
        {
            Build(name, "Filter", @"\app\Filters", "// Your Code here");
        }

        public static void Migration(string name)
        {
            Build(name, "Migration", @"\app\Database\Migrations", "// Your Code here");
        }

        public static void Seed(string name)
        {
            Build(name, "Seed", @"\app\Database\Seeds", "// Your Code here");
        }

        public static void Model(string name)
        {
            Build(name, "Model", @"\app\Models", "// Your Code here");
        }

        public static void Validation(string name)
        {
            Build(name, "Validation", @"\app\Validation", "// Your Code here");
        }

        public static void Helper(string name)
        {
            Build(name, "Helper", @"\app\Helpers", "// Your Code here");
        }

        public static void Build(string name, string scheme, string path, string value)
        {
            var pathLocal = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ci4.dll", "");
            var pathDir = Environment.CurrentDirectory + path;

            if (!Directory.Exists(pathDir))
                Directory.CreateDirectory(pathDir);

            try
            {
                var className = name.ToLower();
                if (scheme == "CellController")
                    className = $"{className}Cell";
                string content = File.ReadAllText($@"{pathLocal}Schemes\Create\{scheme}.tpl");
                content = content.Replace("[CLASSNAME]", $"{char.ToUpper(className[0])}{className.Substring(1)}");
                content = content.Replace("[CLASSVALUE]", value);
                if (scheme != "CellView" && scheme != "Helper")
                    className = $"{char.ToUpper(className[0])}{className.Substring(1)}";
                if (scheme == "Migration")
                    className = $"{DateTime.Now.ToString("yyyy-MM-dd-HHmmss")}_{className}";
                if (scheme == "Helper")
                    className = $"{className}_helper";
                string fileOutput = Path.Combine(pathDir, $"{className}.php");
                File.WriteAllText(fileOutput, content);
                Program._colorify.WriteLine($"{scheme} \"{name.ToLower()}\" successfully created!", Colors.bgSuccess);
            }
            catch (Exception ex)
            {
                Program._colorify.WriteLine($"{scheme} \"{name.ToLower()}\" error: {ex.Message}", Colors.bgDanger);
            }

        }

    }
}
