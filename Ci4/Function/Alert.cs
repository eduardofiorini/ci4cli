using Colorify;

namespace Ci4.Function
{
    class Alert
    {
        public static void ExistDirectory()
        {
            Program._colorify.WriteLine("Attention!", Colors.bgDanger);
            Program._colorify.WriteLine("Could not find default structure directories.", Colors.bgDanger);
            Program._colorify.WriteLine("Verify that you are running the commands in the root directory of your project.", Colors.bgDanger);
            Program._colorify.ResetColor();
        }

        public static void ExistFileEnv()
        {
            Program._colorify.WriteLine("Attention!", Colors.bgDanger);
            Program._colorify.WriteLine("File .env not found.", Colors.bgDanger);
        }
    }
}
