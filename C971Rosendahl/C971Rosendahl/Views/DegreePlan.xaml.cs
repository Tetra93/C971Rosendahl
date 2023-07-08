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

        private void Term_Clicked(object sender, EventArgs e)
        {
            Frame term = (Frame)sender;
            StackLayout container = (StackLayout)term.Parent;
            foreach (View child in container.Children)
            {
                if (child != term)
                {
                    if (child.IsVisible)
                    {
                        child.IsVisible = false;
                    }
                    else if (!child.IsVisible)
                    {
                        child.IsVisible = true;
                    }
                }
            }
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
            StackLayout stackLayout = new StackLayout();
            Frame frame = new Frame
            {
                Margin = new Thickness(0, 10)
            };
            Grid grid = new Grid();
            Label termName = new Label()
            {
                Text = "Term " + (termList.Children.Count + 1).ToString(),
                FontSize = 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetRow(termName, 0);
            Grid.SetColumn(termName, 0);
            grid.Children.Add(termName);
            Label termStartDate = new Label()
            {
                Text = DateTime.Now.ToString(),
                FontSize = 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetRow(termStartDate, 1);
            Grid.SetColumn(termStartDate, 0);
            grid.Children.Add(termStartDate);
            Label termEndDate = new Label()
            {
                Text = DateTime.Now.AddDays(7).ToString(),
                FontSize = 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetRow(termEndDate, 2);
            Grid.SetColumn(termEndDate, 0);
            grid.Children.Add(termEndDate);
            Label termEditButton = new Label()
            {
                Text = "Edit",
                FontSize = 17,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End                
            };
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TermEdit_Clicked;
            termEditButton.GestureRecognizers.Add(tapGestureRecognizer);
            Grid.SetRow(termEditButton, 1);
            Grid.SetColumn(termEditButton, 1);
            grid.Children.Add(termEditButton);
            Entry entry = new Entry()
            {
                Text = string.Empty,
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            Grid.SetRow(entry, 0);
            Grid.SetColumn(entry, 0);
            grid.Children.Add(entry);
            DatePicker startDatePicker = new DatePicker()
            {
                Date = DateTime.Now,
                FontSize= 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions= LayoutOptions.Start,
                IsVisible= false
            };
            Grid.SetRow(startDatePicker, 1);
            Grid.SetColumn(startDatePicker, 0);
            grid.Children.Add(startDatePicker);
            DatePicker endDatePicker = new DatePicker()
            {
                Date = DateTime.Now.AddDays(14),
                FontSize= 17,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions= LayoutOptions.Start,
                IsVisible= false
            };
            Grid.SetRow(endDatePicker, 2);
            Grid.SetColumn(endDatePicker , 0);
            grid.Children.Add(endDatePicker);
            Label saveButton = new Label()
            {
                Text = "Save",
                FontSize = 17,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };
            TapGestureRecognizer tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += TermSave_Clicked;
            saveButton.GestureRecognizers.Add(tapGestureRecognizer1);
            Grid.SetRow(saveButton , 1);
            Grid.SetColumn (saveButton , 1);
            grid.Children.Add (saveButton);
            Label cancelButton = new Label()
            {
                Text = "Cancel",
                FontSize = 17,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };
            TapGestureRecognizer tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += TermCancel_Clicked;
            cancelButton.GestureRecognizers.Add(tapGestureRecognizer2);
            Grid.SetRow(cancelButton , 2);
            Grid.SetColumn (cancelButton , 1);
            grid.Children.Add(cancelButton);
            frame.Content = grid;
            stackLayout.Children.Add(frame);
            termList.Children.Add(stackLayout);
        }

        private async void TermSave_Clicked(object sender, EventArgs e)
        {
            Label save = (Label)sender;
            Grid container = (Grid)save.Parent;
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
                            string child2Text = Child2.Text;
                            Child.Text = child2Text;
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

    
