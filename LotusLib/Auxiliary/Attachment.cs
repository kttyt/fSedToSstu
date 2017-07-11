using System;
using System.IO;

namespace LotusLib.Auxiliary
{
    public class Attachment : IDisposable
    {
        private bool _disposed = false;
        private string AttachPath { get; }
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
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            File.Delete(AttachPath);
            _disposed = true;
        }

        ~Attachment()
        {
            Dispose(false);
        }
    }
}