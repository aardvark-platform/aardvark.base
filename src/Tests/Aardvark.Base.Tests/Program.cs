using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Aardvark.Tests
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var o = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
            Console.WriteLine(JsonSerializer.Serialize(V3d.XAxis), o);
            Console.WriteLine(new Hull2d());
            var h = Hull2d.Parse(new Hull2d().ToString());

            //new DictTests().Run();
            //Aardvark.Tests.Extensions.DateTimeTests.JulianDay();
            //new Aardvark.Tests.TrafoTests().TrafoDecomposeTest();
            //Aardvark.Tests.Rot3dTests.FromM33d();
            //Aardvark.Tests.Rot3dTests.FromEuler();
            //Aardvark.Tests.Rot3dTests.FromInto();
            //new Aardvark.Tests.Images.PixImageTests().MipMapCreate1x1();
        }
    }
}
