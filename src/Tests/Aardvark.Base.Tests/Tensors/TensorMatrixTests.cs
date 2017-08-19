using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Aardvark.Base;

namespace Aardvark.Tests
{
    [TestFixture]
    public class TensorMatrixTests : TestSuite
    {
        [Test]
        public void MatrixTranspose()
        {
            var matT = Matrix.Create(new[] { 1.0, -1.0, 2.0,
                                            0.0, -3.0, 1.0 }, 3, 2).Transposed;
            
            var res = Matrix.Create(new[] { 1.0,  0.0,
                                           -1.0, -3.0,
                                            2.0,  1.0}, 2, 3);

            Assert.True(res.EqualTo(matT));
            //Assert.True(matT.Elements.ZipTuples(res.Elements).All(x => x.E0 == x.E1)); // Elements orderd by index ?!

            var mat2T = Matrix.Create(new[] { 7.0, 14.0,
                                             9.0, 18.0}, 2, 2).Transposed;

            var res2 = Matrix.Create(new[] { 7.0,  9.0,
                                           14.0, 18.0}, 2, 2);

            Assert.True(res2.EqualTo(mat2T));
            //Assert.True(mat2T.Elements.ZipTuples(res2.Elements).All(x => x.E0 == x.E1)); // Elements orderd by index ?!
        }

        [Test]
        public void MatrixVectorMult()
        {
            var a = new Matrix<double>(new[] { 1.0, -1.0, 2.0,
                                               0.0, -3.0, 1.0 }, 3, 2);
            var b = new Vector<double>(new[] { 2.0, 1.0, 0.0 });
            
            var mul = a.Multiply(b);

            var res = new Vector<double>(new[] { 1.0, -3.0, });
            var res2 = new Vector<double>(a.Dim.Y).SetByCoord(x => a.GetRow((int)x).DotProduct(b));

            res.Data.ForEach((x, i) => Assert.True(x == mul.Data[i], "Wrong result"));

            Assert.True(res.EqualTo(mul));
            Assert.True(res2.EqualTo(mul));
        }

        [Test]
        public void MatrixVectorMultTransposed()
        {
            var a = new Matrix<double>(new[] { 1.0,  0.0,
                                              -1.0, -3.0,
                                               2.0,  1.0 }, 2, 3);
            var b = new Vector<double>(new[] { 2.0, 1.0, 0.0 });

            var mul = a.Transposed.Multiply(b);

            var res = new Vector<double>(new[] { 1.0, -3.0, });

            res.Data.ForEach((x, i) => Assert.True(x == mul.Data[i], "Wrong result"));

            Assert.True(res.EqualTo(mul));
        }

        [Test]
        public void MatrixMatrixMult()
        {
            var a = new Matrix<double>(new[] { 0.0,  4.0, -2.0,
                                              -4.0, -3.0,  0.0 }, 3, 2);
            var b = new Matrix<double>(new[] { 0.0,  1.0,
                                               1.0, -1.0,
                                               2.0,  3.0}, 2, 3);

            var mul = a.Multiply(b);

            var res = new Matrix<double>(new[] { 0.0, -10.0,
                                                -3.0, -1.0 }, 2, 2);

            res.Data.ForEach((x, i) => Assert.True(x == mul.Data[i], "Wrong result"));

            Assert.True(res.EqualTo(mul));
        }

        [Test]
        public void MatrixMatrixMultTranspose()
        {
            var a = Matrix.Create(new[] { 1.0, 2.0, 3.0,
                                          4.0, 5.0, 6.0 }, 3, 2);
            var b = Matrix.Create(new[] { 7.0, 9.0, 11.0,
                                          8.0, 10.0, 12.0 }, 3, 2);

            var mul = a.Multiply(b.Transposed);

            var res = Matrix.Create(new[] { 58.0,  64.0,
                                           139.0, 154.0 }, 2, 2);
            
            Assert.True(res.EqualTo(mul));
        }

        [Test]
        public void VectorVectorMult()
        {
            var a = Vector.Create(new[] { 1.0, 2.0, 3.0 });
            var b = Vector.Create(new[] { 7.0, 9.0, 11.0});

            var mul = a.Multiply(b);

            var res = Vector.Create(new[] { 7.0, 18.0, 33.0 });
            
            Assert.True(res.EqualTo(mul));
            
            var mat = a.MultiplyTransposed(b);

            var matRes1 = new Matrix<double>(b.Dim, a.Dim).SetByCoord(crd => a[crd.Y] * b[crd.X]);

            var matRes2 = Matrix.Create(new[] { 7.0, 14.0, 21.0,
                                               9.0, 18.0, 27.0,
                                              11.0, 22.0, 33.0}, 3, 3).Transposed;
            
            var m1 = Matrix.Create(a.Data, 3, 1);
            var m2 = Matrix.Create(b.Data, 3, 1);

            var matRes3 = m1.Transposed.Multiply(m2);

            Assert.True(mat.EqualTo(matRes1));
            Assert.True(mat.EqualTo(matRes2));
            Assert.True(mat.EqualTo(matRes3));
        }

        [Test]
        public void MatrixArithmetic()
        {
            var a = Matrix.Create(new[] { 1.0, 2.0, 3.0,
                                          4.0, 5.0, 6.0 }, 3, 2);
            var b = Matrix.Create(new[] { 7.0, 9.0, 11.0,
                                          8.0, 10.0, 12.0 }, 3, 2);

            var ab = a.Add(b);
            var addRes = Matrix.Create(new[] { 8.0, 11.0, 14.0,
                                               12.0, 15.0, 18.0}, 3, 2);

            Assert.True(ab.EqualTo(addRes));

            var a2 = a.Add(2);
            var addRes2 = Matrix.Create(new[] { 3.0, 4.0, 5.0,
                                                6.0, 7.0, 8.0}, 3, 2);

            Assert.True(a2.EqualTo(addRes2));
        }
    }
}
