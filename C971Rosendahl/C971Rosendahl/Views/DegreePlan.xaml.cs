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
    public partial class DegreePlan : ContentPage
    {
        public DegreePlan()
        {
            InitializeComponent();
        }

        private async void TermEdit_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Term Edit Clicked", "OK");
        }

        private async void CourseView_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Course());
        }

        public async void CourseEdit_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Course Edit Clicked", "OK");
        }

        private async void NewTerm_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "New Term Clicked", "OK");
        }
    }
}