using BitRippr.Core.FileServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippr.Test
{
    [TestFixture]
    public class FileSystemHashingServiceTests
    {

        [Test]
        public void TestFileGenerator()
        {
            var file = File.ReadAllBytes(@"C:\Users\brandon\Google Drive\Archive\3586-S13H01-Brandon-Resheske.docx");
            var segments = 5000;
            var root = @"C:\temp";
            var files = new FileSystemHashingService().ComputeFileLocations(segments, root, file);
            Assert.AreEqual(segments, files.Count);
        }

        [Test]
        public void TestFolderGenerator()
        {
            var root = @"C:\temp";
            new FileSystemHashingService().GenerateDirectoryStructure(0, 100, root, root);
        }
    }
}