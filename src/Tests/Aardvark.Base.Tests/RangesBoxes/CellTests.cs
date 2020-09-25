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
    public class CellTests
    {
        #region creation

        [Test]
        public void UnitCell()
        {
            Assert.IsTrue(Cell.Unit.X == 0 && Cell.Unit.Y == 0 && Cell.Unit.Z == 0);
            Assert.IsTrue(Cell.Unit.Exponent == 0);
            Assert.IsTrue(Cell.Unit.BoundingBox == Box3d.Unit);
        }

        [Test]
        public void CanCreateCell()
        {
            var a = new Cell(1, 2, 3, -1);
            Assert.IsTrue(a.X == 1 && a.Y == 2 && a.Z == 3 && a.Exponent == -1);
        }

        [Test]
        public void CanCreateCell2()
        {
            var a = new Cell(1, 2, 3, -1);
            var b = new Cell(new V3l(1, 2, 3), -1);
            Assert.IsTrue(a == b);
        }

        [Test]
        public void CanCreateCell_CenteredAtOrigin()
        {
            var a = new Cell(1).BoundingBox;
            Assert.IsTrue(a.Min == new V3d(-1, -1, -1));
            Assert.IsTrue(a.Max == new V3d(+1, +1, +1));
        }

        [Test]
        public void CanCreateCell_FromBox()
        {
            var a = new Cell(new Box3d(new V3d(-1.3, 0, 0), new V3d(0.1, 1, 1)));
            Assert.IsTrue(a == new Cell(2));
        }

        [Test]
        public void CanCreateCell_FromBoxf()
        {
            var a = new Cell(new Box3f(new V3f(-1.3, 0, 0), new V3f(0.1, 1, 1)));
            Assert.IsTrue(a == new Cell(2));
        }

        [Test]
        public void CanCreateCell_FromV3ds()
        {
            var a = new Cell(new[] { new V3d(-1.3, 0, 0), new V3d(0.1, 1, 1) });
            Assert.IsTrue(a == new Cell(2));
        }

        [Test]
        public void CanCreateCell_FromV3fs()
        {
            var a = new Cell(new[] { new V3f(-1.3, 0, 0), new V3f(0.1, 1, 1) });
            Assert.IsTrue(a == new Cell(2));
        }

        [Test]
        public void CanCreateCell_FromV3dEnumerable()
        {
            var a = new Cell((IEnumerable<V3d>)new[] { new V3d(-1.3, 0, 0), new V3d(0.1, 1, 1) });
            Assert.IsTrue(a == new Cell(2));
        }

        [Test]
        public void CanCreateCell_FromV3fEnumerable()
        {
            var a = new Cell((IEnumerable<V3f>)new[] { new V3f(-1.3, 0, 0), new V3f(0.1, 1, 1) });
            Assert.IsTrue(a == new Cell(2));
        }

        [Test]
        public void CanCreateCell_FromBox2()
        {
            var a = new Cell(new Box3d(new V3d(0.1, 0.8, 0.8), new V3d(0.8, 0.9, 0.9)));
            Assert.IsTrue(a == new Cell(0, 0, 0, 0));
        }

        [Test]
        public void CanCreateCell_FromBox3()
        {
            var a = new Cell(new Box3d(new V3d(1.1, 0.8, 0.8), new V3d(1.8, 0.9, 0.9)));
            Assert.IsTrue(a == new Cell(1, 0, 0, 0));
        }

        [Test]
        public void CanCreateCell_FromPoint()
        {
            var p = new V3d(0.1, 0.2, -0.3);
            var a = new Cell(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        [Test]
        public void CanCreateCell_FromPointAtPowerOfTwoLocation()
        {
            for (var e = -1024; e <= 1023; e++)
            {
                var p = new V3d(Math.Pow(2.0, e));
                var a = new Cell(p);
                Assert.IsTrue(a.BoundingBox.Contains(p));
            }
        }

        [Test]
        public void CanCreateCell_FromPoint_Origin()
        {
            var p = V3d.Zero;
            var a = new Cell(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        [Test]
        public void CanCreateCell_FromPoint_MaxNegative()
        {
            var p = new V3d(-double.MaxValue, -double.MaxValue, -double.MaxValue);
            var a = new Cell(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        [Test]
        public void CanCreateCell_FromPoint_MaxPositive()
        {
            var p = new V3d(double.MaxValue, double.MaxValue, double.MaxValue);
            var a = new Cell(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        #endregion

        #region serialization
        
        [Test]
        public void CanSerializeCellToBinary()
        {
            var a = new Cell(1, 2, 3, -1);
            _ = a.ToByteArray();
        }

        [Test]
        public void CanDeserializeCellFromBinary()
        {
            var a = new Cell(1, 2, 3, -1);
            var buffer = a.ToByteArray();

            var b = Cell.Parse(buffer);

            Assert.IsTrue(a == b);
        }

        [Test]
        public void CanDeserializeCellFromString()
        {
            var a = new Cell(1, 2, 3, -1);
            var s = a.ToString();

            var b = Cell.Parse(s);

            Assert.IsTrue(a == b);
        }

        //[Test]
        //public void CanRoundtripCellWithSystemTextJsonAndNewtonsoft()
        //{
        //    var a = new Cell(1, 2, 3, -1);
        //    var json = System.Text.Json.JsonSerializer.Serialize(a);
        //    var b = Newtonsoft.Json.JsonConvert.DeserializeObject<Cell>(json);
        //    Assert.IsTrue(a == b);
        //}

        [Test]
        public void CanRoundtripCellWithNewtonsoft()
        {
            var a = new Cell(1, 2, 3, -1);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(a);
            var b = Newtonsoft.Json.JsonConvert.DeserializeObject<Cell>(json);
            Assert.IsTrue(a == b);
        }

        [Test]
        public void CanRoundtripCellWithNewtonsoftAndSystemTextJson()
        {
            var a = new Cell(1, 2, 3, -1);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(a);
            var b = System.Text.Json.JsonSerializer.Deserialize<Cell>(json);
            Assert.IsTrue(a == b);
        }

        #endregion

        #region CellIsCenteredAtOrigin

        [Test]
        public void CellIsCenteredAtOrigin()
        {
            var a = new Cell(10);
            Assert.IsTrue(a.IsCenteredAtOrigin);
        }

        [Test]
        public void CellIsNotCenteredAtOrigin()
        {
            var a = new Cell(1, 2, 3, 4);
            Assert.IsTrue(!a.IsCenteredAtOrigin);
        }

        #endregion

        #region equality

        [Test]
        public void CellIsEqualToItself()
        {
            var a = new Cell(1, 2, 3, -1);

            Assert.IsTrue(a.Equals((object)a));
            Assert.IsTrue(a.Equals(a));
        }

        [Test]
        public void Cell_Equality()
        {
            var a = new Cell(1, 2, 3, -1);
            var b = new Cell(1, 2, 3, -1);

            Assert.IsTrue(a.Equals((object)b));
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a == b);
        }

        [Test]
        public void Cell_Inequality()
        {
            Assert.IsTrue(new Cell(0, 0, 0, 0) != new Cell(1, 0, 0, 0));
            Assert.IsTrue(new Cell(0, 0, 0, 0) != new Cell(0, 1, 0, 0));
            Assert.IsTrue(new Cell(0, 0, 0, 0) != new Cell(0, 0, 1, 0));
            Assert.IsTrue(new Cell(0, 0, 0, 0) != new Cell(0, 0, 0, 1));
        }

        [Test]
        public void Cell_GetHashCode()
        {
            var a = new Cell(1, 0, 0, 0).GetHashCode();
            var b = new Cell(0, 1, 0, 0).GetHashCode();
            var c = new Cell(0, 0, 1, 0).GetHashCode();
            var d = new Cell(0, 0, 0, 1).GetHashCode();

            Assert.IsTrue(a != b && a != c && a != d && b != c && b != d && c != d);
        }

        #endregion

        #region ToString

        [Test]
        public void Cell_ToString()
        {
            var s = new Cell(1, 2, 3, -1).ToString();
            Assert.IsTrue(s == "[1, 2, 3, -1]");
        }

        #endregion

        #region BoundingBox

        [Test]
        public void Cell_Box3d()
        {
            Assert.IsTrue(new Cell(0, 0, 0, 0).BoundingBox == new Box3d(new V3d(0, 0, 0), new V3d(1, 1, 1)));
            Assert.IsTrue(new Cell(0, 0, 0, 1).BoundingBox == new Box3d(new V3d(0, 0, 0), new V3d(2, 2, 2)));
            Assert.IsTrue(new Cell(1, 2, 3, 4).BoundingBox == new Box3d(new V3d(16, 32, 48), new V3d(32, 48, 64)));
            Assert.IsTrue(new Cell(0, 0, 0, -1).BoundingBox == new Box3d(new V3d(0, 0, 0), new V3d(0.5, 0.5, 0.5)));
            Assert.IsTrue(new Cell(-1, 0, 0, -1).BoundingBox == new Box3d(new V3d(-0.5, 0, 0), new V3d(0.0, 0.5, 0.5)));
        }

        #endregion

        #region children

        [Test]
        public void Cell_Centered_Children()
        {
            var xs = new Cell(1).Children;
            Assert.IsTrue(xs[0] == new Cell(-1, -1, -1, 0));
            Assert.IsTrue(xs[1] == new Cell( 0, -1, -1, 0));
            Assert.IsTrue(xs[2] == new Cell(-1,  0, -1, 0));
            Assert.IsTrue(xs[3] == new Cell( 0,  0, -1, 0));
            Assert.IsTrue(xs[4] == new Cell(-1, -1,  0, 0));
            Assert.IsTrue(xs[5] == new Cell( 0, -1,  0, 0));
            Assert.IsTrue(xs[6] == new Cell(-1,  0,  0, 0));
            Assert.IsTrue(xs[7] == new Cell( 0,  0,  0, 0));
        }

        [Test]
        public void Cell_Children1()
        {
            var xs = new Cell(0, 0, 0, 0).Children;
            Assert.IsTrue(xs[0] == new Cell(0, 0, 0, -1));
            Assert.IsTrue(xs[1] == new Cell(1, 0, 0, -1));
            Assert.IsTrue(xs[2] == new Cell(0, 1, 0, -1));
            Assert.IsTrue(xs[3] == new Cell(1, 1, 0, -1));
            Assert.IsTrue(xs[4] == new Cell(0, 0, 1, -1));
            Assert.IsTrue(xs[5] == new Cell(1, 0, 1, -1));
            Assert.IsTrue(xs[6] == new Cell(0, 1, 1, -1));
            Assert.IsTrue(xs[7] == new Cell(1, 1, 1, -1));
        }

        [Test]
        public void Cell_Children2()
        {
            var xs = new Cell(-2, 0, 10, 1).Children;
            Assert.IsTrue(xs[0] == new Cell(-4, 0, 20, 0));
            Assert.IsTrue(xs[1] == new Cell(-3, 0, 20, 0));
            Assert.IsTrue(xs[2] == new Cell(-4, 1, 20, 0));
            Assert.IsTrue(xs[3] == new Cell(-3, 1, 20, 0));
            Assert.IsTrue(xs[4] == new Cell(-4, 0, 21, 0));
            Assert.IsTrue(xs[5] == new Cell(-3, 0, 21, 0));
            Assert.IsTrue(xs[6] == new Cell(-4, 1, 21, 0));
            Assert.IsTrue(xs[7] == new Cell(-3, 1, 21, 0));
        }

        #endregion

        #region parent

        [Test]
        public void Cell_Centered_Parent()
        {
            Assert.IsTrue(new Cell(-1).Parent == new Cell(0));
            Assert.IsTrue(new Cell(0).Parent == new Cell(1));
            Assert.IsTrue(new Cell(1).Parent == new Cell(2));
        }

        [Test]
        public void Cell_Parent1()
        {
            Assert.IsTrue(new Cell(0, 0, 0, 0).Parent == new Cell(0, 0, 0, 1));
            Assert.IsTrue(new Cell(1, 0, 0, 0).Parent == new Cell(0, 0, 0, 1));
            Assert.IsTrue(new Cell(0, 1, 0, 0).Parent == new Cell(0, 0, 0, 1));
            Assert.IsTrue(new Cell(1, 1, 0, 0).Parent == new Cell(0, 0, 0, 1));
            Assert.IsTrue(new Cell(0, 0, 1, 0).Parent == new Cell(0, 0, 0, 1));
            Assert.IsTrue(new Cell(1, 0, 1, 0).Parent == new Cell(0, 0, 0, 1));
            Assert.IsTrue(new Cell(0, 1, 1, 0).Parent == new Cell(0, 0, 0, 1));
            Assert.IsTrue(new Cell(1, 1, 1, 0).Parent == new Cell(0, 0, 0, 1));
        }

        [Test]
        public void Cell_Parent2()
        {
            Assert.IsTrue(new Cell(-1, 0, 0, 0).Parent == new Cell(-1, 0, 0, 1));
            Assert.IsTrue(new Cell(-2, 0, 0, 0).Parent == new Cell(-1, 0, 0, 1));
            Assert.IsTrue(new Cell(-3, 0, 0, 0).Parent == new Cell(-2, 0, 0, 1));
            Assert.IsTrue(new Cell(-4, 0, 0, 0).Parent == new Cell(-2, 0, 0, 1));
        }

        #endregion

        #region touches origin

        [Test]
        public void CenteredCellDoesNotTouchOrigin()
        {
            Assert.IsTrue(new Cell(-1).TouchesOrigin == false);
            Assert.IsTrue(new Cell(0).TouchesOrigin == false);
            Assert.IsTrue(new Cell(1).TouchesOrigin == false);
        }

        [Test]
        public void CellTouchOrigin()
        {
            Assert.IsTrue(new Cell( 0,  0,  0, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell(-1,  0,  0, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell( 0, -1,  0, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell(-1, -1,  0, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell( 0,  0, -1, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell(-1,  0, -1, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell( 0, -1, -1, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell(-1, -1, -1, 0).TouchesOrigin == true);

            Assert.IsTrue(new Cell(1, 0, 0, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell(0, 1, 0, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell(1, 1, 0, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell(0, 0, 1, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell(1, 0, 1, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell(0, 1, 1, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell(1, 1, 1, 0).TouchesOrigin == false);
            
        }

        #endregion

        #region contains/intersects

        [Test]
        public void Cell_InsideOutside_ContainsEqual()
        {
            var a = new Cell(1, 2, 3, 4);
            var b = new Cell(1, 2, 3, 4);
            Assert.IsTrue(a.Contains(b));
            Assert.IsTrue(b.Contains(a));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Inside()
        {
            var a = new Cell(0, 0, 0, 0);
            var b = new Cell(1, 2, 3, -4);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_InsideTouching()
        {
            var a = new Cell(0, 0, 0, 0);
            var b = new Cell(0, 0, 0, -1);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Outside()
        {
            var a = new Cell(0, 0, 0, 0);
            var b = new Cell(2, 0, 0, 0);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_OutsideTouching()
        {
            var a = new Cell(0, 0, 0, 0);
            var b = new Cell(1, 0, 0, 0);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered1()
        {
            var a = new Cell(1);
            Assert.IsTrue(a.Contains(new Cell(-1, -1, -1, 0)));
            Assert.IsTrue(a.Contains(new Cell(0, -1, -1, 0)));
            Assert.IsTrue(a.Contains(new Cell(-1, 0, -1, 0)));
            Assert.IsTrue(a.Contains(new Cell(0, 0, -1, 0)));
            Assert.IsTrue(a.Contains(new Cell(-1, -1, 0, 0)));
            Assert.IsTrue(a.Contains(new Cell(0, -1, 0, 0)));
            Assert.IsTrue(a.Contains(new Cell(-1, 0, 0, 0)));
            Assert.IsTrue(a.Contains(new Cell(0, 0, 0, 0)));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered2()
        {
            var a = new Cell(1);
            var b = new Cell(0);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered3()
        {
            var a = new Cell(1);
            var b = new Cell(0);
            Assert.IsTrue(!b.Contains(a));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered4()
        {
            var a = new Cell(2);
            var b = new Cell(2);
            Assert.IsTrue(a.Contains(b));
            Assert.IsTrue(b.Contains(a));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered5()
        {
            var a = new Cell(2);
            var b = new Cell(0, 0, 0, 2);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered6()
        {
            var a = new Cell(2);
            var b = new Cell(10, 10, 10, 1);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered7()
        {
            var a = new Cell(2);
            var b = new Cell(0, 0, 0, 3);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered8()
        {
            var a = new Cell(0, 0, 0, 2);
            var b = new Cell(1);
            Assert.IsTrue(!a.Contains(b));
        }
        
        [Test]
        public void Cell_InsideOutside_Intersects_Itself_1()
        {
            var a = Cell.Unit;
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_2()
        {
            var a = new Cell(1, -2, 3, -4);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_3()
        {
            var a = new Cell(1, -2, 12345678910111213, 2);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_4()
        {
            var a = new Cell(7);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_5()
        {
            var a = new Cell(-123456789);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Contained_NotTouching()
        {
            var a = new Cell(0, 0, 0, 2);
            var b = new Cell(1, 2, 1, 0);
            Assert.IsTrue(a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_InsideTouching()
        {
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(0, 0, 0, -1)));
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(1, 0, 0, -1)));
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(0, 1, 0, -1)));
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(1, 1, 0, -1)));
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(0, 0, 1, -1)));
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(1, 0, 1, -1)));
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(0, 1, 1, -1)));
            Assert.IsTrue(new Cell(0, 0, 0, 0).Intersects(new Cell(1, 1, 1, -1)));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_OutsideTouchingMin()
        {
            var a = new Cell(2, 0, 0, 0);
            var b = new Cell(1, 0, 0, 0);
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_OutsideTouchingMax()
        {
            var a = new Cell(1, 0, 0, 0);
            var b = new Cell(2, 0, 0, 0);
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_ExponentDeltaGreaterThan63()
        {
            // {[0, 0, 0, 3]}
            // {[4408771833616582656, 3234636573324969984, 4552195268789674496, -62]}
            var a = new Cell(0, 0, 0, 3);
            var b = new Cell(4408771833616582656, 3234636573324969984, 4552195268789674496, -62);
            Assert.IsTrue(a.Contains(b));
        }
        
        [Test]
        public void Cell_InsideOutside_Contains_ExponentLessThanMinus64()
        {
            var a = new Cell(12345678910111213, 1000, 2000, -100);
            var b= new Cell(12345678910111213*2+1, 2000, 4000, -101);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_ExponentLessThanMinus64_1()
        {
            var a = new Cell(12345678910111213, 1000, 2000, -100);
            var b = new Cell(12345678910111213 + 1, 2000, 4000, -100);
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_ExponentLessThanMinus64_2()
        {
            var a = new Cell(12345678910111213, 1000, 2000, -100);
            var b = new Cell(12345678910111213, 2000, 4000, -100);
            Assert.IsTrue(!a.Intersects(b));
        }

        #endregion

        #region octant
        
        [Test]
        public void Cell_GetOctant()
        {
            var c = new Cell(-2, 0, 10, 1);
            var xs = new[] { c.GetOctant(0), c.GetOctant(1), c.GetOctant(2), c.GetOctant(3), c.GetOctant(4), c.GetOctant(5), c.GetOctant(6), c.GetOctant(7) };
            Assert.IsTrue(xs[0] == new Cell(-4, 0, 20, 0));
            Assert.IsTrue(xs[1] == new Cell(-3, 0, 20, 0));
            Assert.IsTrue(xs[2] == new Cell(-4, 1, 20, 0));
            Assert.IsTrue(xs[3] == new Cell(-3, 1, 20, 0));
            Assert.IsTrue(xs[4] == new Cell(-4, 0, 21, 0));
            Assert.IsTrue(xs[5] == new Cell(-3, 0, 21, 0));
            Assert.IsTrue(xs[6] == new Cell(-4, 1, 21, 0));
            Assert.IsTrue(xs[7] == new Cell(-3, 1, 21, 0));
        }

        [Test]
        public void Cell_GetOctantOfCell_TooBig()
        {
            var c = new Cell(0, 0, 0, 1);

            Assert.IsTrue(c.GetOctant(new Cell(0, 0, 0, 1)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 0, 0, 2)) == null);
        }

        [Test]
        public void Cell_GetOctantOfCell_Centered()
        {
            var c = new Cell(1);

            Assert.IsTrue(c.GetOctant(new Cell(0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(1)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(2)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 0, 0, 1)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 0, 0, 2)) == null);

            Assert.IsTrue(c.GetOctant(new Cell(-1, -1, -1, 0)) == 0);
            Assert.IsTrue(c.GetOctant(new Cell( 0, -1, -1, 0)) == 1);
            Assert.IsTrue(c.GetOctant(new Cell(-1,  0, -1, 0)) == 2);
            Assert.IsTrue(c.GetOctant(new Cell( 0,  0, -1, 0)) == 3);
            Assert.IsTrue(c.GetOctant(new Cell(-1, -1,  0, 0)) == 4);
            Assert.IsTrue(c.GetOctant(new Cell( 0, -1,  0, 0)) == 5);
            Assert.IsTrue(c.GetOctant(new Cell(-1,  0,  0, 0)) == 6);
            Assert.IsTrue(c.GetOctant(new Cell( 0,  0,  0, 0)) == 7);

            Assert.IsTrue(c.GetOctant(new Cell(-2, -1, -1, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell( 1, -1, -1, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(-1, -2, -1, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(-1,  1, -1, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(-1, -1, -2, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(-1, -1,  1, 0)) == null);
        }

        [Test]
        public void Cell_GetOctantOfCell_1()
        {
            var c = new Cell(0, 0, 0, 1);

            Assert.IsTrue(c.GetOctant(new Cell(0, 0, 0, 0)) == 0);
            Assert.IsTrue(c.GetOctant(new Cell(1, 0, 0, 0)) == 1);
            Assert.IsTrue(c.GetOctant(new Cell(0, 1, 0, 0)) == 2);
            Assert.IsTrue(c.GetOctant(new Cell(0, 0, 1, 0)) == 4);
        }

        [Test]
        public void Cell_GetOctantOfCell_1_Outside()
        {
            var c = new Cell(0, 0, 0, 1);

            Assert.IsTrue(c.GetOctant(new Cell(-1, 0, 0, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, -1, 0, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 0, -1, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(2, 0, 0, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 2, 0, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 0, 2, 0)) == null);
        }


        [Test]
        public void Cell_GetOctantOfCell_2()
        {
            var c = new Cell(0, 1, 0, 1);

            Assert.IsTrue(c.GetOctant(new Cell(0, 2, 0, 0)) == 0);
            Assert.IsTrue(c.GetOctant(new Cell(1, 2, 0, 0)) == 1);
            Assert.IsTrue(c.GetOctant(new Cell(0, 3, 0, 0)) == 2);
            Assert.IsTrue(c.GetOctant(new Cell(0, 2, 1, 0)) == 4);
        }

        [Test]
        public void Cell_GetOctantOfCell_2_Outside()
        {
            var c = new Cell(0, 1, 0, 1);

            Assert.IsTrue(c.GetOctant(new Cell(-1, 2, 0, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(1, 1, 0, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 4, 0, 0)) == null);
            Assert.IsTrue(c.GetOctant(new Cell(0, 2, 2, 0)) == null);
        }

        #endregion

        #region operators

        #region operator >>

        [Test]
        public void Cell_RightShift_0()
        {
            var c = new Cell(1, -2, 3, 4) >> 0;
            Assert.IsTrue(c == new Cell(1, -2, 3, 4));
            Assert.IsTrue(new Cell(1, -2, 3, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_1()
        {
            var c = new Cell(1, -2, 3, 4) >> 1;
            Assert.IsTrue(c == new Cell(2, -4, 6, 3));
            Assert.IsTrue(new Cell(1, -2, 3, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_2()
        {
            var c = new Cell(1, -2, 3, 4) >> 2;
            Assert.IsTrue(c == new Cell(4, -8, 12, 2));
            Assert.IsTrue(new Cell(1, -2, 3, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_m1()
        {
            var c = new Cell(1, -2, 3, 4) >> -1;
            Assert.IsTrue(c == new Cell(1, -2, 3, 4));
            Assert.IsTrue(new Cell(1, -2, 3, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_c0()
        {
            var c = new Cell(4) >> 0;
            Assert.IsTrue(c == new Cell(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_RightShift_c1()
        {
            var c = new Cell(4) >> 1;
            Assert.IsTrue(c == new Cell(3));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_RightShift_cm1()
        {
            var c = new Cell(4) >> -1;
            Assert.IsTrue(c == new Cell(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        #endregion

        #region operator <<

        [Test]
        public void Cell_LeftShift_0()
        {
            var c = new Cell(1, -2, 3, 4) << 0;
            Assert.IsTrue(c == new Cell(1, -2, 3, 4));
            Assert.IsTrue(new Cell(1, -2, 3, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_1()
        {
            var c = new Cell(1, -2, 3, 4) << 1;
            Assert.IsTrue(c == new Cell(0, -1, 1, 5));
            Assert.IsTrue(new Cell(0, -2, 2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_2()
        {
            var c = new Cell(1, -2, 3, 4) << 2;
            Assert.IsTrue(c == new Cell(0, -1, 0, 6));
            Assert.IsTrue(new Cell(0, -4, 0, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_m1()
        {
            var c = new Cell(1, -2, 3, 4) << -1;
            Assert.IsTrue(c == new Cell(1, -2, 3, 4));
            Assert.IsTrue(new Cell(1, -2, 3, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_c0()
        {
            var c = new Cell(4) << 0;
            Assert.IsTrue(c == new Cell(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_LeftShift_c1()
        {
            var c = new Cell(4) << 1;
            Assert.IsTrue(c == new Cell(5));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_LeftShift_cm1()
        {
            var c = new Cell(4) << -1;
            Assert.IsTrue(c == new Cell(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        #endregion

        #endregion
    }
}
