using C971Rosendahl.Models;
using C971Rosendahl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Rosendahl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditNote : ContentPage
    {
        Note currentNote;
        public bool nameCheck = false;
        public bool contentsCheck = false;
        public EditNote(Note note)
        {
            InitializeComponent();
            if (note == null)
            {
                currentNote = new Note();
                this.Title = "Add Note";
            }
            else
            {
                currentNote = note;
                this.Title = "Edit Note";
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (currentNote != null)
            {
                noteName.Text = currentNote.Name;
                noteContents.Text = currentNote.Contents;
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (nameCheck == true && contentsCheck == true)
            {
                if (Title == "Add Note")
                {
                    await DatabaseService.AddNote(ViewCourse.CurrentCourseId, noteName.Text, noteContents.Text);
                    await Navigation.PopAsync();
                }
                else if (Title == "Edit Note")
                {
                    await DatabaseService.UpdateNote(currentNote.NoteId, noteName.Text, noteContents.Text);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Unable to save", "Note name and contents must not be empty", "OK");
            }
        }

        private void NameChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noteName.Text))
            {
                nameCheck = true;
            }
            else
            {
                nameCheck = false;
            }
        }
        private void ContentsChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(noteContents.Text))
            {
                contentsCheck = true;
            }
            else
            {
                contentsCheck = false;
            }
        }
    }
}