using BitRippr.Core.Cryptography;
using BitRippr.Core.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BitRippr.Core.FileServices
{
    public class FileEncryptionService
    {
        public void DecryptFile(string filename, string keyname)
        {
            /* Read the header file, decrypt it. */
            var key = File.ReadAllBytes(keyname);
            var bytes = new EncryptionService()
                .DecryptBytes(File.ReadAllBytes(filename), key);
            var model = new JavaScriptSerializer()
                .Deserialize<FileModel>(System.Text.Encoding.UTF8.GetString(bytes));
            var nextfile = model.NextFile;
            var segments = model.Segments;
            File.Delete(filename);
            var key_bytespersegment = key.Length / segments;
            var count = 0;

            var output = File.OpenWrite(filename.Substring(0, filename.LastIndexOf('.')));
            while (!string.IsNullOrWhiteSpace(nextfile) && File.Exists(nextfile))
            {
                bytes = new EncryptionService()
                    .DecryptBytes(File.ReadAllBytes(nextfile), 
                        key.Skip(count * key_bytespersegment)
                        .Take(key_bytespersegment)
                        .ToArray());
                model = new JavaScriptSerializer()
                    .Deserialize<FileModel>(System.Text.Encoding.UTF8.GetString(bytes));
                File.Delete(nextfile);
                nextfile = model.NextFile;
                output.Write(model.Data, 0, model.Data.Length);
                count++;
            }
            output.Close();
        }

        public void EncryptFile(string filename, string keyname, int segments, string root)
        {
            /* Loop through file. */
            var data_filesize = new FileInfo(filename).Length;
            if (data_filesize < segments)
                segments = (int)data_filesize;

            var data_bytespersegment = data_filesize / segments;
            var data_currbytes = new byte[data_bytespersegment];
            
            var key_data = File.ReadAllBytes(keyname);
            var key_bytespersegment = key_data.Length / segments;

            var data_stream = File.OpenRead(filename);

            /* Files we are using. */
            var hasher = new FileSystemHashingService();
            var files = hasher.ComputeFileLocations(segments % 32 == 0 ? segments : segments + 1, root, key_data);

            /* First write the header. */
            var model = new FileModel()
            {
                NextFile = files[0],
                Segments = segments
            };
            var modelstring = new JavaScriptSerializer().Serialize(model);
            var data = System.Text.Encoding.UTF8.GetBytes(modelstring);
            var enc = new EncryptionService().EncryptBytes(data, File.ReadAllBytes(keyname));
            File.WriteAllBytes(filename + ".enc", enc);

            var bytesread = data_stream.Read(data_currbytes, 0, (int)data_bytespersegment);
            var bytelocation = 0;
            var index = 0;
            while (bytesread > 0)
            {
                /* Read in our key vector. */
                var key = key_data
                    .Skip(index * key_bytespersegment)
                    .Take(key_bytespersegment)
                    .ToArray();
                
                /* Encrypt */
                model = new FileModel()
                {
                    Data = data_currbytes,
                    NextFile = index < files.Count - 1 ? files[index + 1] : string.Empty
                };
                modelstring = new JavaScriptSerializer().Serialize(model);
                data = System.Text.Encoding.UTF8.GetBytes(modelstring);
                enc = new EncryptionService().EncryptBytes(data, key);
                File.WriteAllBytes(files[index], enc);

                bytesread = data_stream.Read(data_currbytes, 0, (int)data_bytespersegment);
                bytelocation += bytesread;
                index++;
            }
            data_stream.Close();
            File.Delete(filename);
        }
    }
}
