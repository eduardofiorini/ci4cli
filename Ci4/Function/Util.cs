using System.IO;
using System.Linq;

namespace Ci4.Function
{
    class Util
    {
        public static string FormatNameUp(string name)
        {
            name = name.ToLower();
            return char.ToUpper(name[0]) + name.Substring(1);
        }

        public static string FormatName(string name)
        {
            if (name.Contains("_"))
            {
                var n = "";
                string[] listName = name.Split('_');
                foreach (var item in listName)
                {
                    n += FormatNameUp(item);
                }
                return n;
            }
            else
            {
                if (name.Contains("#"))
                {
                    var n = "";
                    string[] listName = name.Split('#');
                    foreach (var item in listName)
                    {
                        n += " " + FormatNameUp(item);
                    }
                    return n.Trim();
                }
                return FormatNameUp(name);
            }
        }

        public static void InsertLine(string filePaht, int line, string content)
        {
            var txtLines = File.ReadAllLines(filePaht).ToList();
            txtLines.Insert(line, content);
            File.WriteAllLines(filePaht, txtLines);
        }
    }
}
