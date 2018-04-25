using Aardvark.Base;
using NUnit.Framework;
using System.Drawing;
using System.IO;
using System.Net;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class SystemDrawingTests
    {
        private static string TestImageUrl = @"https://github.com/aardvark-platform/aardvark.docs/wiki/gallery/HelloWorld.jpg";
        private static byte[] TestImageBuffer = new WebClient().DownloadData(TestImageUrl);

        [Test]
        public void SystemDrawingBitmap_ToPixImage()
        {
            using (var stream = new MemoryStream(TestImageBuffer))
            {
                var a = (Bitmap)Image.FromStream(stream);
                var b = a.ToPixImage();
                Assert.IsTrue(a.Width == b.Size.X && a.Height == b.Size.Y);
            }
        }

        [Test]
        public void SystemDrawingImage_ToPixImage()
        {
            using (var stream = new MemoryStream(TestImageBuffer))
            {
                var a = Image.FromStream(stream);
                var b = a.ToPixImage();
                Assert.IsTrue(a.Width == b.Size.X && a.Height == b.Size.Y);
            }
        }

        [Test]
        public void PixImage_ToSystemDrawingBitmap()
        {
            using (var stream = new MemoryStream(TestImageBuffer))
            {
                var a = PixImage.Create(stream);
                var b = a.ToSystemDrawingBitmap();

                Assert.IsTrue(a.Size.X == b.Width && a.Size.Y == b.Height);
            }
        }
        
        [Test]
        public void SaveViaSystemDrawing_Png()
        {
            using (var stream = new MemoryStream(TestImageBuffer))
            {
                var a = PixImage.Create(stream);

                using (var store = new MemoryStream())
                {
                    var success = a.SaveViaSystemDrawing(store, PixFileFormat.Png, PixSaveOptions.Default, 100);
                    Assert.IsTrue(success);
                    store.Seek(0, SeekOrigin.Begin);
                    var b = PixImage.Create(store);
                    Assert.IsTrue(a.Size == b.Size);
                }
            }
        }

        [Test]
        public void SaveViaSystemDrawing_Jpg()
        {
            using (var stream = new MemoryStream(TestImageBuffer))
            {
                var a = PixImage.Create(stream);

                using (var store = new MemoryStream())
                {
                    var success = a.SaveViaSystemDrawing(store, PixFileFormat.Jpeg, PixSaveOptions.Default, 90);
                    Assert.IsTrue(success);
                    store.Seek(0, SeekOrigin.Begin);
                    var b = PixImage.Create(store);
                    Assert.IsTrue(a.Size == b.Size);
                }
            }
        }
    }
}
