/*
    Copyright (C) 2017. Aardvark Platform Team. http://github.com/aardvark-platform.
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Tests
{
    [TestFixture]
    public class BoxTests
    {
        #region contains/intersects

        [Test]
        public void ValidBoxContainsItself()
        {
            var a = Box3d.Unit;
            Assert.IsTrue(a.Contains(a));
        }

        [Test]
        public void ValidBoxContainsValidBox()
        {
            var a = new Box3d(new V3d(0, 0, 0), new V3d(3, 3, 3));
            var b = new Box3d(new V3d(1, 1, 1), new V3d(2, 2, 2));
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        [Ignore("Check for invalid box is not implemented for performance reasons.")]
        public void InvalidBoxDoesNotContainItself()
        {
            var a = Box3d.Invalid;
            Assert.IsTrue(!a.Contains(a));
        }

        [Test]
        [Ignore("Check for invalid box is not implemented for performance reasons.")]
        public void ValidBoxDoesNotContainInvalidBox()
        {
            var a = new Box3d(new V3d(0, 0, 0), new V3d(3, 3, 3));
            var b = Box3d.Invalid;
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void InvalidBoxDoesNotContainValidBox()
        {
            var a = Box3d.Invalid;
            var b = new Box3d(new V3d(1, 1, 1), new V3d(2, 2, 2));
            Assert.IsTrue(!a.Contains(b));
        }



        [Test]
        public void ValidBoxIntersectsItself()
        {
            var a = Box3d.Unit;
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void InvalidBoxDoesNotIntersectItself()
        {
            var a = Box3d.Invalid;
            Assert.IsTrue(!a.Intersects(a));
        }

        [Test]
        public void ValidBoxDoesNotIntersectInvalidBox()
        {
            var a = new Box3d(new V3d(0, 0, 0), new V3d(1, 1, 1));
            var b = Box3d.Invalid;
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void InvalidBoxDoesNotIntersectValidBox()
        {
            var a = Box3d.Invalid;
            var b = new Box3d(new V3d(1, 0, 0), new V3d(2, 1, 1));
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void TouchingBoxesDoNotIntersect()
        {
            var a = new Box3d(new V3d(0, 0, 0), new V3d(1, 1, 1));
            var b = new Box3d(new V3d(1, 0, 0), new V3d(2, 1, 1));
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void BoxDoesIntersectItself()
        {
            var a = new Box3d(new V3d(1, 2, 3), new V3d(4, 5, 6));
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void ContainedBoxesDoIntersect()
        {
            var a = new Box3d(new V3d(0, 0, 0), new V3d(3, 3, 3));
            var b = new Box3d(new V3d(1, 1, 1), new V3d(2, 2, 2));
            Assert.IsTrue(a.Intersects(b));
        }

        #endregion

        #region Transformed

        static Box3d Transform1(Box3d box, M44d trafo)
        {
            var res = Box3d.Invalid;
            if (!box.IsInvalid)
            {
                res.ExtendBy(trafo.TransformPos((V3d)box.Min));
                res.ExtendBy(trafo.TransformPos((V3d)new V3d(box.Max.X, box.Min.Y, box.Min.Z)));
                res.ExtendBy(trafo.TransformPos((V3d)new V3d(box.Min.X, box.Max.Y, box.Min.Z)));
                res.ExtendBy(trafo.TransformPos((V3d)new V3d(box.Max.X, box.Max.Y, box.Min.Z)));
                res.ExtendBy(trafo.TransformPos((V3d)new V3d(box.Min.X, box.Min.Y, box.Max.Z)));
                res.ExtendBy(trafo.TransformPos((V3d)new V3d(box.Max.X, box.Min.Y, box.Max.Z)));
                res.ExtendBy(trafo.TransformPos((V3d)new V3d(box.Min.X, box.Max.Y, box.Max.Z)));
                res.ExtendBy(trafo.TransformPos((V3d)box.Max));
            }
            return res;
        }

        static Box3d Transform2(Box3d box, M44d trafo)
        {
            var xa = trafo.C0.XYZ * box.Min.X;
            var xb = trafo.C0.XYZ * box.Max.X;

            var ya = trafo.C1.XYZ * box.Min.Y;
            var yb = trafo.C1.XYZ * box.Max.Y;

            var za = trafo.C2.XYZ * box.Min.Z;
            var zb = trafo.C2.XYZ * box.Max.Z;

            return new Box3d(
                V3d.Min(xa, xb) + V3d.Min(ya, yb) + V3d.Min(za, zb) + trafo.C3.XYZ,
                V3d.Max(xa, xb) + V3d.Max(ya, yb) + V3d.Max(za, zb) + trafo.C3.XYZ);
        }

        static Box3d Transform3(Box3d box, M44d trafo)
        {
            var min = new V3d(trafo.M03, trafo.M13, trafo.M23);
            var max = min;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    var a = trafo[i, j];
                    var av = a * box.Min[j];
                    var bv = a * box.Max[j];
                    if (av < bv)
                    {
                        min[i] += av;
                        max[i] += bv;
                    }
                    else
                    {
                        min[i] += bv;
                        max[i] += av;
                    }
                }
            return new Box3d(min, max);
        }

        static Box3d Transform4(Box3d box, M44d trafo)
        {
            //if (!box.IsValid) return Box3d.Invalid;
            //if (box.IsInvalid) return Box3d.Invalid;
            var t = new V3d(trafo.M03, trafo.M13, trafo.M23);
            var res = new Box3d(t, t);
            // unrolled
            double av, bv;
            av = trafo.M00 * box.Min.X;
            bv = trafo.M00 * box.Max.X;
            if (av < bv)
            {
                res.Min.X += av;
                res.Max.X += bv;
            }
            else
            {
                res.Min.X += bv;
                res.Max.X += av;
            }

            av = trafo.M01 * box.Min.Y;
            bv = trafo.M01 * box.Max.Y;
            if (av < bv)
            {
                res.Min.X += av;
                res.Max.X += bv;
            }
            else
            {
                res.Min.X += bv;
                res.Max.X += av;
            }

            av = trafo.M02 * box.Min.Z;
            bv = trafo.M02 * box.Max.Z;
            if (av < bv)
            {
                res.Min.X += av;
                res.Max.X += bv;
            }
            else
            {
                res.Min.X += bv;
                res.Max.X += av;
            }

            av = trafo.M10 * box.Min.X;
            bv = trafo.M10 * box.Max.X;
            if (av < bv)
            {
                res.Min.Y += av;
                res.Max.Y += bv;
            }
            else
            {
                res.Min.Y += bv;
                res.Max.Y += av;
            }

            av = trafo.M11 * box.Min.Y;
            bv = trafo.M11 * box.Max.Y;
            if (av < bv)
            {
                res.Min.Y += av;
                res.Max.Y += bv;
            }
            else
            {
                res.Min.Y += bv;
                res.Max.Y += av;
            }

            av = trafo.M12 * box.Min.Z;
            bv = trafo.M12 * box.Max.Z;
            if (av < bv)
            {
                res.Min.Y += av;
                res.Max.Y += bv;
            }
            else
            {
                res.Min.Y += bv;
                res.Max.Y += av;
            }

            av = trafo.M20 * box.Min.X;
            bv = trafo.M20 * box.Max.X;
            if (av < bv)
            {
                res.Min.Z += av;
                res.Max.Z += bv;
            }
            else
            {
                res.Min.Z += bv;
                res.Max.Z += av;
            }

            av = trafo.M21 * box.Min.Y;
            bv = trafo.M21 * box.Max.Y;
            if (av < bv)
            {
                res.Min.Z += av;
                res.Max.Z += bv;
            }
            else
            {
                res.Min.Z += bv;
                res.Max.Z += av;
            }

            av = trafo.M22 * box.Min.Z;
            bv = trafo.M22 * box.Max.Z;
            if (av < bv)
            {
                res.Min.Z += av;
                res.Max.Z += bv;
            }
            else
            {
                res.Min.Z += bv;
                res.Max.Z += av;
            }

            return res;
        }

        [Test]
        public void BoxTransformTest()
        {
            var cnt = 10000;
            var rnd = new RandomSystem(1);
            var trafos = new M44d[cnt].SetByIndex(i => new M44d(rnd.CreateUniformDoubleArray(16)));
            //var trafos = new M44d[cnt].SetByIndex(i => M44d.FromBasis(rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d(), rnd.UniformV3d()));
            //var trafos = new M44d[cnt].SetByIndex(i => M44d.Translation(rnd.UniformV3d()));
            //var trafos = new M44d[cnt].SetByIndex(i => M44d.Rotation(rnd.UniformV3d()) * M44d.Translation(rnd.UniformV3d()));
            var boxes = new Box3d[cnt].SetByIndex(i => Box3d.FromCenterAndSize(rnd.UniformV3d(), rnd.UniformV3d()));
            var refBoxes = boxes.Map((b, i) => Transform1(b, trafos[i]));

            for (int j = 0; j < 10; j++)
            {
                Report.BeginTimed("Transform Boxes Aardvark");
                for (int i = 0; i < trafos.Length; i++)
                {
                    var test = boxes[i].Transformed(trafos[i]);
                    Assert.IsTrue(test.Min.ApproxEqual(refBoxes[i].Min, 1e-7) && test.Max.ApproxEqual(refBoxes[i].Max, 1e-7));
                }
                Report.End();

                Report.BeginTimed("Transform Boxes1");
                for (int i = 0; i < trafos.Length; i++)
                {
                    var test = Transform1(boxes[i], trafos[i]);
                    Assert.IsTrue(test.Min.ApproxEqual(refBoxes[i].Min, 1e-7) && test.Max.ApproxEqual(refBoxes[i].Max, 1e-7));
                }
                Report.End();


                Report.BeginTimed("Transform Boxes2");
                for (int i = 0; i < trafos.Length; i++)
                {
                    var test = Transform2(boxes[i], trafos[i]);
                    Assert.IsTrue(test.Min.ApproxEqual(refBoxes[i].Min, 1e-7) && test.Max.ApproxEqual(refBoxes[i].Max, 1e-7));
                }
                Report.End();

                Report.BeginTimed("Transform Boxes3");
                for (int i = 0; i < trafos.Length; i++)
                {
                    var test = Transform3(boxes[i], trafos[i]);
                    Assert.IsTrue(test.Min.ApproxEqual(refBoxes[i].Min, 1e-7) && test.Max.ApproxEqual(refBoxes[i].Max, 1e-7));
                }
                Report.End();

                Report.BeginTimed("Transform Boxes4");
                for (int i = 0; i < trafos.Length; i++)
                {
                    var test = Transform4(boxes[i], trafos[i]);
                    Assert.IsTrue(test.Min.ApproxEqual(refBoxes[i].Min, 1e-7) && test.Max.ApproxEqual(refBoxes[i].Max, 1e-7));
                }
                Report.End();
            }

        }
        #endregion
    }
}
