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
            if (editNew == false)
            {
                Course course = await DatabaseService.GetSpecificCourse(courseId);
                Instructor currentInstructor = await DatabaseService.GetInstructorById(course.InstructorId);
                

                courseName.Text = course.Name;
                courseDescription.Text = course.Description;
                courseStartDate.Date = course.StartDate.Date;
                courseEndDate.Date = course.EndDate.Date;
                selectedInstructor.SelectedIndex = course.InstructorId;
                instructorEmail.Text = currentInstructor.Email;
                instructorEmail.IsVisible = true;
                instructorPhone.Text = currentInstructor.Phone;
                instructorPhone.IsVisible = true;

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
            if (confirmation == true)
            {
                await Navigation.PopAsync();
            }
        }

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
                    await DatabaseService.AddCourse(course.TermId, course.InstructorId, course.Name, course.StartDate, course.EndDate, course.Description);
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
                //instructorName.IsVisible = false;
                instructorEmail.IsVisible = false;
                instructorPhone.IsVisible = false;
            }
            else
            {
                course.InstructorId = selectedInstructor.SelectedIndex;
                currentInstructor = instructors[selectedInstructor.SelectedIndex - 1];
                //instructorName.Text = currentInstructor.Name;
                //instructorName.IsVisible = true;
                instructorEmail.Text = currentInstructor.Email;
                instructorEmail.IsVisible = true;
                instructorPhone.Text = currentInstructor.Phone;
                instructorPhone.IsVisible = true;
            }
        }
    }
}