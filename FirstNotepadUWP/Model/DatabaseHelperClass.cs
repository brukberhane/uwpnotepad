using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstNotepadUWP
{
    class DatabaseHelperClass
    {

        //Create table
        public void CreateDatabase(string DB_PATH)
        {
            if (!CheckFileExists(DB_PATH).Result)
            {
                using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new
                    SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), DB_PATH))
                {
                    conn.CreateTable<Notes>();
                }
            }
        }

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Insert new note into the table
        public void Insert(Notes notesOBj)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new
                SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {
                conn.RunInTransaction(() =>
                {
                    conn.Insert(notesOBj);
                });
            }
        }

        //Retrieve the specific note 
        public Notes ReadNote(int noteId)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new
                SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {
                var existingNote = conn.Query<Notes>
                    ("select * from Notes where Id =" + noteId).FirstOrDefault();
                return existingNote;
            }
        }

        public ObservableCollection<Notes> ReadAllNotes()
        {
            try
            {
                using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new
                    SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
                {
                    List<Notes> mycollection = conn.Table<Notes>().ToList();
                    ObservableCollection<Notes> NotesList = new ObservableCollection<Notes>(mycollection);
                    return NotesList;
                }
            }
            catch
            {
                return null;
            }
        }

        //Update existing note
        public void UpdateDetails(Notes objNote)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new
                SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {

                var existingNote = conn.Query<Notes>
                    ("select * from Notes where Id =" + objNote.Id).FirstOrDefault();
                if (existingNote != null)
                {
                    conn.RunInTransaction(() =>
                    {
                        conn.Update(objNote);
                    });
                }
            }
        }

        //Delete entire note table
        public void DeleteAllNotes()
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new 
                SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {
                conn.DropTable<Notes>();
                conn.CreateTable<Notes>();
                conn.Dispose();
                conn.Close();
            }
        }

        public void DeleteNote(int noteId)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new 
                SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.DB_PATH))
            {

                var existingNote = conn.Query<Notes>
                    ("select * from Notes where Id =" + noteId).FirstOrDefault();
                if(existingNote != null)
                {
                    conn.RunInTransaction(() =>
                    {
                        conn.Delete(existingNote);
                    });
                }
            }
        }

    }
}
