using System;
using System.Collections.Generic;
using System.IO;
using Domino;
using LotusLib.Exceptions;

namespace LotusLib
{
    internal class Lotus
    {
        private NotesSession _lotusNotesServerSession;
        private string _currentServer;

        public delegate void ProgressEventHandler(int num);

        public event EventHandler InitializeEvent;
        public event ProgressEventHandler GetDocIndexRaise;

        public bool IsInitialize => _lotusNotesServerSession != null;

        public string CurrentServer
        {
            get
            {
                if (IsInitialize && _lotusNotesServerSession.IsOnServer)
                {
                    return _lotusNotesServerSession.ServerName;
                }
                return _currentServer;
            }
        }

        public Lotus(string server)
        {
            _currentServer = server;
        }

        private void OnInitEvent()
        {
            InitializeEvent?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeCurrentServer(string server)
        {
            _currentServer = server;
        }

        public void Init(string password)
        {
            if (_lotusNotesServerSession != null) return;
            _lotusNotesServerSession = new NotesSession();
            try
            {
                _lotusNotesServerSession.Initialize(password);
                OnInitEvent();
            }
            catch
            {
                _lotusNotesServerSession = null;
            }
        }

        public string[] GetDbFileNames()
        {
            var list = new List<string>();
            var dir = _lotusNotesServerSession.GetDbDirectory(CurrentServer);
            var db = dir.GetFirstDatabase(DB_TYPES.NOTES_DATABASE);
            while (db != null)
            {
                //if (db.FilePath.StartsWith(@"mail\")) { db = dir.GetNextDatabase(); continue; }
                list.Add(db.FilePath);
                db = dir.GetNextDatabase();
            }
            return list.ToArray();
        }

        public void GetAllItemsInDb(string dbPath, string filename)
        {
            if (!IsInitialize) throw new NotInitializeException();
            var serverDatabase = _lotusNotesServerSession.GetDatabase(CurrentServer, dbPath, false);

            var docs = serverDatabase.AllDocuments;

            using (var fileStream = new StreamWriter(filename))
            {
                var totalCount = docs.Count;
                for (var count = 1; count <= totalCount; count++)
                {
                    Console.WriteLine("Current: " + count);
                    var mail = docs.GetNthDocument(count);
                    var ni = (object[])mail.Items;
                    if (ni == null)
                        continue;
                    var namesValues = "";
                    for (var x = 0; x < ni.Length; x++)
                    {
                        var item = (NotesItem)ni[x];
                        if (!string.IsNullOrEmpty(item.Name))
                            namesValues += x + ": " + item.Name + "\t\t" + item.Text + "\r\n";
                    }
                    fileStream.WriteLine("DocUrl: '{0}'", mail.NotesURL);
                    fileStream.WriteLine(namesValues);
                    GetDocIndexRaise?.Invoke(((100 * count) / totalCount));
                }
            }
        }

        public NotesDocumentCollection SearchDocumentsByFormula(string dbReplica, string formula)
        {
            if (!IsInitialize) throw new NotInitializeException();
            var serverDatabase = _lotusNotesServerSession.GetDbDirectory(CurrentServer).OpenDatabaseByReplicaID(dbReplica);
            if (serverDatabase == null)
                throw new DatabaseNotFoundException();
            var results = serverDatabase.FTSearch(formula, 5000);
            if (results.Count == 0)
                throw new DocumentsNotFoundException();
            return results;
        }

        public IEnumerable<string> GetAttachments(NotesDocument doc, string filterName = null)
        {
            bool hasAttachment = false;
            if (doc.HasEmbedded)
            {
                foreach (NotesItem item in (object[])doc.Items)
                {
                    if (item.Name.Equals("$FILE"))
                    {
                        object[] values = (object[])item.Values;
                        var filename = values[0].ToString();
                        if(!string.IsNullOrEmpty(filterName) && !filename.Contains(filterName))
                            continue;
                        var path = Path.Combine(Path.GetTempPath(), filename);
                        var attach = doc.GetAttachment(filename);
                        attach.ExtractFile(path);
                        hasAttachment = true;
                        yield return path;
                    }
                }
                if(!hasAttachment)
                    throw new NoAttachmentException();
               yield break;
            } 
            throw new NoAttachmentException();
        }
    }
}
