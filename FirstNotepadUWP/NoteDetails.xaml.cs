using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        private void NameTxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            setNoteStuff();
        }

        private void ContentTxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            setNoteStuff();
        }

        private void setNoteStuff()
        {
            string name;
            string content;
            NameTxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out name);
            ContentTxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
            tempNote.Content = content;
            tempNote.Title = name;
            db.UpdateDetails(tempNote);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            tempNote = (Notes)e.Parameter;
            NameTxtBx.Document.SetText(Windows.UI.Text.TextSetOptions.None, tempNote.Title.TrimEnd());
            //Load(tempNote.Content);
            ContentTxtBx.Document.SetText(Windows.UI.Text.TextSetOptions.None, tempNote.Content.TrimEnd());
        }

        private async void Load(string text)
        {
            using (var memory = new InMemoryRandomAccessStream())
            {
                var dataWriter = new DataWriter(memory);

                dataWriter.WriteString(text);

                await dataWriter.StoreAsync();

                Debug.WriteLine("NoteDetails.xaml.cs: " + text);

                ContentTxtBx.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, memory);
            }
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                this.Frame.GoBack();
            }
        }
    }
}
