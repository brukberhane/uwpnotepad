using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FirstNotepadUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddNote : Page
    {
        Notes tempNote;
        DatabaseHelperClass db = new DatabaseHelperClass();
        
        
        public AddNote()
        {
            this.InitializeComponent();

            
            tempNote = new Notes("", "");
            db.Insert(tempNote);
        }

        private  async void NametxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            await SetNoteStuff();
        }

        private async void ContenttxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            await SetNoteStuff();
        }

        private async Task<int> SetNoteStuff()
        {
            string name;
            //string content;
            NametxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out name);
            //ContenttxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
            tempNote.Title = name;
            tempNote.Content = await GetContent();

            db.UpdateDetails(tempNote);
            return 0;
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                string name;
                string content;
                NametxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out name);
                ContenttxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
                //db.Insert(new Notes(name, content));
                this.Frame.GoBack();

                e.Handled = true;
            }
            
        }

        private async Task<string> GetContent()
        {
            byte[] bytes;

            using (InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream())
            {
                ContenttxtBx.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, ras);
                //MemoryStream stream = (MemoryStream)ras.AsStreamForRead();

                ras.Seek(0);
                var dataReader = new DataReader(ras);
                await dataReader.LoadAsync((uint)ras.Size);
                bytes = new byte[ras.Size];
                dataReader.ReadBytes(bytes);

                return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
               
            }

            return null;
        }

    }
}



/*
 * FILE = await folder.CreateFileAsync("test.bruk", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            FILE_BACKUP = await folder.CreateFileAsync("test1.bruk", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            try
            {
                var stream = await FILE.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                ContenttxtBx.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, stream);
            } catch (IOException ioe)
            {
                Debug.WriteLine("There was a problem with the file in AddNote.xaml.cs, " + ioe.Message);
                var stream = await FILE_BACKUP.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                ContenttxtBx.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, stream);
            }
            */

/*
        byte[] bytes;
        using (var memory = new InMemoryRandomAccessStream())
        {
            // Save the content: in this case, persistent the content.
            ContenttxtBx.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, memory);
            var streamToSave = memory.AsStreamForRead();
            var dataReader = new DataReader(streamToSave.AsInputStream());
            bytes = new byte[streamToSave.Length];
            await dataReader.LoadAsync((uint)streamToSave.Length);
            dataReader.ReadBytes(bytes);
            StreamReader reader = new StreamReader(streamToSave);
            string temp = reader.ReadToEnd();
            Debug.WriteLine("AddNote.xaml.cs: "+temp);
        }*/
