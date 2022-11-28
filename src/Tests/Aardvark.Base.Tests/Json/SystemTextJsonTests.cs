using NUnit.Framework;
using System;

namespace Aardvark.Base.Tests.Json
{
    [TestFixture]
    class SystemTextJsonTests
    {
        #region Helpers

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

        #endregion

        #region Affine[23][df]

        [Test] public void Affine2d_Roundtrip() => RoundtripTest(new Affine2d(M22d.Identity));
        [Test] public void Affine2f_Roundtrip() => RoundtripTest(new Affine2f(M22f.Identity));
        [Test] public void Affine3d_Roundtrip() => RoundtripTest(new Affine3d(M33d.Identity));
        [Test] public void Affine3f_Roundtrip() => RoundtripTest(new Affine3f(M33f.Identity));

        #endregion

        #region Box[23][dfil]

        [Test] public void Box2i_Roundtrip() => RoundtripTest(new Box2i(new V2i(1, -2), new V2i(-17, 0)));
        [Test] public void Box2l_Roundtrip() => RoundtripTest(new Box2l(new V2l(1, -2), new V2l(-17, 0)));
        [Test] public void Box2f_Roundtrip() => RoundtripTest(new Box2f(new V2f(1.1, -2.2), new V2f(-17.17, 0)));
        [Test] public void Box2d_Roundtrip() => RoundtripTest(new Box2d(new V2d(1.1, -2.2), new V2d(-17.17, 0)));
        [Test] public void Box3i_Roundtrip() => RoundtripTest(new Box3i(new V3i(1, -2, 0), new V3i(-17, 42, -555555555)));
        [Test] public void Box3l_Roundtrip() => RoundtripTest(new Box3l(new V3l(1, -2, 0), new V3l(-17, 42, -555555555)));
        [Test] public void Box3f_Roundtrip() => RoundtripTest(new Box3f(new V3f(1.1, -2.2, 0), new V3f(-17.17, 42.42, -555.001)));
        [Test] public void Box3d_Roundtrip() => RoundtripTest(new Box3d(new V3d(1.1, -2.2, 0), new V3d(-17.18, 42.42, -555.001)));

        #endregion

        #region C[34][b|d|f|ui|us], CieLabf, CieLuvf, CieXYZf, CieYxyf, CMYKf, HSLf, HSVf, Yuvf
        // TODO
        #endregion

        #region Capsule3[df]

        [Test] public void Capsule3d_Roundtrip() => RoundtripTest(new Capsule3d(new V3d(1, -2, 3), new V3d(-4, 5, -6), 7.14));
        [Test] public void Capsule3f_Roundtrip() => RoundtripTest(new Capsule3f(new V3f(1, -2, 3), new V3f(-4, 5, -6), 7.14f));

        #endregion

        #region Cell, Cell2d

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
            DeserializeTest("{\"X\":1,\"Y\":2,\"Z\":3,\"E\":-1}", new Cell(1, 2, 3, -1));

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

        #endregion

        #region Circle[23][df]

        [Test] public void Circle2d_Roundtrip() => RoundtripTest(new Circle2d(new V2d(1.2, -3.4), 5.6));
        [Test] public void Circle2f_Roundtrip() => RoundtripTest(new Circle2f(new V2f(1.2f, -3.4f), 5.6f));
        [Test] public void Circle3d_Roundtrip() => RoundtripTest(new Circle3d(new V3d(1.2, -3.4, 5.6), new V3d(1,2,3).Normalized, 5.6));
        [Test] public void Circle3f_Roundtrip() => RoundtripTest(new Circle3f(new V3f(1.2, -3.4, 5.6), new V3f(1, 2, 3).Normalized, 5.6f));

        #endregion

        #region Cone[23][df]
        // TODO
        #endregion

        #region Cylinder3[df]
        // TODO
        #endregion

        #region Ellipse[23][df]
        // TODO
        #endregion

        #region Euclidean[23][df]
        // TODO
        #endregion

        #region Fraction
        // TODO
        #endregion

        #region Hull[23][df]

        [Test] public void Hull2d_Roundtrip() => RoundtripTest(new Hull2d(Box2d.Unit));
        [Test] public void Hull2f_Roundtrip() => RoundtripTest(new Hull2f(Box2f.Unit));

        [Test] public void Hull3d_Roundtrip() => RoundtripTest(new Hull3d(Box3d.Unit));
        [Test] public void Hull3f_Roundtrip() => RoundtripTest(new Hull3f(Box3f.Unit));

        #endregion

        #region Line[23][df]
        // TODO
        #endregion

        #region M[22|23|33|34|44][dfil]

        [Test] public void M22d_Roundtrip() => RoundtripTest(new M22d(1.2, -2.3, 3.4, -4.5));
        [Test] public void M22f_Roundtrip() => RoundtripTest(new M22f(1.2f, -2.3f, 3.4f, -4.5f));
        [Test] public void M22i_Roundtrip() => RoundtripTest(new M22i(1, -2, 3, -4));
        [Test] public void M22l_Roundtrip() => RoundtripTest(new M22l(1, -2, 3, -4));

        [Test] public void M23d_Roundtrip() => RoundtripTest(new M23d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7));
        [Test] public void M23f_Roundtrip() => RoundtripTest(new M23f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f));
        [Test] public void M23i_Roundtrip() => RoundtripTest(new M23i(1, -2, 3, -4, 5, -6));
        [Test] public void M23l_Roundtrip() => RoundtripTest(new M23l(1, -2, 3, -4, 6, -6));

        [Test] public void M33d_Roundtrip() => RoundtripTest(new M33d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7, 7.8, -8.9, 9.10));
        [Test] public void M33f_Roundtrip() => RoundtripTest(new M33f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f, 7.8f, -8.9f, 9.10f));
        [Test] public void M33i_Roundtrip() => RoundtripTest(new M33i(1, -2, 3, -4, 5, -6, 7, -8, 9));
        [Test] public void M33l_Roundtrip() => RoundtripTest(new M33l(1, -2, 3, -4, 6, -6, 7, -8, 9));

        [Test] public void M34d_Roundtrip() => RoundtripTest(new M34d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7, 7.8, -8.9, 9.10, -10.11, 11.12, -12.13));
        [Test] public void M34f_Roundtrip() => RoundtripTest(new M34f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f, 7.8f, -8.9f, 9.10f, -10.11f, 11.12f, -12.13f));
        [Test] public void M34i_Roundtrip() => RoundtripTest(new M34i(1, -2, 3, -4, 5, -6, 7, -8, 9, -10, 11, -12));
        [Test] public void M34l_Roundtrip() => RoundtripTest(new M34l(1, -2, 3, -4, 6, -6, 7, -8, 9, -10, 11, -12));

        [Test] public void M44d_Roundtrip() => RoundtripTest(new M44d(1.2, -2.3, 3.4, -4.5, 5.6, -6.7, 7.8, -8.9, 9.10, -10.11, 11.12, -12.13, 13.14, -14.15, 15.16, -16.17));
        [Test] public void M44f_Roundtrip() => RoundtripTest(new M44f(1.2f, -2.3f, 3.4f, -4.5f, 5.6f, -6.7f, 7.8f, -8.9f, 9.10f, -10.11f, 11.12f, -12.13f, 13.14f, -14.15f, 15.16f, -16.17f));
        [Test] public void M44i_Roundtrip() => RoundtripTest(new M44i(1, -2, 3, -4, 5, -6, 7, -8, 9, -10, 11, -12, 13, -14, 15, -16));
        [Test] public void M44l_Roundtrip() => RoundtripTest(new M44l(1, -2, 3, -4, 6, -6, 7, -8, 9, -10, 11, -12, 13, -14, 15, -16));

        #endregion

        #region OrientedBox[23][df]
        // TODO
        #endregion

        #region Plane[23][df]

        [Test] public void Plane2d_Roundtrip() => RoundtripTest(new Plane2d(new V2d(1.2, -3.4).Normalized, new V2d(-5.6, 7.8)));
        [Test] public void Plane2f_Roundtrip() => RoundtripTest(new Plane2f(new V2f(1.2, -3.4).Normalized, new V2f(-5.6, 7.8)));
        [Test] public void Plane3d_Roundtrip() => RoundtripTest(new Plane3d(new V3d(1.2, -3.4, 5.6).Normalized, new V3d(-7.8, 9.10, -11.12)));
        [Test] public void Plane3f_Roundtrip() => RoundtripTest(new Plane3f(new V3f(1.2, -3.4, 5.6).Normalized, new V3f(-7.8, 9.10, -11.12)));

        #endregion

        #region Polygon[23][df]
        // TODO
        #endregion

        #region Quad[23][df]
        // TODO
        #endregion

        #region Quadric[df]
        // TODO
        #endregion

        #region Quaternion[DF]
        // TODO
        #endregion

        #region Ray[23][df], FastRay[23][df]
        // TODO
        #endregion

        #region Rot[23][df]
        // TODO
        #endregion

        #region Scale[23][df]
        // TODO
        #endregion

        #region Shift[23][df]
        // TODO
        #endregion

        #region Similarity[23][df]
        // TODO
        #endregion

        #region Spectrum
        // TODO
        #endregion

        #region Sphere3[df]
        // TODO
        #endregion

        #region Torus3[df]
        // TODO
        #endregion

        #region Trafo[23][df]
        // TODO
        #endregion

        #region Triangle[23][df]
        // TODO
        #endregion

        #region V[234][dfil]

        [Test] public void V2f_Roundtrip() => RoundtripTest(new V2f(1.1, -2.2));
        [Test] public void V2d_Roundtrip() => RoundtripTest(new V2d(1.1, -2.2));
        [Test] public void V2i_Roundtrip() => RoundtripTest(new V2i(1, -2));
        [Test] public void V2l_Roundtrip() => RoundtripTest(new V2l(1, -2));

        [Test] public void V3f_Roundtrip() => RoundtripTest(new V3f(1.1, 0, -2.2));
        [Test] public void V3d_Roundtrip() => RoundtripTest(new V3d(1.1, 0, -2.2));
        [Test] public void V3i_Roundtrip() => RoundtripTest(new V3i(1, 0, -2));
        [Test] public void V3l_Roundtrip() => RoundtripTest(new V3l(1, 0, -2));

        [Test] public void V4f_Roundtrip() => RoundtripTest(new V4f(1.1, 0, -2.2, 3.3));
        [Test] public void V4d_Roundtrip() => RoundtripTest(new V4d(1.1, 0, -2.2, 3.3));
        [Test] public void V4i_Roundtrip() => RoundtripTest(new V4i(1, 0, -2, 3));
        [Test] public void V4l_Roundtrip() => RoundtripTest(new V4l(1, 0, -2, 3));

        #endregion
    }
}
