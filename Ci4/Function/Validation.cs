using Colorify;
using System;
using System.IO;

namespace Ci4.Function
{
    class Validation
    {
        public static bool ExistDirectory()
        {
            var check = true;

            if (!Directory.Exists(Environment.CurrentDirectory + @"\app\"))
                check = false;

            return check;
        }
    }
}
