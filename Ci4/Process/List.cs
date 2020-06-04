using Colorify;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ci4.Process
{
    class List
    {
        public static void Translate()
        {
            Program._colorify.WriteLine("+---------------------+", Colors.bgDefault);
            Program._colorify.WriteLine("| ISO CODE | LANGUAGE |", Colors.bgDefault);
            Program._colorify.WriteLine("+----------+----------+", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic   |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic   |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic   |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic   |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic   |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic   |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic   |", Colors.bgDefault);
            Program._colorify.WriteLine("+---------------------+", Colors.bgDefault);
        }
        public static void Template()
        {
            Program._colorify.WriteLine("+-----------------------------------------------------+", Colors.bgDefault);
            Program._colorify.WriteLine("| CODE     | AUTHOR      | GITHUB                     | ", Colors.bgDefault);
            Program._colorify.WriteLine("+----------+-------------+----------------------------+", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic      |                            |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic      |                            |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic      |                            |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic      |                            |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic      |                            |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic      |                            |", Colors.bgDefault);
            Program._colorify.WriteLine("| ar       | Arabic      |                            |", Colors.bgDefault);
            Program._colorify.WriteLine("+-----------------------------------------------------+", Colors.bgDefault);
        }
    }
}
