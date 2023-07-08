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
            Label edit = (Label)sender;
            Grid container = (Grid)edit.Parent;
            int row1 = -1;
            int row2 = -1;
            int column1 = -1;
            int column2 = -1;
            string value = string.Empty;

            foreach (View child in container.Children)
            {
                    row1 = Grid.GetRow(child);
                    column1 = Grid.GetColumn(child);  

                foreach (View child2 in container.Children)
                {
                        row2 = Grid.GetRow(child2);
                        column2 = Grid.GetColumn(child2);                   
                    if (row1 == row2 && column1 == column2)
                    {
                        if (child is Label Child && child2 is Entry Child2)
                        {
                            string childText = Child.Text;
                            Child2.Text = childText;
                            break;
                        }
                    }
                    //else if (row1 == row2 && column1 == column2 && (child2 is DatePicker))
                    //{
                    //    DatePicker Child2 = (DatePicker)child2;
                    //    Child2.Date = 
                    //}
                }
                if (child.IsVisible == true)
                {
                    child.IsVisible = false;
                }
                else if (child.IsVisible == false)
                {
                    child.IsVisible = true;
                }
            }
            
        }

        private async void CourseView_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Course());
        }

        public async void CourseEdit_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Clicked", "Course Edit Clicked", "OK");
            //List<Term> terms = (List<Term>)await DatabaseService.GetTerms();
            //await DisplayAlert("Clicked", $"{terms[0].ToString()}", "OK");
            //term1NameLabel.Text = terms[0].Name;
            //term1NameEntry.Text = terms[0].Name;

            //await DatabaseService.ClearSampleData();

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

        private async void TermSave_Clicked(object sender, EventArgs e)
        {
            Label edit = (Label)sender;
            Grid container = (Grid)edit.Parent;

            foreach (var child in container.Children)
            {
                if (child.IsVisible == true)
                {
                    child.IsVisible = false;
                }
                else if (child.IsVisible == false)
                {
                    child.IsVisible = true;
                }
            }
        }

        private async void TermCancel_Clicked(object sender, EventArgs e)
        {
            Label edit = (Label)sender;
            Grid container = (Grid)edit.Parent;

            foreach (var child in container.Children)
            {
                if (child.IsVisible == true)
                {
                    child.IsVisible = false;
                }
                else if (child.IsVisible == false)
                {
                    child.IsVisible = true;
                }
            }
        }
    }

}

    
