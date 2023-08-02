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

        public EditCourse(int id)
        {
            InitializeComponent();
            if (id != -1)
            {
                courseId = id;
            }

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (courseId != -1)
            {
                Course course = await DatabaseService.GetSpecificCourse(courseId);
                courseName.Text = course.Name;
                courseDescription.Text = course.Description;
            }
            else
            {
                Course course = new Course()
                {
                };
            }
        }
        private async void CourseDateNotifications_Clicked(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlert("Notifications", "Would you like notifications regarding the end date of this course?", "Yes", "No");
            if (confirmation)
            {
                await DatabaseService.UpdateCourse(courseId, true);
            }
            else
            {
                await DatabaseService.UpdateCourse(courseId, false);
            }
        }

        private async void OANotifications_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "OA Notifications Clicked", "OK");

        }

        private async void PANotifications_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "PA Notifications Clicked", "OK");
        }
    }
}