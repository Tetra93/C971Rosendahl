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
        public ViewCourse()
        {
            InitializeComponent();
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
    }
}