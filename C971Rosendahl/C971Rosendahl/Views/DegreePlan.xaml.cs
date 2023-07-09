using C971Rosendahl.Models;
using C971Rosendahl.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public static int objectCount;

        public static List<string> status = new List<string> { "Completed", "Started", "Failed", "Not Started" };

        public DegreePlan()
        {
            InitializeComponent();
            objectCount = termList.Children.Count;
        }

        #region Term methods

        private void NewTerm_Clicked(object sender, EventArgs e)
        {
            StackLayout stackLayout = new StackLayout();
            Frame frame = new Frame
            {
                Margin = new Thickness(0, 10)
            };
            TapGestureRecognizer collapseTerm = new TapGestureRecognizer();
            collapseTerm.Tapped += Term_Clicked;
            frame.GestureRecognizers.Add(collapseTerm);
            Grid grid = new Grid();
            ColumnDefinition column0 = new ColumnDefinition();
            ColumnDefinition column1 = new ColumnDefinition();
            column0.Width = new GridLength(3, GridUnitType.Star);
            column1.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(column0);
            grid.ColumnDefinitions.Add(column1);
            Label termName = new Label()
            {
                Text = "New Term",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetRow(termName, 0);
            Grid.SetColumn(termName, 0);
            grid.Children.Add(termName);
            Grid grid1 = new Grid();
            Grid.SetRow(grid1, 1);
            grid1.HorizontalOptions = LayoutOptions.Start;
            Label termStartDate = new Label()
            {
                Text = "Start Date: ",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            grid1.Children.Add(termStartDate);
            Label termStartDate1 = new Label()
            {
                Text = DateTime.Now.Date.ToString("MM/dd/yyyy"),
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetColumn(termStartDate1, 1);
            grid1.Children.Add(termStartDate1);
            grid.Children.Add(grid1);
            Grid grid2 = new Grid();
            Grid.SetRow(grid2, 2);
            grid2.HorizontalOptions = LayoutOptions.Start;
            Label termEndDate = new Label()
            {
                Text = "End Date: ",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            grid2.Children.Add(termEndDate);
            Label termEndDate1 = new Label()
            {
                Text = DateTime.Now.Date.AddDays(7).ToString("MM/dd/yyyy"),
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetColumn(termEndDate1, 1);
            grid2.Children.Add(termEndDate1);
            grid.Children.Add(grid2);
            Label termEditButton = new Label()
            {
                Text = "Edit",
                FontSize = 18,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End
            };
            TapGestureRecognizer EditTerm = new TapGestureRecognizer();
            EditTerm.Tapped += TermEdit_Clicked;
            termEditButton.GestureRecognizers.Add(EditTerm);
            Grid.SetRow(termEditButton, 0);
            Grid.SetColumn(termEditButton, 1);
            grid.Children.Add(termEditButton);
            Entry entry = new Entry()
            {
                Text = string.Empty,
                FontSize = 18,
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
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            Grid.SetRow(startDatePicker, 1);
            Grid.SetColumn(startDatePicker, 0);
            grid.Children.Add(startDatePicker);
            DatePicker endDatePicker = new DatePicker()
            {
                Date = DateTime.Now.AddDays(14),
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            Grid.SetRow(endDatePicker, 2);
            Grid.SetColumn(endDatePicker, 0);
            grid.Children.Add(endDatePicker);
            Label saveButton = new Label()
            {
                Text = "Save",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };
            TapGestureRecognizer SaveTerm = new TapGestureRecognizer();
            SaveTerm.Tapped += TermSave_Clicked;
            saveButton.GestureRecognizers.Add(SaveTerm);
            Grid.SetRow(saveButton, 0);
            Grid.SetColumn(saveButton, 1);
            grid.Children.Add(saveButton);
            Label cancelButton = new Label()
            {
                Text = "Cancel",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };
            TapGestureRecognizer CancelTermEdit = new TapGestureRecognizer();
            CancelTermEdit.Tapped += TermCancel_Clicked;
            cancelButton.GestureRecognizers.Add(CancelTermEdit);
            Grid.SetRow(cancelButton, 1);
            Grid.SetColumn(cancelButton, 1);
            grid.Children.Add(cancelButton);
            Label deleteButton = new Label()
            {
                Text = "Delete",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };
            TapGestureRecognizer DeleteTerm = new TapGestureRecognizer();
            DeleteTerm.Tapped += DeleteTerm_Clicked;
            deleteButton.GestureRecognizers.Add(DeleteTerm);
            Grid.SetRow(deleteButton, 2);
            Grid.SetColumn(deleteButton, 1);
            grid.Children.Add(deleteButton);
            frame.Content = grid;
            stackLayout.Children.Add(frame);
            termList.Children.Add(stackLayout);
            Frame frame1 = new Frame();
            frame1.Margin = new Thickness(25, 3);
            Label newCourse = new Label()
            {
                Text = "Add New Course",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            TapGestureRecognizer AddCourse = new TapGestureRecognizer();
            AddCourse.Tapped += CourseAdd_Clicked;
            newCourse.GestureRecognizers.Add(AddCourse);
            stackLayout.Children.Add(newCourse);
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

        private void TermEdit_Clicked(object sender, EventArgs e)
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
                        if (child is Label Child)
                        {
                            string childText = Child.Text;

                            if (child2 is Entry entry)
                            {
                                entry.Text = childText;
                                break;
                            }
                            else if (child2 is DatePicker datePicker)
                            {
                                datePicker.Date = DateTime.ParseExact(childText, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                break;

                            }
                        }
                    }
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
                        if (child is Label Child && child2 is Entry entry)
                        {
                            Child.Text = entry.Text;
                            break;
                        }
                        else if (child is Grid grid)
                        {
                            if (grid.Children[1] is Label child3 && child2 is DatePicker datePicker)
                            {

                                child3.Text = datePicker.Date.ToString("MM/dd/yyyy");
                                break;
                            }
                        }
                    }
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

        private void TermCancel_Clicked(object sender, EventArgs e)
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

        private void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            Label delete = (Label)sender;
            Grid container = (Grid)delete.Parent;
            Frame frame = (Frame)container.Parent;
            StackLayout stackLayout = (StackLayout)frame.Parent;
            StackLayout stackLayout1 = (StackLayout)stackLayout.Parent;
            stackLayout1.Children.Remove(stackLayout);
        }

        #endregion

        #region Course methods

        private void CourseAdd_Clicked(object sender, EventArgs e)
        {
            Label button = (Label)sender;
            StackLayout container = (StackLayout)button.Parent;
            Frame frame = new Frame();
            frame.Margin = new Thickness(25, 3);
            Grid grid = new Grid();
            ColumnDefinition column0 = new ColumnDefinition();
            ColumnDefinition column1 = new ColumnDefinition();
            column0.Width = new GridLength(2, GridUnitType.Star);
            column1.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(column0);
            grid.ColumnDefinitions.Add(column1);
            TapGestureRecognizer courseClick = new TapGestureRecognizer();
            courseClick.Tapped += CourseView_Clicked;
            grid.GestureRecognizers.Add(courseClick);
            Label courseName = new Label()
            {
                Text = "New Course",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            grid.Children.Add(courseName); 
            Label completionStatus = new Label()
            {
                Text = "Not Started",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End
            };
            Grid.SetColumn(completionStatus, 1);
            grid.Children.Add(completionStatus);
            Picker completionPicker = new Picker()
            {
                ItemsSource = status,
                IsVisible = false
            };
            Grid.SetColumn(completionPicker, 1);
            TapGestureRecognizer changeStatus = new TapGestureRecognizer();
            changeStatus.Tapped += ChangeStatus_Clicked;
            completionStatus.GestureRecognizers.Add(changeStatus);
            grid.Children.Add(completionPicker);
            Grid startDateGrid = new Grid();
            Grid.SetRow(startDateGrid, 1);
            Label courseStartDate = new Label()
            {
                Text = "Start Date: ",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            startDateGrid.Children.Add(courseStartDate);
            Label courseStartDate1 = new Label()
            {
                Text = DateTime.Now.Date.ToString("MM/dd/yyyy"),
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetColumn(courseStartDate1, 1);
            startDateGrid.Children.Add(courseStartDate1);
            grid.Children.Add(startDateGrid);
            Grid endDateGrid = new Grid();
            Grid.SetRow(endDateGrid, 2);
            Label courseEndDate = new Label()
            {
                Text = "End Date: ",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            endDateGrid.Children.Add(courseEndDate);
            Label courseEndDate1 = new Label()
            {
                Text = DateTime.Now.Date.AddDays(14).ToString("MM/dd/yyyy"),
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetColumn(courseEndDate1 , 1);
            endDateGrid.Children.Add(courseEndDate1);
            grid.Children.Add(endDateGrid);


            frame.Content = grid;
            container.Children.Insert((container.Children.Count() - 1), frame);
        }

        private void ChangeStatus_Clicked(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                Grid container = (Grid)label.Parent;
                foreach (View child in container.Children)
                {
                    //SEARCH FOR PICKER, TOGGLE VISIBILITY, TOGGLE LABEL VISIBILITY
                }
            }
            foreach (View child in container.Children)
            {
                if (child)
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

        #endregion
    }

}

    
