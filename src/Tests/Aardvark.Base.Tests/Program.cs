using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //new BoxTests().BoxTransformTest();
            //Extensions.DateTimeTests.JulinaDay();
            //new SamplerStateTest().SamplerStateTestHashCollision();
            new ReportingTests().MultiThread();
        }
    }
}
