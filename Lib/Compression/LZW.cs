using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.Core.Parser.VMF.Lib.Compression
{
    public static class LZW
    {
        public static List<long> Compress(string uncompressed)
        {
            // build the dictionary
            Dictionary<string, long> dictionary = new Dictionary<string, long>();
            
            for (long i = 0; i < 256; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            string w = string.Empty;
            List<long> compressed = new List<long>();
            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, (long)dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
            {
                compressed.Add(dictionary[w]);
            }

            return compressed;
        }

        public static string Decompress(List<long> compressed)
        {
            // build the dictionary
            Dictionary<long, string> dictionary = new Dictionary<long, string>();
            for (long i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = dictionary[compressed[0]];
            compressed.RemoveAt(0);
            StringBuilder decompressed = new StringBuilder(w);

            foreach (long k in compressed)
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dictionary.Add((long)dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }
    }
}
