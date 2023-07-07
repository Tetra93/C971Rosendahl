using C971Rosendahl.Models;
using C971Rosendahl.Services;
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
            await DatabaseService.ClearSampleData();
        }

        private async void CourseView_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Course());
        }

        public async void CourseEdit_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Course Edit Clicked", "OK");
            List<Term> terms = (List<Term>)await DatabaseService.GetTerms();
            await DisplayAlert("Clicked", $"{terms[0].ToString()}", "OK");
            term1Name.Text = terms[0].Name;
        }

        private async void NewTerm_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "New Term Clicked", "OK");
            StackLayout stackLayout = new StackLayout();
            Frame frame = new Frame
            {
                Margin = new Thickness(0, 10)
            };
            Grid grid = new Grid();
            Label label = new Label()
            {
                Text = "Term " + (termList.Children.Count + 1).ToString(),
                FontSize = 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, 0);
            grid.Children.Add(label);
            Label label1 = new Label()
            {
                Text = DateTime.Now.ToString(),
                FontSize = 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetRow(label1, 1);
            Grid.SetColumn(label1, 0);
            grid.Children.Add(label1);
            Label label2 = new Label()
            {
                Text = DateTime.Now.AddDays(7).ToString(),
                FontSize = 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetRow(label2, 2);
            Grid.SetColumn(label2, 0);
            grid.Children.Add(label2);
            Label label3 = new Label()
            {
                Text = "Edit",
                FontSize = 17,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End                
            };
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TermEdit_Clicked;
            label3.GestureRecognizers.Add(tapGestureRecognizer);
            Grid.SetRow(label3, 1);
            Grid.SetColumn(label3, 1);
            grid.Children.Add(label3);
            frame.Content = grid;
            stackLayout.Children.Add(frame);
            termList.Children.Add(stackLayout);
        }

    }

}

    
