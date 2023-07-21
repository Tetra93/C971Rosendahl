using C971Rosendahl.Models;
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
    public partial class ViewCourse : ContentPage
    {
        public Course course = new Course();
        public Note currentNote = new Note();
        public int maxNoteId = 0;

        public ViewCourse(int id)
        {
            InitializeComponent();
            List<Course> courses = DegreePlan.courses;
            List<Instructor> instructors = DegreePlan.instructors;
            List<Note> notes = new List<Note>();
            List<Assessment> assessments = DegreePlan.assessments;

            foreach (Course searchCourse in courses)
            {
                if (searchCourse.CourseId == id)
                {
                    course = searchCourse;
                }
            }
            foreach (Note note in DegreePlan.notes)
            {
                if (note.NoteId > maxNoteId)
                {
                    maxNoteId = note.NoteId;
                }
                if (note.CourseID == id)
                {
                    notes.Add(note);
                }
            }
            courseName.Text = course.Name;
            courseStartDate.Text = course.StartDate.Date.ToString("MM/dd/yyyy");
            courseEndDate.Text = course.EndDate.Date.ToString("MM/dd/yyyy");
            courseDescription.Text = course.Description;
            instructorInfo.Text = instructors[course.InstructorId - 1].Name;
            foreach (Note note in notes)
            {
                currentNote = note;
                AddNote_Clicked(null, null);
            }
        }

        public async void CourseEdit_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Course Edit Clicked", "OK");
        }

        private async void CourseDateNotifications_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Course Date Notifications Clicked", "OK");
        }

        private async void OANotifications_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "OA Notifications Clicked", "OK");

        }

        private async void EditOA_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Edit OA Clicked", "OK");

        }
        private async void EditPA_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Edit PA Clicked", "OK");

        }

        private async void PANotifications_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "PA Notifications Clicked", "OK");
        }

        private async void Cancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void AddNote_Clicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                await Navigation.PushAsync(new EditNote(maxNoteId + 1));
            }
            //Grid grid = new Grid();
            //Label newNote = new Label()
            //{

            //};

        }
    }
}