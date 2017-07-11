using System;
using System.IO;
using Newtonsoft.Json;

namespace SstuLib.Auxiliary
{
    public class Attachment
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; }

        [JsonProperty(PropertyName = "content")]
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