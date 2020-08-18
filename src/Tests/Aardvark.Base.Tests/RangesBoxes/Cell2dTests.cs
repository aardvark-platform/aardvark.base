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
    public class Cell2dTests
    {
        #region creation

        [Test]
        public void UnitCell()
        {
            Assert.IsTrue(Cell2d.Unit.X == 0 && Cell2d.Unit.Y == 0);
            Assert.IsTrue(Cell2d.Unit.Exponent == 0);
            Assert.IsTrue(Cell2d.Unit.BoundingBox == Box2d.Unit);
        }

        [Test]
        public void CanCreateCell()
        {
            var a = new Cell2d(1, 2, -1);
            Assert.IsTrue(a.X == 1 && a.Y == 2 && a.Exponent == -1);
        }

        [Test]
        public void CanCreateCell2()
        {
            var a = new Cell2d(1, 2, -1);
            var b = new Cell2d(new V2l(1, 2), -1);
            Assert.IsTrue(a == b);
        }

        [Test]
        public void CanCreateCell_CenteredAtOrigin()
        {
            var a = new Cell2d(1).BoundingBox;
            Assert.IsTrue(a.Min == new V2d(-1, -1));
            Assert.IsTrue(a.Max == new V2d(+1, +1));
        }

        [Test]
        public void CanCreateCell_FromBox()
        {
            var a = new Cell2d(new Box2d(new V2d(-1.3, 0), new V2d(0.1, 1)));
            Assert.IsTrue(a == new Cell2d(2));
        }

        [Test]
        public void CanCreateCell_FromBoxf()
        {
            var a = new Cell2d(new Box2f(new V2f(-1.3, 0), new V2f(0.1, 1)));
            Assert.IsTrue(a == new Cell2d(2));
        }

        [Test]
        public void CanCreateCell_FromV2ds()
        {
            var a = new Cell2d(new[] { new V2d(-1.3, 0), new V2d(0.1, 1) });
            Assert.IsTrue(a == new Cell2d(2));
        }

        [Test]
        public void CanCreateCell_FromV2fs()
        {
            var a = new Cell2d(new[] { new V2f(-1.3, 0), new V2f(0.1, 1) });
            Assert.IsTrue(a == new Cell2d(2));
        }

        [Test]
        public void CanCreateCell_FromV2dEnumerable()
        {
            var a = new Cell2d((IEnumerable<V2d>)new[] { new V2d(-1.3, 0), new V2d(0.1, 1) });
            Assert.IsTrue(a == new Cell2d(2));
        }

        [Test]
        public void CanCreateCell_FromV2fEnumerable()
        {
            var a = new Cell2d((IEnumerable<V2f>)new[] { new V2f(-1.3, 0), new V2f(0.1, 1) });
            Assert.IsTrue(a == new Cell2d(2));
        }

        [Test]
        public void CanCreateCell_FromBox2()
        {
            var a = new Cell2d(new Box2d(new V2d(0.1, 0.8), new V2d(0.8, 0.9)));
            Assert.IsTrue(a == new Cell2d(0, 0, 0));
        }

        [Test]
        public void CanCreateCell_FromBox3()
        {
            var a = new Cell2d(new Box2d(new V2d(1.1, 0.8), new V2d(1.8, 0.9)));
            Assert.IsTrue(a == new Cell2d(1, 0, 0));
        }

        [Test]
        public void CanCreateCell_FromPoint()
        {
            var p = new V2d(0.1, 0.2);
            var a = new Cell2d(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        [Test]
        public void CanCreateCell_FromPointAtPowerOfTwoLocation()
        {
            for (var e = -1024; e <= 1023; e++)
            {
                var p = new V2d(Math.Pow(2.0, e));
                var a = new Cell2d(p);
                Assert.IsTrue(a.BoundingBox.Contains(p));
            }
        }

        [Test]
        public void CanCreateCell_FromPoint_Origin()
        {
            var p = V2d.Zero;
            var a = new Cell2d(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        [Test]
        public void CanCreateCell_FromPoint_MaxNegative()
        {
            var p = new V2d(-double.MaxValue, -double.MaxValue);
            var a = new Cell2d(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        [Test]
        public void CanCreateCell_FromPoint_MaxPositive()
        {
            var p = new V2d(double.MaxValue, double.MaxValue);
            var a = new Cell2d(p);
            Assert.IsTrue(a.BoundingBox.Contains(p));
        }

        #endregion

        #region serialization
        
        [Test]
        public void CanSerializeCellToBinary()
        {
            var a = new Cell2d(1, 2, -1);
            _ = a.ToByteArray();
        }

        [Test]
        public void CanDeserializeCellFromBinary()
        {
            var a = new Cell2d(1, 2, -1);
            var buffer = a.ToByteArray();

            var b = Cell2d.Parse(buffer);

            Assert.IsTrue(a == b);
        }

        #endregion

        #region CellIsCenteredAtOrigin

        [Test]
        public void CellIsCenteredAtOrigin()
        {
            var a = new Cell2d(10);
            Assert.IsTrue(a.IsCenteredAtOrigin);
        }

        [Test]
        public void CellIsNotCenteredAtOrigin()
        {
            var a = new Cell2d(1, 2, 4);
            Assert.IsTrue(!a.IsCenteredAtOrigin);
        }

        #endregion

        #region equality

        [Test]
        public void CellIsEqualToItself()
        {
            var a = new Cell2d(1, 2, -1);

            Assert.IsTrue(a.Equals((object)a));
            Assert.IsTrue(a.Equals(a));
        }

        [Test]
        public void Cell_Equality()
        {
            var a = new Cell2d(1, 2, -1);
            var b = new Cell2d(1, 2, -1);

            Assert.IsTrue(a.Equals((object)b));
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a == b);
        }

        [Test]
        public void Cell_Inequality()
        {
            Assert.IsTrue(new Cell2d(0, 0, 0) != new Cell2d(1, 0, 0));
            Assert.IsTrue(new Cell2d(0, 0, 0) != new Cell2d(0, 1, 0));
            Assert.IsTrue(new Cell2d(0, 0, 0) != new Cell2d(0, 0, 1));
        }

        [Test]
        public void Cell_GetHashCode()
        {
            var a = new Cell2d(1, 0, 0).GetHashCode();
            var b = new Cell2d(0, 1, 0).GetHashCode();
            var c = new Cell2d(0, 0, 0).GetHashCode();
            var d = new Cell2d(0, 0, 1).GetHashCode();

            Assert.IsTrue(a != b && a != c && a != d && b != c && b != d && c != d);
        }

        #endregion

        #region ToString

        [Test]
        public void Cell_ToString()
        {
            var s = new Cell2d(1, 2, -1).ToString();
            Assert.IsTrue(s == "[1, 2, -1]");
        }

        #endregion

        #region BoundingBox

        [Test]
        public void Cell_Box2d()
        {
            Assert.IsTrue(new Cell2d(0, 0, 0).BoundingBox   == new Box2d(new V2d(0, 0),    new V2d(1, 1)));
            Assert.IsTrue(new Cell2d(0, 0, 1).BoundingBox   == new Box2d(new V2d(0, 0),    new V2d(2, 2)));
            Assert.IsTrue(new Cell2d(1, 2, 4).BoundingBox   == new Box2d(new V2d(16, 32),  new V2d(32, 48)));
            Assert.IsTrue(new Cell2d(0, 0, -1).BoundingBox  == new Box2d(new V2d(0, 0),    new V2d(0.5, 0.5)));
            Assert.IsTrue(new Cell2d(-1, 0, -1).BoundingBox == new Box2d(new V2d(-0.5, 0), new V2d(0.0, 0.5)));
        }

        #endregion

        #region children

        [Test]
        public void Cell_Centered_Children()
        {
            var xs = new Cell2d(1).Children;
            Assert.IsTrue(xs[0] == new Cell2d(-1, -1, 0));
            Assert.IsTrue(xs[1] == new Cell2d( 0, -1, 0));
            Assert.IsTrue(xs[2] == new Cell2d(-1,  0, 0));
            Assert.IsTrue(xs[3] == new Cell2d( 0,  0, 0));
        }

        [Test]
        public void Cell_Children1()
        {
            var xs = new Cell2d(0, 0, 0).Children;
            Assert.IsTrue(xs[0] == new Cell2d(0, 0, -1));
            Assert.IsTrue(xs[1] == new Cell2d(1, 0, -1));
            Assert.IsTrue(xs[2] == new Cell2d(0, 1, -1));
            Assert.IsTrue(xs[3] == new Cell2d(1, 1, -1));
        }

        [Test]
        public void Cell_Children2()
        {
            var xs = new Cell2d(-2, 0, 1).Children;
            Assert.IsTrue(xs[0] == new Cell2d(-4, 0, 0));
            Assert.IsTrue(xs[1] == new Cell2d(-3, 0, 0));
            Assert.IsTrue(xs[2] == new Cell2d(-4, 1, 0));
            Assert.IsTrue(xs[3] == new Cell2d(-3, 1, 0));
        }

        #endregion

        #region parent

        [Test]
        public void Cell_Centered_Parent()
        {
            Assert.IsTrue(new Cell2d(-1).Parent == new Cell2d(0));
            Assert.IsTrue(new Cell2d(0).Parent  == new Cell2d(1));
            Assert.IsTrue(new Cell2d(1).Parent  == new Cell2d(2));
        }

        [Test]
        public void Cell_Parent1()
        {
            Assert.IsTrue(new Cell2d(0, 0, 0).Parent == new Cell2d(0, 0, 1));
            Assert.IsTrue(new Cell2d(1, 0, 0).Parent == new Cell2d(0, 0, 1));
            Assert.IsTrue(new Cell2d(0, 1, 0).Parent == new Cell2d(0, 0, 1));
            Assert.IsTrue(new Cell2d(1, 1, 0).Parent == new Cell2d(0, 0, 1));
        }

        [Test]
        public void Cell_Parent2()
        {
            Assert.IsTrue(new Cell2d(-1, 0, 0).Parent == new Cell2d(-1, 0, 1));
            Assert.IsTrue(new Cell2d(-2, 0, 0).Parent == new Cell2d(-1, 0, 1));
            Assert.IsTrue(new Cell2d(-3, 0, 0).Parent == new Cell2d(-2, 0, 1));
            Assert.IsTrue(new Cell2d(-4, 0, 0).Parent == new Cell2d(-2, 0, 1));
        }

        #endregion

        #region touches origin

        [Test]
        public void CenteredCellDoesNotTouchOrigin()
        {
            Assert.IsTrue(new Cell2d(-1).TouchesOrigin == false);
            Assert.IsTrue(new Cell2d(0).TouchesOrigin == false);
            Assert.IsTrue(new Cell2d(1).TouchesOrigin == false);
        }

        [Test]
        public void CellTouchOrigin()
        {
            Assert.IsTrue(new Cell2d( 0,  0, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell2d(-1,  0, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell2d( 0, -1, 0).TouchesOrigin == true);
            Assert.IsTrue(new Cell2d(-1, -1, 0).TouchesOrigin == true);

            Assert.IsTrue(new Cell2d(1, 0, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell2d(0, 1, 0).TouchesOrigin == false);
            Assert.IsTrue(new Cell2d(1, 1, 0).TouchesOrigin == false);
        }

        #endregion

        #region contains/intersects

        [Test]
        public void Cell_InsideOutside_ContainsEqual()
        {
            var a = new Cell2d(1, 2, 4);
            var b = new Cell2d(1, 2, 4);
            Assert.IsTrue(a.Contains(b));
            Assert.IsTrue(b.Contains(a));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Inside()
        {
            var a = new Cell2d(0, 0, 0);
            var b = new Cell2d(1, 2,-4);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_InsideTouching()
        {
            var a = new Cell2d(0, 0, 0);
            var b = new Cell2d(0, 0, -1);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Outside()
        {
            var a = new Cell2d(0, 0, 0);
            var b = new Cell2d(2, 0, 0);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_OutsideTouching()
        {
            var a = new Cell2d(0, 0, 0);
            var b = new Cell2d(1, 0, 0);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered1()
        {
            var a = new Cell2d(1);
            Assert.IsTrue(a.Contains(new Cell2d(-1, -1, 0)));
            Assert.IsTrue(a.Contains(new Cell2d(0, -1, 0)));
            Assert.IsTrue(a.Contains(new Cell2d(-1, 0, 0)));
            Assert.IsTrue(a.Contains(new Cell2d(0, 0, 0)));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered2()
        {
            var a = new Cell2d(1);
            var b = new Cell2d(0);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered3()
        {
            var a = new Cell2d(1);
            var b = new Cell2d(0);
            Assert.IsTrue(!b.Contains(a));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered4()
        {
            var a = new Cell2d(2);
            var b = new Cell2d(2);
            Assert.IsTrue(a.Contains(b));
            Assert.IsTrue(b.Contains(a));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered5()
        {
            var a = new Cell2d(2);
            var b = new Cell2d(0, 0, 2);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered6()
        {
            var a = new Cell2d(2);
            var b = new Cell2d(10, 10, 1);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered7()
        {
            var a = new Cell2d(2);
            var b = new Cell2d(0, 0, 3);
            Assert.IsTrue(!a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_Centered8()
        {
            var a = new Cell2d(0, 0, 2);
            var b = new Cell2d(1);
            Assert.IsTrue(!a.Contains(b));
        }
        
        [Test]
        public void Cell_InsideOutside_Intersects_Itself_1()
        {
            var a = Cell2d.Unit;
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_2()
        {
            var a = new Cell2d(1, -2, -4);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_3()
        {
            var a = new Cell2d(1, 12345678910111213, 2);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_4()
        {
            var a = new Cell2d(7);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Itself_5()
        {
            var a = new Cell2d(-123456789);
            Assert.IsTrue(a.Intersects(a));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_Contained_NotTouching()
        {
            var a = new Cell2d(0, 0, 2);
            var b = new Cell2d(1, 2, 0);
            Assert.IsTrue(a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_InsideTouching()
        {
            Assert.IsTrue(new Cell2d(0, 0, 0).Intersects(new Cell2d(0, 0, -1)));
            Assert.IsTrue(new Cell2d(0, 0, 0).Intersects(new Cell2d(1, 0, -1)));
            Assert.IsTrue(new Cell2d(0, 0, 0).Intersects(new Cell2d(0, 1, -1)));
            Assert.IsTrue(new Cell2d(0, 0, 0).Intersects(new Cell2d(1, 1, -1)));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_OutsideTouchingMin()
        {
            var a = new Cell2d(2, 0, 0);
            var b = new Cell2d(1, 0, 0);
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_OutsideTouchingMax()
        {
            var a = new Cell2d(1, 0, 0);
            var b = new Cell2d(2, 0, 0);
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Contains_ExponentDeltaGreaterThan63()
        {
            // {[0, 0, 0, 3]}
            // {[4408771833616582656, 3234636573324969984, 4552195268789674496, -62]}
            var a = new Cell2d(0, 0, 3);
            var b = new Cell2d(4408771833616582656, 3234636573324969984, -62);
            Assert.IsTrue(a.Contains(b));
        }
        
        [Test]
        public void Cell_InsideOutside_Contains_ExponentLessThanMinus64()
        {
            var a = new Cell2d(12345678910111213, 1000, -100);
            var b= new Cell2d(12345678910111213*2+1, 2000, -101);
            Assert.IsTrue(a.Contains(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_ExponentLessThanMinus64_1()
        {
            var a = new Cell2d(12345678910111213, 1000, -100);
            var b = new Cell2d(12345678910111213 + 1, 2000, -100);
            Assert.IsTrue(!a.Intersects(b));
        }

        [Test]
        public void Cell_InsideOutside_Intersects_ExponentLessThanMinus64_2()
        {
            var a = new Cell2d(12345678910111213, 1000, -100);
            var b = new Cell2d(12345678910111213, 2000, -100);
            Assert.IsTrue(!a.Intersects(b));
        }

        #endregion

        #region quadrant
        
        [Test]
        public void Cell_GetQuadrant()
        {
            var c = new Cell2d(-2, 0, 1);
            var xs = new[] { c.GetQuadrant(0), c.GetQuadrant(1), c.GetQuadrant(2), c.GetQuadrant(3) };
            Assert.IsTrue(xs[0] == new Cell2d(-4, 0, 0));
            Assert.IsTrue(xs[1] == new Cell2d(-3, 0, 0));
            Assert.IsTrue(xs[2] == new Cell2d(-4, 1, 0));
            Assert.IsTrue(xs[3] == new Cell2d(-3, 1, 0));
        }

        [Test]
        public void Cell_GetQuadrantOfCell_TooBig()
        {
            var c = new Cell2d(0, 0, 1);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 0, 1)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 0, 2)) == null);
        }

        [Test]
        public void Cell_GetQuadrantOfCell_Centered()
        {
            var c = new Cell2d(1);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(1)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(2)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 0, 1)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 0, 2)) == null);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(-1, -1, 0)) == 0);
            _ = c.GetQuadrant(new Cell2d(0, -1, 0));
            Assert.IsTrue(c.GetQuadrant(new Cell2d( 0, -1, 0)) == 1);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(-1,  0, 0)) == 2);
            Assert.IsTrue(c.GetQuadrant(new Cell2d( 0,  0, 0)) == 3);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(-2, -1, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d( 1, -1, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(-1, -2, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(-1,  1, 0)) == null);
        }

        [Test]
        public void Cell_GetQuadrantOfCell_1()
        {
            var c = new Cell2d(0, 0, 1);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 0, 0)) == 0);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(1, 0, 0)) == 1);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 1, 0)) == 2);
        }

        [Test]
        public void Cell_GetQuadrantOfCell_1_Outside()
        {
            var c = new Cell2d(0, 0, 1);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(-1, 0, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, -1, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(2, 0, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 2, 0)) == null);
        }


        [Test]
        public void Cell_GetQuadrantOfCell_2()
        {
            var c = new Cell2d(0, 1, 1);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 2, 0)) == 0);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(1, 2, 0)) == 1);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 3, 0)) == 2);
        }

        [Test]
        public void Cell_GetQuadrantOfCell_2_Outside()
        {
            var c = new Cell2d(0, 1, 1);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(-1, 2, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(1, 1, 0)) == null);
            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 4, 0)) == null);
        }

        [Test]
        public void Cell_GetQuadrantOfCell_3()
        {
            var c = new Cell2d(0, 1, 2);

            Assert.IsTrue(c.GetQuadrant(new Cell2d(0, 6, 0)) == 2);
        }

        #endregion

        #region operators

        #region operator >>

        [Test]
        public void Cell_RightShift_0()
        {
            var c = new Cell2d(1, -2, 4) >> 0;
            Assert.IsTrue(c == new Cell2d(1, -2, 4));
            Assert.IsTrue(new Cell2d(1, -2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_1()
        {
            var c = new Cell2d(1, -2, 4) >> 1;
            Assert.IsTrue(c == new Cell2d(2, -4, 3));
            Assert.IsTrue(new Cell2d(1, -2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_2()
        {
            var c = new Cell2d(1, -2, 4) >> 2;
            Assert.IsTrue(c == new Cell2d(4, -8, 2));
            Assert.IsTrue(new Cell2d(1, -2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_m1()
        {
            var c = new Cell2d(1, -2, 4) >> -1;
            Assert.IsTrue(c == new Cell2d(1, -2, 4));
            Assert.IsTrue(new Cell2d(1, -2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_RightShift_c0()
        {
            var c = new Cell2d(4) >> 0;
            Assert.IsTrue(c == new Cell2d(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_RightShift_c1()
        {
            var c = new Cell2d(4) >> 1;
            Assert.IsTrue(c == new Cell2d(3));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_RightShift_cm1()
        {
            var c = new Cell2d(4) >> -1;
            Assert.IsTrue(c == new Cell2d(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        #endregion

        #region operator <<

        [Test]
        public void Cell_LeftShift_0()
        {
            var c = new Cell2d(1, -2, 4) << 0;
            Assert.IsTrue(c == new Cell2d(1, -2, 4));
            Assert.IsTrue(new Cell2d(1, -2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_1()
        {
            var c = new Cell2d(1, -2, 4) << 1;
            Assert.IsTrue(c == new Cell2d(0, -1, 5));
            Assert.IsTrue(new Cell2d(0, -2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_2()
        {
            var c = new Cell2d(1, -2, 4) << 2;
            Assert.IsTrue(c == new Cell2d(0, -1, 6));
            Assert.IsTrue(new Cell2d(0, -4, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_m1()
        {
            var c = new Cell2d(1, -2, 4) << -1;
            Assert.IsTrue(c == new Cell2d(1, -2, 4));
            Assert.IsTrue(new Cell2d(1, -2, 4).BoundingBox.Min == c.BoundingBox.Min);
        }

        [Test]
        public void Cell_LeftShift_c0()
        {
            var c = new Cell2d(4) << 0;
            Assert.IsTrue(c == new Cell2d(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_LeftShift_c1()
        {
            var c = new Cell2d(4) << 1;
            Assert.IsTrue(c == new Cell2d(5));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        [Test]
        public void Cell_LeftShift_cm1()
        {
            var c = new Cell2d(4) << -1;
            Assert.IsTrue(c == new Cell2d(4));
            Assert.IsTrue(c.IsCenteredAtOrigin);
        }

        #endregion

        #endregion
    }
}
