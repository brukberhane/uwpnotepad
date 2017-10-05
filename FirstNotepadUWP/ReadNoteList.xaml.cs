using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class ReadNoteList : Page
    {

        DatabaseHelperClass db;
        Notes note;
        ObservableCollection<Notes> filter;

        public ObservableCollection<Notes> Db_NotesList;

        public ReadNoteList()
        {
            this.InitializeComponent();

            db = new DatabaseHelperClass();
            listBoxobj.DataContext = new Visible() { IsVisible = false };
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReadAllNotesList dbNotes = new ReadAllNotesList();
            Db_NotesList = dbNotes.GetAllNotes();
            if (Db_NotesList.Count == 0)
            {
                //DeleteNoteButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            Db_NotesList.OrderBy(i => i.Id);
            Db_NotesList = new ObservableCollection<Notes>(Db_NotesList.OrderByDescending(i => i.Id));
            listBoxobj.ItemsSource = Db_NotesList;
            txtAutoSuggestBox.ItemsSource = Db_NotesList;
            
        }

        private async void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxobj.SelectedItem != null)
            {
                note = listBoxobj.SelectedItem as Notes;
                //note = Db_NotesList[listBoxobj.SelectedIndex];
                
            } else
            {
                var dag = new MessageDialog("Please select a note to delete");
                await dag.ShowAsync();
                return;
            }
            //Db_NotesList.Remove(note);
            //listBoxobj.Items.RemoveAt(listBoxobj.SelectedIndex);
            db.DeleteNote(note.Id);
            Db_NotesList.Remove(note);
            filter = Db_NotesList;
            //listBoxobj.Items.Remove(listBoxobj.SelectedItem);

        }

        private void txtAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //filter = RefreshList(filter);
        }

        private ObservableCollection<Notes> RefreshList(ObservableCollection<Notes> notes)
        {
            var filtered = (ObservableCollection<Notes>)filter.Where(i => i.Title == txtAutoSuggestBox.Text.Trim());
                txtAutoSuggestBox.ItemsSource = filtered;
            test();
            return filtered;
        }

        private async void test()
        {
            MessageDialog diag = new MessageDialog(txtAutoSuggestBox.Text);
            await diag.ShowAsync();
        }

        private void txtAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            filter = RefreshList(filter);
        }

        private void txtAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }

        private void listBoxobj_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NoteDetails), listBoxobj.SelectedItem);
            MainPage page = this.Frame.Parent as MainPage;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dag = new MessageDialog("you has protec, dun dun dun dun");
            await dag.ShowAsync();
        }

        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            Notes note = (Notes)(sender as Button).DataContext;

            db.DeleteNote(note.Id);
            Db_NotesList.Remove(note);
            filter = Db_NotesList;
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var g = (Grid)sender;
            var s = (StackPanel)g.Children[1];
            var del = (Button)s.Children[1];

            del.Visibility = Visibility.Collapsed;
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var g = (Grid)sender;
            var s = (StackPanel)g.Children[1];
            var del = (Button)s.Children[1];

            del.Visibility = Visibility.Visible;
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var g = (Grid)sender;
            var s = (StackPanel)g.Children[1];
            var del = (Button)s.Children[1];

            del.Visibility = Visibility.Collapsed;
        }
    }

    public class Visible
    {
        public bool IsVisible { get; set; }
    }

}
