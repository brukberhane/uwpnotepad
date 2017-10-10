using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        Windows.Storage.Streams.IRandomAccessStream mStream;
        StorageFile FILE, FILE_BACKUP;
        StorageFolder folder = ApplicationData.Current.LocalFolder;
        
        
        public AddNote()
        {
            this.InitializeComponent();

            
            tempNote = new Notes("", "");
            db.Insert(tempNote);
        }

        private  async void NametxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            setNoteStuff();
        }

        private void ContenttxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            //setNoteStuff();
        }

        private async void setNoteStuff()
        {
            string name;
            string content;
            NametxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out name);
            ContenttxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
            tempNote.Title = name;
            tempNote.Content = content;
            db.UpdateDetails(tempNote);

            /*using (var stream = new InMemoryRandomAccessStream())
            {

                tempNote.Title = name;
                ContenttxtBx.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, stream);

                using (var inputStream = stream.GetInputStreamAt(0))
                {
                    using (var dataReader = new DataReader(inputStream))
                    {
                        await dataReader.LoadAsync((uint)stream.Size);

                        var recievedStrings = dataReader.ReadString((uint)stream.Size);
                        tempNote.Content = recievedStrings;

                        Debug.WriteLine("AddNote.xaml.cs: " + recievedStrings);
                    }
                }

                db.UpdateDetails(tempNote);

            }*/

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
