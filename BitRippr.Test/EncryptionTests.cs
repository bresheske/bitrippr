using BitRippr.Core.Cryptography;
using BitRippr.Core.FileServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippr.Test
{
    [TestFixture]
    public class EncryptionTests
    {

        [Test]
        public void TestEncryptionWriting()
        {
            var file = @"C:\temp\SomeStuff.txt";
            var key = @"C:\temp\SomeKey.txt";
            var segments = 200;
            var root = @"C:\temp";
            var enc = new FileEncryptionService();
            enc.EncryptFile(file, key, segments, root);
        }

        [Test]
        public void TestDecryptionReading()
        {
            var file = @"C:\temp\SomeStuff.txt.enc";
            var key = @"C:\temp\SomeKey.txt";
            var enc = new FileEncryptionService();
            enc.DecryptFile(file, key);
        }

        [Test]
        public void TestEncryptionService()
        {
            var data = System.Text.Encoding.UTF8.GetBytes("{Data:[anawoqpqienzxcv],Next='banana.txt'}");
            var key = System.Text.Encoding.UTF8.GetBytes("bananakey");

            var encrypted = new EncryptionService().EncryptBytes(data, key);
            var decrypted = new EncryptionService().DecryptBytes(encrypted, key);

            Assert.AreEqual(System.Text.Encoding.UTF8.GetString(data), 
                System.Text.Encoding.UTF8.GetString(decrypted));
        }
    }
}