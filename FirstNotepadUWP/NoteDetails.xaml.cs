using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
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
    public sealed partial class NoteDetails : Page
    {
        private Notes tempNote;
        private DatabaseHelperClass db = new DatabaseHelperClass();

        public NoteDetails()
        {
            this.InitializeComponent();
        }

        private async void NameTxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            await SetNoteStuff();
        }

        private async void ContentTxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            await SetNoteStuff();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            tempNote = (Notes)e.Parameter;
            SetInitialNotes(tempNote);
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                this.Frame.GoBack();
            }
        }

        private async void SetInitialNotes(Notes note)
        {
            byte[] bytes;

            NameTxtBx.Document.SetText(Windows.UI.Text.TextSetOptions.None, note.Title.TrimEnd());
            
            // Getting the stored string (byte array) and putting it back in the array so as to change it to a stream
            bytes = System.Text.Encoding.UTF8.GetBytes(note.Content);

            var buffer = bytes.AsBuffer();

            using (var ras = new InMemoryRandomAccessStream())
            {
                await ras.WriteAsync(buffer);

                ras.Seek(0);

                ContentTxtBx.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, ras);
            }

        }

        private async Task<string> GetContent()
        {
            byte[] bytes;

            using (InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream())
            {
                ContentTxtBx.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, ras);
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

        private async Task<int> SetNoteStuff()
        {
            string name;
            //string content;
            NameTxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out name);
            //ContenttxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
            tempNote.Title = name;
            tempNote.Content = await GetContent();

            db.UpdateDetails(tempNote);
            return 0;
        }
    }
}
