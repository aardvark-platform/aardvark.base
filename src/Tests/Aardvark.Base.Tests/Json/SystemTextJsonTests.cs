using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Tests.Json
{
    [TestFixture]
    class SystemTextJsonTests
    {
        private static void SerializeTest<T>(T a, string s) where T : IEquatable<T>
        {
            var json = System.Text.Json.JsonSerializer.Serialize(a);
            Assert.IsTrue(json == s);
        }
        private static void DeserializeTest<T>(string json, T a) where T : IEquatable<T>
        {
            var b = System.Text.Json.JsonSerializer.Deserialize<T>(json);
            Assert.IsTrue(a.Equals(b));
        }
        private static void RoundtripTest<T>(T a) where T : IEquatable<T>
        {
            var json = System.Text.Json.JsonSerializer.Serialize(a);
            var b = System.Text.Json.JsonSerializer.Deserialize<T>(json);
            Assert.IsTrue(a.Equals(b));
        }

        [Test]
        public void Cell2d_Serialize()
        {
            SerializeTest(new Cell2d(1, 2, -1), "[1,2,-1]");
            SerializeTest(new Cell2d(10), "[10]");
            SerializeTest(Cell2d.Invalid, $"[{int.MinValue}]");
        }
        [Test]
        public void Cell2d_Deserialize()
        {
            DeserializeTest("\"[1,2,-1]\"", new Cell2d(1, 2, -1));
            DeserializeTest("[1,2,-1]", new Cell2d(1, 2, -1));
            DeserializeTest("{\"x\":1,\"y\":2,\"e\":-1}", new Cell2d(1, 2, -1));
            DeserializeTest("{\"X\":1,\"Y\":2,\"E\":-1}", new Cell2d(1, 2, -1));

            DeserializeTest("\"[10]\"", new Cell2d(10));
            DeserializeTest("[10]", new Cell2d(10));
            DeserializeTest("{\"e\":10}", new Cell2d(10));
            DeserializeTest("{\"E\":10}", new Cell2d(10));

            DeserializeTest($"\"[{int.MinValue}]\"", Cell2d.Invalid);
            DeserializeTest($"[{int.MinValue}]", Cell2d.Invalid);
            DeserializeTest($"{{\"e\":{int.MinValue}}}", Cell2d.Invalid);
            DeserializeTest($"{{\"E\":{int.MinValue}}}", Cell2d.Invalid);
        }
        [Test]
        public void Cell2d_Roundtrip()
        {
            RoundtripTest(new Cell2d(1, 2, -1));
            RoundtripTest(new Cell2d(10));
            RoundtripTest(Cell2d.Invalid);
        }

        [Test]
        public void Cell_Serialize()
        {
            SerializeTest(new Cell(1, 2, 3, -1), "[1,2,3,-1]");
            SerializeTest(new Cell(10), "[10]");
            SerializeTest(Cell.Invalid, $"[{int.MinValue}]");
        }
        [Test]
        public void Cell_Deserialize()
        {
            DeserializeTest("\"[1,2,3,-1]\"", new Cell(1, 2, 3, -1));
            DeserializeTest("[1,2,3,-1]", new Cell(1, 2, 3, -1));
            DeserializeTest("{\"x\":1,\"y\":2,\"z\":3,\"e\":-1}", new Cell(1, 2, 3, -1));
            DeserializeTest("{\"X\":1,\"Y\":2,\"Z\":3,\"E\":-1}", new Cell(1, 2, 3,-1));

            DeserializeTest("\"[10]\"", new Cell(10));
            DeserializeTest("[10]", new Cell(10));
            DeserializeTest("{\"e\":10}", new Cell(10));
            DeserializeTest("{\"E\":10}", new Cell(10));

            DeserializeTest($"\"[{int.MinValue}]\"", Cell.Invalid);
            DeserializeTest($"[{int.MinValue}]", Cell.Invalid);
            DeserializeTest($"{{\"e\":{int.MinValue}}}", Cell.Invalid);
            DeserializeTest($"{{\"E\":{int.MinValue}}}", Cell.Invalid);
        }
        [Test]
        public void Cell_Roundtrip()
        {
            RoundtripTest(new Cell(1, 2, 3, -1));
            RoundtripTest(new Cell(10));
            RoundtripTest(Cell.Invalid);
        }

        [Test] public void Box2i_Roundtrip() => RoundtripTest(new Box2i(new V2i(1, -2), new V2i(-17, 0)));
        [Test] public void Box2l_Roundtrip() => RoundtripTest(new Box2l(new V2l(1, -2), new V2l(-17, 0)));
        [Test] public void Box2f_Roundtrip() => RoundtripTest(new Box2f(new V2f(1.1, -2.2), new V2f(-17.17, 0)));
        [Test] public void Box2d_Roundtrip() => RoundtripTest(new Box2d(new V2d(1.1, -2.2), new V2d(-17.17, 0)));
        [Test] public void Box3i_Roundtrip() => RoundtripTest(new Box3i(new V3i(1, -2, 0), new V3i(-17, 42, -555555555)));
        [Test] public void Box3l_Roundtrip() => RoundtripTest(new Box3l(new V3l(1, -2, 0), new V3l(-17, 42, -555555555)));
        [Test] public void Box3f_Roundtrip() => RoundtripTest(new Box3f(new V3f(1.1, -2.2, 0), new V3f(-17.17, 42.42, -555.001)));
        [Test] public void Box3d_Roundtrip() => RoundtripTest(new Box3d(new V3d(1.1, -2.2, 0), new V3d(-17.18, 42.42, -555.001)));

        [Test] public void Hull2d() => RoundtripTest(new Hull2d(Box2d.Unit));
        [Test] public void Hull2f() => RoundtripTest(new Hull2f(Box2f.Unit));

        [Test] public void Plane2d() => RoundtripTest(new Plane2d(new V2d(1.2, -3.4).Normalized, new V2d(-5.6, 7.8)));
        [Test] public void Plane2f() => RoundtripTest(new Plane2f(new V2f(1.2, -3.4).Normalized, new V2f(-5.6, 7.8)));
        [Test] public void Plane3d() => RoundtripTest(new Plane3d(new V3d(1.2, -3.4, 5.6).Normalized, new V3d(-7.8, 9.10, -11.12)));
        [Test] public void Plane3f() => RoundtripTest(new Plane3f(new V3f(1.2, -3.4, 5.6).Normalized, new V3f(-7.8, 9.10, -11.12)));

        [Test] public void V2i() => RoundtripTest(new V2i(1, -2));
        [Test] public void V2l() => RoundtripTest(new V2l(1, -2));
        [Test] public void V2f() => RoundtripTest(new V2f(1.1, -2.2));
        [Test] public void V2d() => RoundtripTest(new V2d(1.1, -2.2));
        [Test] public void V3i() => RoundtripTest(new V3i(1, 0, -2));
        [Test] public void V3l() => RoundtripTest(new V3l(1, 0, -2));
        [Test] public void V3f() => RoundtripTest(new V3f(1.1, 0, -2.2));
        [Test] public void V3d() => RoundtripTest(new V3d(1.1, 0, -2.2));
        [Test] public void V4i() => RoundtripTest(new V4i(1, 0, -2, 3));
        [Test] public void V4l() => RoundtripTest(new V4l(1, 0, -2, 3));
        [Test] public void V4f() => RoundtripTest(new V4f(1.1, 0, -2.2, 3.3));
        [Test] public void V4d() => RoundtripTest(new V4d(1.1, 0, -2.2, 3.3));
    }
}
