using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aardvark.Base;
using FSharp.Data.Adaptive;
using CSharp.Data.Adaptive;
using FSharp.Data.Traceable;

namespace IncrementalDemo.CSharp
{
    class Program
    {
        static void SimpleCallbackTest()
        {
            var m = new ChangeableValue<int>(0);
            var d = m.Map(a => 2 * a);

            Report.Begin("m = 0");

            void print ()
            {
                Report.Line("m * 2 = {0}", d.GetValue());
            }

            Report.End();

            Report.Begin("m = 3");
            using (Adaptive.Transact)
            {
                m.Value = 3;
            }
            print();
            Report.End();


            Report.Begin("m = 1; m = 2");
            using (Adaptive.Transact)
            {
                m.Value = 1;
                m.Value = 2;
            }
            print();
            Report.End();

        }

        static void SetContainmentTest()
        {
            var set = new ChangeableHashSet<int>(new [] { 1, 2, 3, 4 });


            var greater1 = set.Filter(i => i > 1);


            var reader = greater1.GetReader();

            Report.Begin("set = {1,2,3,4}");
            void print()
            {
                var deltas = reader.GetChanges(AdaptiveToken.Top);
                foreach (var d in deltas)
                {
                    if (d.Count > 0) Report.Line("add {0}", d.Value);
                    else Report.Line("rem {0}", d.Value);
                }
            }
            Report.End();

            

            Report.Begin("set = {1,2,3,4,5,6}");
            using (Adaptive.Transact)
            {
                set.Add(5); set.Add(6);
            }
            print();
            Report.End();

            Report.Begin("set = {2,4,6}");
            using (Adaptive.Transact)
            {
                set.Remove(1); set.Remove(3); set.Remove(5);
            }
            print();
            Report.End();

            Report.Begin("set = {4,6}");
            using (Adaptive.Transact)
            {
                set.Remove(2);
            }
            print();
            Report.End();

            Report.Begin("set = {4,5,6}");
            using (Adaptive.Transact)
            {
                set.Add(5);
            }
            print();
            Report.End();

            

        }


        static void AdvancedASetTest()
        {
            Action<IOpReader<CountingHashSet<int>, FSharpHashSetDelta<int>>> print = (r) =>
            {
                var deltas = r.GetChanges(AdaptiveToken.Top);
                var content = r.State;

                var deltaStr = deltas.Select(d => d.Count > 0 ? string.Format("Add {0}", d.Value) : string.Format("Rem {0}", d.Value)).Join(", ");
                var contentStr = content.OrderBy(a => a).Select(i => i.ToString()).Join(", ");

                Report.Line("deltas = [{0}]", deltaStr);
                Report.Line("content = [{0}]", contentStr);
            };

            var i0 = new ChangeableHashSet<int>(new [] { 1, 2, 3 });
            var i1 = new ChangeableHashSet<int>(new [] { 4, 5 });
            var i2 = new ChangeableHashSet<int>(new [] { 6, 7 });
            var input = new ChangeableHashSet<IAdaptiveHashSet<int>>(new [] { i0, i1 });

            var flat = input.SelectMany(a => a);

            Report.Begin("initial");
            var reader = flat.GetReader();
            print(reader);
            Report.End();


            Report.Begin("add i2; i1.rem 4");
            Report.Line("all changes (inner and outer) shall be seen consistently");
            using (Adaptive.Transact)
            {
                input.Add(i2);
                i1.Remove(4);
            }
            print(reader);
            Report.End();


            Report.Begin("rem i0; i0.add 10");
            Report.Line("the set shall not see the (Add 10) operation since i0 was removed as a whole");
            using (Adaptive.Transact)
            {
                input.Remove(i0);
                i0.Add(10);
            }
            print(reader);
            Report.End();



            Report.Begin("add i0; i0.rem 10");
            Report.Line("the set shall not see the (Add 10) operation since 10 was removed from i0");
            using (Adaptive.Transact)
            {
                input.Add(i0);
                i0.Remove(10);
            }
            print(reader);
            Report.End();

        }


        static void Main(string[] args)
        {
            AdvancedASetTest();
            //SetContainmentTest();
            //SimpleCallback();

        }
    }
}
