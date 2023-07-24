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
        public int id;

        public EditNote(int noteId)
        {
            id = noteId; 
            InitializeComponent();
            if (id == ViewCourse.maxNoteId + 1)
            {
                this.Title = "Add Note";
            }
            else
            {
                this.Title = "Edit Note";
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {

        }

        private void DeleteNote(object sender, EventArgs e)
        {

        }
    }
}