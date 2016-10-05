using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRippr.Core.Extensions
{
    public static class BytesExtensions
    {
        public static byte[] ExtrapolateTo(this byte[] bytes, int length)
        {
            if (bytes.Length >= length)
                return bytes
                    .Take(length)
                    .ToArray();
            
            var output = new byte[length];
            for (int i = 0; i < length; i++)
            {
                var index = i;
                if (bytes.Length <= i)
                    index = i % bytes.Length;
                output[i] = bytes[index];
            }
            return output;
        }
    }
}