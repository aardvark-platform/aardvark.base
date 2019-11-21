using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Tests
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new DictTests().Run();
        }
    }
}
