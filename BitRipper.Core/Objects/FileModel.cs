using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippr.Core.Objects
{
    public class FileModel
    {
        public byte[] Data { get; set; }
        public string NextFile { get; set; }
        public int Segments { get; set; }
    }
}