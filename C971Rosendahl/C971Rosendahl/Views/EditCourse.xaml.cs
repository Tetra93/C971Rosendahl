using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971Rosendahl;
using C971Rosendahl.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971Rosendahl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCourse : ContentPage
    {
        public Course currentCourse;

        public EditCourse(Course course)
        {
            InitializeComponent();
            currentCourse = course;

            courseName.Text = currentCourse.Name;
        }
        private async void CourseDateNotifications_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Course Date Notifications Clicked", "OK");
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