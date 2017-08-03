using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fSedToSstu
{
    public class ZipArchiveParted : IDisposable
    {
        private FileStream FileStream { get; set; }
        private ZipArchive ZipArchive { get; set; }

        public double MaxSize { get; set; }
        public string TemplateName { get; set; }
        public int PartsCount { get; set; }

        public ZipArchiveParted(string template, double maxSize)
        {
            TemplateName = template;
            MaxSize = maxSize;
        }

        private void CreateNewPart()
        {
            ZipArchive?.Dispose();
            FileStream?.Dispose();

            PartsCount++;
            var name = GetNextName();
            try
            {
                FileStream = File.Create(name);
                ZipArchive = new ZipArchive(FileStream, ZipArchiveMode.Create);
            }
            catch
            {
                ZipArchive?.Dispose();
                FileStream?.Dispose();
            }
        }

        public void AddEntry(string entryName, string str)
        {
            if(FileStream == null || FileStream.Length > MaxSize)
                CreateNewPart();
            var entry = ZipArchive.CreateEntry(entryName, CompressionLevel.Optimal);
            using (var entryStream = entry.Open())
            using (var sw = new StreamWriter(entryStream))
            {
                sw.Write(str);
            }
        }

        private string GetNextName()
        {
            var path = Path.GetDirectoryName(TemplateName) ?? "";
            var extension = Path.GetExtension(TemplateName);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(TemplateName);
            return Path.Combine(
                    path,
                    Path.ChangeExtension(nameWithoutExtension + PartsCount.ToString("D3"), extension)
                );
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ZipArchive?.Dispose();
                FileStream?.Dispose();
            }
        }
    }
}
