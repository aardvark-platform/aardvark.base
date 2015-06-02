using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class TelemetryTests : TestSuite
    {
        public TelemetryTests() : base() { }
        public TelemetryTests(TestSuite.Options options) : base(options) { }

        public void NUnitRun()
        {
            Test.Begin("Telemetry tests");
            SnapshotProbeLong_CanBeCreated();
            SnapshotProbeLong_Works();
            SnapshotProbeDouble_CanBeCreated();
            SnapshotProbeDouble_Works();
            SnapshotProbeTimeSpan_CanBeCreated();
            SnapshotProbeTimeSpan_Works();
            Test.End();
        }


        [Test]
        public void SnapshotProbeLong_CanBeCreated()
        {
            Test.Begin("SnapshotProbeLong can be created");
            var a = new Telemetry.Counter();
            a.Set(100);

            var s = new Telemetry.SnapshotProbeLong(a);

            Test.IsTrue(s.Value == 0);
            Test.End();
        }

        [Test]
        public void SnapshotProbeLong_Works()
        {
            Test.Begin("SnapshotProbeLong works");
            var a = new Telemetry.Counter();
            a.Set(100);

            var s = new Telemetry.SnapshotProbeLong(a);
            
            a.Increment();
            Test.IsTrue(s.Value == 1);

            a.Increment();
            Test.IsTrue(s.Value == 2);

            a.Decrement();
            Test.IsTrue(s.Value == 1);

            a.Decrement(2);
            Test.IsTrue(s.Value == -1);
            Test.End();
        }

        [Test]
        public void SnapshotProbeDouble_CanBeCreated()
        {
            Test.Begin("SnapshotProbeDouble can be created");
            double x = 5.1;
            var a = new Telemetry.CustomProbeDouble(() => x);

            var s = new Telemetry.SnapshotProbeDouble(a);

            Test.IsTrue(s.Value == 0.0);
            Test.End();
        }

        [Test]
        public void SnapshotProbeDouble_Works()
        {
            Test.Begin("SnapshotProbeDouble works");
            double x = 5.1;
            var a = new Telemetry.CustomProbeDouble(() => x);

            var s = new Telemetry.SnapshotProbeDouble(a);

            x = 6.1;
            Test.IsTrue(s.Value == 1.0);

            x = 10.1;
            Test.IsTrue(s.Value == 5.0);

            x = 0.0;
            Test.IsTrue(s.Value == -5.1);
            Test.End();
        }

        [Test]
        public void SnapshotProbeTimeSpan_CanBeCreated()
        {
            Test.Begin("SnapshotProbeTimeSpan can be created");
            var x = TimeSpan.FromSeconds(60);
            var a = new Telemetry.CustomProbeTimeSpan(() => x);

            var s = new Telemetry.SnapshotProbeTimeSpan(a);

            Test.IsTrue(s.Value == TimeSpan.FromSeconds(0));
            Test.End();
        }

        [Test]
        public void SnapshotProbeTimeSpan_Works()
        {
            Test.Begin("SnapshotProbeTimeSpan works");
            var x = TimeSpan.FromSeconds(60);
            var a = new Telemetry.CustomProbeTimeSpan(() => x);

            var s = new Telemetry.SnapshotProbeTimeSpan(a);

            x = TimeSpan.FromSeconds(70);
            Test.IsTrue(s.Value == TimeSpan.FromSeconds(10.0));

            x = TimeSpan.FromSeconds(120);
            Test.IsTrue(s.Value == TimeSpan.FromSeconds(60.0));

            x = TimeSpan.FromSeconds(0);
            Test.IsTrue(s.Value == TimeSpan.FromSeconds(-60.0));
            Test.End();
        }
    }
}
