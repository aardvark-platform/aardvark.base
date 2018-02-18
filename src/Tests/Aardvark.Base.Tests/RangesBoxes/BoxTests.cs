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
    }
}
