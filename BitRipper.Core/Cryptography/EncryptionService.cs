using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BitRippr.Core.Extensions;

namespace BitRippr.Core.Cryptography
{
    public class EncryptionService
    {
        public byte[] EncryptBytes(byte[] data, byte[] key)
        {
            byte[] output;
            using (var rij = new RijndaelManaged() { Padding = PaddingMode.ISO10126, Mode = CipherMode.CBC, Key = key.ExtrapolateTo(32), IV = key.ExtrapolateTo(16) })
            using (var memstream = new MemoryStream())
            using (var stream = new CryptoStream(memstream, rij.CreateEncryptor(), CryptoStreamMode.Write))
            {
                stream.Write(data, 0, data.Length);
                stream.FlushFinalBlock();
                stream.Flush();
                output = memstream.ToArray();
            }
            return output;
        }

        public byte[] DecryptBytes(byte[] data, byte[] key)
        {
            var memout = new MemoryStream();
            using (var memstream = new MemoryStream(data))
            using (var rij = new RijndaelManaged() { Padding = PaddingMode.ISO10126, Mode = CipherMode.CBC, Key = key.ExtrapolateTo(32), IV = key.ExtrapolateTo(16) })
            using (var stream = new CryptoStream(memstream, rij.CreateDecryptor(), CryptoStreamMode.Read))
            {
                var buffer = new byte[1024];
                var read = stream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    memout.Write(buffer, 0, read);
                    read = stream.Read(buffer, 0, buffer.Length);
                }
            }
            return memout.ToArray();
        }
    }
}
