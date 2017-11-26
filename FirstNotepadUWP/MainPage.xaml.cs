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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FirstNotepadUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<Notes> tempNotes;
        DatabaseHelperClass db;

        public MainPage()
        {
            this.InitializeComponent();

            var page = (ReadNoteList)myFrame.Content;
            if (page != null)
            {
                tempNotes = page.Db_NotesList;
            }
            db = new DatabaseHelperClass();
            
        }

        private async void ShowPerson(Notes note)
        {
            MessageDialog message = new MessageDialog(note.Id + " : " + note.Title);
            await message.ShowAsync();
        }

        public void CheckBackButton()
        {
            if (myFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested +=
                    (s, f) =>
                    {
                        if (myFrame.CanGoBack)
                        {
                            myFrame.GoBack();
                        }
                        CheckBackButton();
                    };
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addnote.IsSelected)
            {
                myFrame.Navigate(typeof(AddNote));
                CheckBackButton();
            }
            else if (deleteall.IsSelected)
            {

               if (!myFrame.CanGoBack)
                {
                    var dialog = new MessageDialog("Are you sure you want to delete all your notes?");
                    dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });

                    var result = await dialog.ShowAsync();

                    if (result.Label == "Yes")
                    {
                        db.DeleteAllNotes();
                        var page = (ReadNoteList)myFrame.Content;
                        var notesList= page.Db_NotesList;
                        notesList.Clear();
                    }
                }
                else
                {
                    var dag = new MessageDialog("You must go to the main page to do this");
                    await dag.ShowAsync();
                }
                
                //Probably add something here to clear the observablecollection

            } else if (HamburglerButton.IsSelected)
            {
                mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
                HamburglerButton.IsSelected = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(typeof(ReadNoteList));
        }

        private void myFrame_Navigated(object sender, NavigationEventArgs e)
        {
            CheckBackButton();
            addnote.IsSelected = false;
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                if (myFrame.CanGoBack)
                {
                    myFrame.GoBack();
                }
            } else if (!myFrame.CanGoBack)
            {
                if (IsCtrlPressed())
                {
                    if (e.Key == Windows.System.VirtualKey.A)
                    {
                        myFrame.Navigate(typeof(AddNote));
                        CheckBackButton();
                    }
                }
                
            }
        }

        private static bool IsCtrlPressed()
        {
            var ctrlstate = CoreWindow.GetForCurrentThread().GetKeyState(Windows.System.VirtualKey.Control);
            return ((ctrlstate & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down);
        }
    }
}
