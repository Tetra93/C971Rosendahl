using C971Rosendahl.Models;
using C971Rosendahl.Services;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace C971Rosendahl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCourse : ContentPage
    {
        public static int CurrentCourseId { get; set; }
        public Course course = new Course();
        public Note currentNote = new Note();
        public Instructor instructor = new Instructor();
        public Assessment currentAssessment = new Assessment();
        public static int maxNoteId = 0;
        public List<Assessment> assessments = new List<Assessment>();
        public List<Note> notes = new List<Note>();
        public List<string> noteTitles = new List<string>();
        public static readonly List<string> assessmentTypes = new List<string>() { "Objective Assessment", "Performance Assessment" };
        public static readonly List<string> completionStatus = new List<string>() { "Completed", "Not Completed" };

        public ViewCourse(int id)
        {
            CurrentCourseId = id;
            InitializeComponent();
            addAssessment.ItemsSource = assessmentTypes;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            course = await DatabaseService.GetSpecificCourse(CurrentCourseId);            
            instructor = await DatabaseService.GetInstructorById(course.InstructorId);            
            notes.Clear();
            notesList.Children.Clear();
            notes = await DatabaseService.GetNotesById(course.CourseId);
            List<Note> currentNotes = new List<Note>();
            assessments.Clear();
            assessments = await DatabaseService.GetAssessment(course.CourseId);

            foreach (Assessment assessment in assessments)
            {
                currentAssessment = assessment;
                AddAssessment_Clicked(null, null);
            }

            
            courseName.Text = course.Name;
            courseStartDate.Text = course.StartDate.Date.ToString("MM/dd/yy");
            courseEndDate.Text = course.EndDate.Date.ToString("MM/dd/yy");
            courseDescription.Text = course.Description;
            if (instructor != null)
            {
                instructorInfo.Text = instructor.Name;
            }
            noteTitles.Clear();
            foreach (Note note in notes)
            {
                currentNote = note;
                noteTitles.Add(currentNote.Name);
                AddNote_Clicked(null, null);
            }
        }

        public async void CourseEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCourse(course.CourseId));
        }

        //private async void CourseDateNotifications_Clicked(object sender, EventArgs e)
        //{
        //    bool confirmation = await DisplayAlert("Course date alerts", "Would you like notifications regarding the end date of this course?", "Yes", "No");
        //    if (confirmation == true)
        //    {
        //        await DatabaseService.UpdateCourse(course.CourseId, confirmation);
        //        CrossLocalNotifications.Current.Show("Course status", $"{course.Name} is ending today", 5);
        //    }
        //}

        private async void CourseNotifications_Clicked(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlert("Enable notifications?", "Would you like notifications regarding the end date of this course?", "Yes", "No");
            if (confirmation)
            {
                await DatabaseService.UpdateCourse(CurrentCourseId, true);
            }
            else
            {
                await DatabaseService.UpdateCourse(CurrentCourseId, false);
            }
        }

        private async void AddNote_Clicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                await Navigation.PushAsync(new EditNote(null));
            }
            else
            {
                Frame frame = new Frame();
                frame.BorderColor = Color.Gray;
                frame.BackgroundColor = Color.WhiteSmoke;
                StackLayout stackLayout = new StackLayout();
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
                Label share = new Label()
                {
                    Text = "Share",
                    FontSize = 18,
                    VerticalOptions= LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                Grid.SetColumn(share, 1);
                TapGestureRecognizer shareNote = new TapGestureRecognizer();
                shareNote.Tapped += ShareNote_Clicked;
                share.GestureRecognizers.Add(shareNote);
                grid.Children.Add(share);
                stackLayout.Children.Add(grid);
                Label newNoteContents = new Label()
                {
                    Text = currentNote.Contents,
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                stackLayout.Children.Add(newNoteContents);
                Picker moreOptions = new Picker()
                {
                    Title = "More Options",
                    Items = {"Edit Note", "Delete Note"},
                    FontSize = 18,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                moreOptions.SelectedIndexChanged += MoreOptions_SelectedIndexChanged;
                stackLayout.Children.Add(moreOptions);
                frame.Content = stackLayout;
                notesList.Children.Add(frame);
            }
        }

        private async void MoreOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            StackLayout stackLayout = picker.Parent as StackLayout;
            Frame frame = stackLayout.Parent as Frame;
            int noteId = notesList.Children.IndexOf(frame);

            if (picker.SelectedItem == "Edit Note")
            {
                currentNote = notes[noteId];
                await Navigation.PushAsync(new EditNote(currentNote));
            }
            else if (picker.SelectedItem == "Delete Note")
            {
                bool selection = await DisplayAlert("Delete note?", "Are you sure you want to delete this note?", "Yes", "No");
                if (selection == true)
                {
                    notes.RemoveAt(noteId);
                    notesList.Children.RemoveAt(noteId);
                    await DatabaseService.RemoveNote(currentNote.NoteId);
                }
            }
        }

        private async void ShareNote_Clicked(object sender, EventArgs e)
        {
            Label share = (Label)sender;
            Grid grid = (Grid)share.Parent;
            StackLayout stackLayout = (StackLayout)grid.Parent;
            Frame frame = (Frame)stackLayout.Parent;            

            bool selection = await DisplayAlert("Share Note", "Share note?", "Yes", "No");
            if (selection)
            {
                Note currentNote = notes[notesList.Children.IndexOf(frame)];
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = currentNote.Contents,
                    Title = "Share Text"
                });
            }
        }

        private void AddAssessment_Clicked(object sender, EventArgs e)
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
                    HorizontalTextAlignment = TextAlignment.Start,
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
                currentAssessment.Type = selection;
                currentAssessment.Name = "New Assessment";
                EditAssessment_Clicked(null, null);
            }
        }

        private async void EditAssessment_Clicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Label label = (Label)sender;
                Grid grid = (Grid)label.Parent;
                Frame frame = (Frame)grid.Parent;
                int id = assessmentsView.Children.IndexOf(frame);
                currentAssessment = assessments[id];
            }
            await Navigation.PushAsync(new EditAssessment(currentAssessment));
        }

        
        private async void AssessmentNotifications_Clicked(object sender, EventArgs e)
        {
            Label notificationLabel = (Label)sender;
            Grid grid = (Grid)notificationLabel.Parent;
            Frame frame = (Frame)grid.Parent;
            Assessment assessment = assessments[assessmentsView.Children.IndexOf(frame)];


            bool selection = await DisplayAlert("Enable Notifications?", "Would you like notifications regarding the due date of this assessment?", "Yes", "No");
            if (selection)
            {
                await DatabaseService.UpdateAssessment(assessment.AssessmentId, true);
            }
            else
            {
                await DatabaseService.UpdateAssessment(assessment.AssessmentId, false);
            }
        }
    }
}