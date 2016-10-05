using BitRippr.Core.Cryptography;
using BitRippr.Core.FileServices;
using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippr.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var encrypt = false;
            var decrypt = false;
            var filename = string.Empty;
            var keyname = string.Empty;
            var rootdir = string.Empty;
            var help = false;
            var segments = 0;
            var options = new OptionSet()
            {
                
                {"e", "Encrypt Mode", o => encrypt = true},
                {"d", "Decrypt Mode", o => decrypt = true},
                {"f=", "File", o => filename = o},
                {"k=", "Key File", o => keyname = o},
                {"s=", "Segments", o => segments = int.Parse(o)},
                {"r=", "Root Directory", o => rootdir = o},
                {"h", "Help", o => help = true}
            };
            options.Parse(args);

            if (help
                || (!encrypt && !decrypt))
            {
                options.WriteOptionDescriptions(System.Console.Out);
                return;
            }

            if (!Path.IsPathRooted(filename))
                filename = Environment.CurrentDirectory + @"\" + filename;
            if (!Path.IsPathRooted(keyname))
                keyname = Environment.CurrentDirectory + @"\" + keyname;
            if (!Path.IsPathRooted(rootdir))
                rootdir = Environment.CurrentDirectory + @"\" + rootdir;

            if ((encrypt || decrypt) && !File.Exists(filename))
            {
                System.Console.WriteLine("File '{0}' does not exist.", filename);
                return;
            }
            if ((encrypt || decrypt) && !File.Exists(keyname))
            {
                System.Console.WriteLine("File '{0}' does not exist.", keyname);
                return;
            }
            if (encrypt && !Directory.Exists(rootdir))
            {
                System.Console.WriteLine("Directory '{0}' does not exist.", rootdir);
                return;
            }


            if (encrypt)
            {
                System.Console.WriteLine("Encrypting File - '{0}'", filename);
                new FileEncryptionService().EncryptFile(filename, keyname, segments, rootdir);
                System.Console.WriteLine("Done.");
            }
            else if (decrypt)
            {
                System.Console.WriteLine("Decrypting File - '{0}'", filename);
                new FileEncryptionService().DecryptFile(filename, keyname);
                System.Console.WriteLine("Done.");
            }
        }
    }
}