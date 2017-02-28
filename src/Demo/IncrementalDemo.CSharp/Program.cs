using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aardvark.Base;
using Aardvark.Base.Incremental;
using Aardvark.Base.Incremental.CSharp;
using Aardvark.Base.Incremental.Validation;

namespace IncrementalDemo.CSharp
{
    class Program
    {
        static void SimpleCallbackTest()
        {
            var m = new ModRef<int>(0);
            var d = m.Select(a => 2 * a);

            Report.Begin("m = 0");
            var s = d.UnsafeRegisterCallbackNoGcRoot(v =>
            {
                Report.Line("m * 2 = {0}", v);
            });
            Report.End();

            Report.Begin("m = 3");
            using (Adaptive.Transaction)
            {
                m.Value = 3;
            }
            Report.End();


            Report.Begin("m = 1; m = 2");
            using (Adaptive.Transaction)
            {
                m.Value = 1;
                m.Value = 2;
            }
            Report.End();

        }

        static void SetContainmentTest()
        {
            var set = new cset<int> { 1, 2, 3, 4 };


            var greater1 = from i in set where i > 1 select i;
            var contains5 = greater1.ContainsMod(5);
            var contains2And4 = greater1.ContainsAll(2, 4);



            Report.Begin("set = {1,2,3,4}");
            greater1.UnsafeRegisterCallbackNoGcRoot(deltas =>
            {
                foreach(var d in deltas)
                {
                    if (d.Count > 0) Report.Line("add {0}", d.Value);
                    else Report.Line("rem {0}", d.Value);
                }
            });

            contains5.UnsafeRegisterCallbackNoGcRoot(c => Report.Line("contains 5 = {0}", c));
            contains2And4.UnsafeRegisterCallbackNoGcRoot(c => Report.Line("contains [2,4] = {0}", c));
            Report.End();

            

            Report.Begin("set = {1,2,3,4,5,6}");
            using (Adaptive.Transaction)
            {
                set.Add(5); set.Add(6);
            }
            Report.End();

            Report.Begin("set = {2,4,6}");
            using (Adaptive.Transaction)
            {
                set.Remove(1); set.Remove(3); set.Remove(5);
            }
            Report.End();

            Report.Begin("set = {4,6}");
            using (Adaptive.Transaction)
            {
                set.Remove(2);
            }
            Report.End();

            Report.Begin("set = {4,5,6}");
            using (Adaptive.Transaction)
            {
                set.Add(5);
            }
            Report.End();

            

        }


        static void AdvancedASetTest()
        {
            Action<IOpReader<hrefset<int>, hdeltaset<int>>> print = (r) =>
            {
                var deltas = r.GetDeltas();
                var content = r.State;

                var deltaStr = deltas.Select(d => d.Count > 0 ? string.Format("Add {0}", d.Value) : string.Format("Rem {0}", d.Value)).Join(", ");
                var contentStr = content.OrderBy(a => a).Select(i => i.ToString()).Join(", ");

                Report.Line("deltas = [{0}]", deltaStr);
                Report.Line("content = [{0}]", contentStr);
            };

            var i0 = new cset<int> { 1, 2, 3 };
            var i1 = new cset<int> { 4, 5 };
            var i2 = new cset<int> { 6, 7 };
            var input = new cset<aset<int>> { i0, i1 };

            var flat = input.SelectMany(a => a);

            Report.Begin("initial");
            var reader = flat.GetReader();
            print(reader);
            Report.End();


            Report.Begin("add i2; i1.rem 4");
            Report.Line("all changes (inner and outer) shall be seen consistently");
            using (Adaptive.Transaction)
            {
                input.Add(i2);
                i1.Remove(4);
            }
            print(reader);
            Report.End();


            Report.Begin("rem i0; i0.add 10");
            Report.Line("the set shall not see the (Add 10) operation since i0 was removed as a whole");
            using (Adaptive.Transaction)
            {
                input.Remove(i0);
                i0.Add(10);
            }
            print(reader);
            Report.End();



            Report.Begin("add i0; i0.rem 10");
            Report.Line("the set shall not see the (Add 10) operation since 10 was removed from i0");
            using (Adaptive.Transaction)
            {
                input.Add(i0);
                i0.Remove(10);
            }
            print(reader);
            Report.End();


            reader.Dump();
        }


        static void Main(string[] args)
        {
            AdvancedASetTest();
            //SetContainmentTest();
            //SimpleCallback();

        }
    }
}
