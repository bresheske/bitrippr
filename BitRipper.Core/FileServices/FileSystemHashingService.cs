using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRippr.Core.Extensions;

namespace BitRippr.Core.FileServices
{
    public class FileSystemHashingService
    {
        /// <summary>
        /// Just returns random locations. :(
        /// </summary>
        /// <param name="count"></param>
        /// <param name="root"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<string> ComputeFileLocations(int count, string root, byte[] data)
        {
            var output = new List<string>();

            /* Gather all directories, recursivly. */
            var folders = Directory.GetDirectories(root, "*", SearchOption.AllDirectories).ToList();
            folders.Add(root);

            /* Just add some random directories. */
            var rand = new Random();
            for (int i = 0; i < count; i++)
                output.Add(folders[rand.Next(folders.Count)]);

            /* Populate file names. */
            for (int i = 0; i < output.Count; i++)
                output[i] += @"\" + GetNonExistingFile(rand.Next().ToString().ToMD5());

            return output;
        }

        private string GetNonExistingFile(string file)
        {
            if (!File.Exists(file))
                return file;
            var rand = new Random();
            return GetNonExistingFile(Path.GetDirectoryName(file) + "/" + rand.Next().ToString().ToMD5());
        }

        /// <summary>
        /// Generates an unreadable directory structure.
        /// </summary>
        /// <param name="count">
        /// Stopping condition. 
        /// Total directories will roughy be this number.
        /// However, no gaurantee.
        /// </param>
        /// <param name="root"></param>
        public void GenerateDirectoryStructure(int start, int count, string current, string root)
        {
            if (start >= count)
                return;

            /* Generate on current. */
            var currcount = start + GenerateFolders(current);

            /* 
             * Grab next. 
             * To find next directory: 
             *  33% chance of current directory
             *  33% chance of child directory
             *  33% chance of parent directory (if not the root)
             */
            var chance = current == root
                ? new Random().Next(2)
                : new Random().Next(3);
            if (chance == 0)
                GenerateDirectoryStructure(currcount, count, current, root);
            else if (chance == 1)
            {
                var children = Directory.GetDirectories(current);
                var lucky = children.ElementAt(new Random().Next(children.Count()));
                GenerateDirectoryStructure(currcount, count, lucky, root);
            }
            else if (chance == 2)
                GenerateDirectoryStructure(currcount, count, Directory.GetParent(current).FullName, root);
        }

        private int GenerateFolders(string folder)
        {
            var random = new Random();
            var num = random.Next(20);
            for (int i = 0; i < num; i++)
            {
                var hashname = (num + i).ToString().ToMD5(DateTime.Now.Millisecond.ToString())
                    .Substring(0, 8);
                var fullname = folder + @"\" + hashname;
                if (fullname.Length < 248)
                    Directory.CreateDirectory(folder + @"\" + hashname);
            }
            return num;
        }
    }
}