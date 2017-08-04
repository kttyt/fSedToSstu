using System;
using System.IO;
using System.IO.Compression;

namespace fSedToSstu
{
    public class ZipArchiveParted : IDisposable
    {
        private FileStream FileStream { get; set; }
        private ZipArchive ZipArchive { get; set; }

        public double MaxSize { get; set; }
        public string TemplateName { get; set; }
        public int PartsCount { get; private set; }

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

        public ZipArchiveEntry CreateEntry(string entryName)
        {
            if (FileStream == null || FileStream.Length > MaxSize)
                CreateNewPart();
            var entry = ZipArchive.CreateEntry(entryName, CompressionLevel.Optimal);
            return entry;
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
