using System;
using System.IO;

namespace SstuLib.Auxiliary
{
    public class Attachment
    {
        public string Name { get; }
        public string Content { get; }
        internal int Length => Content.Length;

        public Attachment(string filename, byte[] contentBytes)
        {
            Name = filename;
            Content = Convert.ToBase64String(contentBytes);
        }

        public static Attachment FromFile(string path)
        {
            var filename = Path.GetFileName(path);
            var contentBytes = File.ReadAllBytes(path);

            return new Attachment(filename, contentBytes);
        }
    }
}