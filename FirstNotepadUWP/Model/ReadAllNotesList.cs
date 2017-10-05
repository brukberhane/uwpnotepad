using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstNotepadUWP
{
    class ReadAllNotesList
    {

        DatabaseHelperClass db = new DatabaseHelperClass();
        public ObservableCollection<Notes> GetAllNotes()
        {
            return db.ReadAllNotes();
        }

    }
}
