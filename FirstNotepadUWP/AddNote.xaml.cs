using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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

        private void NametxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            setNoteStuff();
        }

        private void ContenttxtBx_TextChanged(object sender, RoutedEventArgs e)
        {
            setNoteStuff();
        }

        private void setNoteStuff()
        {
            string name;
            string content;
            NametxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out name);
            ContenttxtBx.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
            tempNote.Title = name;
            tempNote.Content = content;
            db.UpdateDetails(tempNote);
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
