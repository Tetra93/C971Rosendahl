using C971Rosendahl.Models;
using C971Rosendahl.Services;
using Plugin.LocalNotifications;
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
        public Assessment currentAssessment = new Assessment();
        public static int maxNoteId = 0;
        public static readonly List<string> assessmentTypes = new List<string>() { "Objective Assessment", "Performance Assessment"};
        public static readonly List<string> completionStatus = new List<string>() { "Completed", "Not Completed" };
        
        public ViewCourse(int id)
        {
            InitializeComponent();
            addAssessment.ItemsSource = assessmentTypes;
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
            foreach (Assessment assessment in assessments)
            {
                if (assessment.AssessmentId == id)
                {
                    currentAssessment = assessment;
                    AddAssessment_Clicked(null, null);
                }
            }
            courseName.Text = course.Name;
            courseStartDate.Text = course.StartDate.Date.ToString("MM/dd/yy");
            courseEndDate.Text = course.EndDate.Date.ToString("MM/dd/yy");
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
            bool confirmation = await DisplayAlert("Course date alerts", "Would you like notifications regarding the end date of this course?", "No", "Yes");
            if (confirmation == true)
            {
                await DatabaseService.UpdateCourse(course.CourseId, confirmation);
                CrossLocalNotifications.Current.Show("Course status", $"{course.Name} is ending today", 5);
            }
        }


        private async void AddNote_Clicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                await Navigation.PushAsync(new EditNote(maxNoteId + 1));
            }
            else
            {
                Frame frame = new Frame();
                frame.BorderColor = Color.Gray;
                frame.BackgroundColor = Color.WhiteSmoke;
                Grid grid = new Grid();
                Label newNoteName = new Label()
                {
                    Text = currentNote.Name,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                grid.Children.Add(newNoteName);
                Label newNoteContents = new Label()
                {
                    Text = currentNote.Contents,
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                Grid.SetRow(newNoteContents, 1);
                grid.Children.Add(newNoteContents);
                frame.Content = grid;
                notesList.Children.Add(frame);
            }
        }

        private async void ShareNote_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Share Note", "Share note?", "No", "Yes");
        }

        private async void AddAssessment_Clicked(object sender, EventArgs e)
        {
            if (sender == null)
            {
                Frame frame = new Frame();
                frame.Margin = new Thickness(0, 10);
                Grid grid = new Grid();
                ColumnDefinition column0 = new ColumnDefinition();
                ColumnDefinition column1 = new ColumnDefinition();
                column0.Width = new GridLength(2, GridUnitType.Star);
                column1.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(column0);
                grid.ColumnDefinitions.Add(column1);
                Label name = new Label
                {
                    Text = currentAssessment.Name,
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                };

                grid.Children.Add(name);

                Label notifications = new Label
                {
                    Text = "Notifications",
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.End,
                };
                Grid.SetColumn(notifications, 1);
                TapGestureRecognizer courseNotifications = new TapGestureRecognizer();
                courseNotifications.Tapped += CourseNotifications_Clicked;
                notifications.GestureRecognizers.Add(courseNotifications);
                grid.Children.Add(notifications);

                Grid startDateGrid = new Grid();
                Grid.SetRow(startDateGrid, 1);
                Label startDateLabel1 = new Label
                {
                    Text = "Due Date: ",
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                };
                startDateGrid.Children.Add(startDateLabel1);

                Label startDateLabel2 = new Label
                {
                    Text = currentAssessment.DueDate.Date.ToString("MM/dd/yy"),
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Start,
                };
                Grid.SetColumn(startDateLabel2, 1);
                startDateGrid.Children.Add(startDateLabel2);
                grid.Children.Add(startDateGrid);

                Label edit = new Label
                {
                    Text = "Edit",
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.End,
                };
                Grid.SetColumn(edit, 1);
                Grid.SetRow(edit, 1);
                TapGestureRecognizer editAssessment = new TapGestureRecognizer();
                editAssessment.Tapped += EditAssessment_Clicked;
                edit.GestureRecognizers.Add(editAssessment);
                grid.Children.Add(edit);
                frame.Content = grid;
                assessmentsView.Children.Add(frame);
            }
            else
            {
                string selection = addAssessment.SelectedItem.ToString();
                if (selection != null || selection != string.Empty)
                {
                    await Navigation.PushAsync(new EditAssessment());
                }
            }
        }

        private void EditAssessment_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CourseNotifications_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

    }
}