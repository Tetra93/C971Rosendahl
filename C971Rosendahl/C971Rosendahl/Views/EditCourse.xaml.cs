using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971Rosendahl;
using C971Rosendahl.Models;
using C971Rosendahl.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Rosendahl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCourse : ContentPage
    {
        public static int courseId;
        public static int termId;
        public static bool editNew = false;
        public Course course = new Course();
        public Instructor currentInstructor = new Instructor();
        public List<Instructor> instructors = new List<Instructor>();
        public List<string> instructorNames = new List<string>();
        public List<Assessment> assessments = new List<Assessment>();
        public Assessment currentAssessment = new Assessment();

        public EditCourse(int id)
        {
            InitializeComponent();
            if (editNew == false)
            {
                courseId = id;
            }

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            instructors.Clear();
            instructors = await DatabaseService.GetInstructor();
            instructorNames.Clear();
            instructorNames.Add("New Instructor");
            foreach (Instructor instructor in instructors)
            {
                instructorNames.Add(instructor.Name);
            }
            selectedInstructor.ItemsSource = instructorNames;
            assessmentsList.Children.Clear();
            assessments.Clear();
            assessments = await DatabaseService.GetAssessment(courseId);
            foreach (Assessment assessment in assessments)
            {
                currentAssessment = assessment;
                AddAssessment();
            }
            if (editNew == false)
            {
                Course course = await DatabaseService.GetSpecificCourse(courseId);
                Instructor currentInstructor = await DatabaseService.GetInstructorById(course.InstructorId);

                //This message displays if an invalid instructor is assigned to the course. This
                //would typically only happen if the instructor data was deleted.

                if (currentInstructor == null)
                {
                    await DisplayAlert("Instructor deleted", "The instructor for this course has been removed. Please select a new instructor", "OK");
                }
                else
                {
                    selectedInstructor.SelectedIndex = course.InstructorId;
                    instructorEmail.Text = currentInstructor.EmailAddress;
                    instructorEmail.IsVisible = true;
                    instructorPhone.Text = currentInstructor.Phone;
                    instructorPhone.IsVisible = true;
                }

                courseName.Text = course.Name;
                courseDescription.Text = course.Description;
                courseStartDate.Date = course.StartDate.Date;
                courseEndDate.Date = course.EndDate.Date;

            }
            else
            {
                Course course = new Course()
                {
                    CourseId = courseId,
                    TermId = termId,
                };
                selectedInstructor.SelectedIndex = 0;
                courseStartDate.Date = DateTime.Now;
                courseEndDate.Date = DateTime.Now.AddDays(30);
                courseStartDate.MinimumDate = DateTime.Now;
                courseEndDate.MinimumDate = DateTime.Now.AddDays(1);

            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlert("Cancel changes?", "Are you sure you wish to go back? Any changes made will be lost", "Yes", "No");
            if (confirmation == true && selectedInstructor.SelectedIndex != -1)
            {
                await Navigation.PopAsync();
            }
            else if (confirmation == true && selectedInstructor.SelectedIndex == -1)
            {
                await Navigation.PopToRootAsync();
            }
        }

        //This verifies that no data blanks are empty. If all blanks are filled
        //with valid data, then data is saved.

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(courseName.Text) && !string.IsNullOrWhiteSpace(courseDescription.Text) && course.InstructorId != -1)
            {
                course.Name = courseName.Text;
                course.Description = courseDescription.Text;
                course.StartDate = courseStartDate.Date; 
                course.EndDate = courseEndDate.Date;
                if (editNew == true)
                {
                    await DatabaseService.AddCourse(termId, course.InstructorId, course.Name, course.StartDate, course.EndDate, course.Description);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DatabaseService.UpdateCourse(courseId, course.InstructorId, course.Name, course.StartDate, course.EndDate, course.Description);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Invalid data", "Please enter a course name and course description and select a course instructor", "OK");
            }
        }

        //These are to prevent invalid start and end dates. If the start date is later than
        //the end date or the end date is earlier than the start date, then the other
        //changes. It changes both the current date and the minimum or maximum date
        //to prevent future invalid date selections.

        private void StartDate_Changed(object sender, EventArgs e)
        {
            if (courseStartDate.Date > courseEndDate.Date)
            {
                courseEndDate.Date = courseStartDate.Date.AddDays(1);
            }
            courseEndDate.MinimumDate = courseStartDate.Date;
            course.StartDate = courseStartDate.Date;

        }

        private void EndDate_Changed(object sender, EventArgs e)
        {
            if (courseEndDate.Date < courseStartDate.Date)
            {
                courseStartDate.Date = courseEndDate.Date.AddDays(-1);
            }
            courseStartDate.MaximumDate = courseEndDate.Date;
            course.EndDate = courseEndDate.Date;

        }

        private void SelectedInstructor_Changed(object sender, EventArgs e)
        {
            if (selectedInstructor.SelectedIndex == 0)
            {
                course.InstructorId = -1;
                instructorEmail.IsVisible = false;
                instructorPhone.IsVisible = false;
            }
            else
            {
                course.InstructorId = selectedInstructor.SelectedIndex;
                currentInstructor = instructors[selectedInstructor.SelectedIndex - 1];
                instructorEmail.Text = currentInstructor.EmailAddress;
                instructorEmail.IsVisible = true;
                instructorPhone.Text = currentInstructor.Phone;
                instructorPhone.IsVisible = true;
            }
        }

        private void AddAssessment()
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
            assessmentsList.Children.Add(frame);
        }

        private async void EditAssessment_Clicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Label label = (Label)sender;
                Grid grid = (Grid)label.Parent;
                Frame frame = (Frame)grid.Parent;
                int id = assessmentsList.Children.IndexOf(frame);
                currentAssessment = assessments[id];
            }
            await Navigation.PushAsync(new EditAssessment(currentAssessment));
        }
    }
}