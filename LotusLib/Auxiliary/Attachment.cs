using System;
using System.IO;

namespace LotusLib.Auxiliary
{
    public class Attachment : IDisposable
    {
        private bool _disposed = false;
        private string AttachPath { get; set; }
        public string Name => Path.GetFileName(AttachPath);

        public Attachment(string path)
        {
            this.AttachPath = path;
        }

        public byte[] GetContent()
        {
            return File.ReadAllBytes(AttachPath);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            if (this.AttachPath != null)
            {
                try
                {
                    File.Delete(this.AttachPath);
                }
                catch
                {
                    // ignored
                }
                this.AttachPath = null;
            }
            _disposed = true;
        }

        ~Attachment()
        {
            Dispose(false);
        }
    }
}