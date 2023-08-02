using C971Rosendahl.Models;
using C971Rosendahl.Services;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace C971Rosendahl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DegreePlan : ContentPage
    {
        public static int termCount;

        public static int courseCount;

        public static int courseTermId;

        public static List<string> status = new List<string> { "Completed", "Started", "Failed", "Dropped", "Not Started" };

        public static List<string> dataOptions = new List<string> { "Clear All Data", "Load Sample Data" };

        public static List<string> instructorList = new List<string>();

        public static List<Term> terms = new List<Term>();

        public static List<Course> courses = new List<Course>();

        public static List<Instructor> instructors = new List<Instructor>();

        public static List<Note> notes = new List<Note>();

        public static List<Assessment> assessments = new List<Assessment>();
        
        public DegreePlan()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Clearing the page of visual elements
            termList.Children.Clear();
            termCount = 0;
            Random random = new Random();
            int notificationNum = random.Next(1000);

            //Loading sample data if this is the first time the app is run
            //Otherwise, loading currently existing data from the database

            if (Settings.FirstRun == true)
            {
                await DatabaseService.LoadSampleData();
            }
            else
            {
                terms = await DatabaseService.GetTerms();
                courses = await DatabaseService.GetCourse();
                instructors = await DatabaseService.GetInstructor();
                notes = await DatabaseService.GetNote();
                assessments = await DatabaseService.GetAssessment();
            }
            instructorList.Clear();
            instructorList.Add("New Instructor");
            foreach (Instructor instructor in instructors)
            {
                instructorList.Add(instructor.Name);
            }
            //Constructing the page. I recycle my NewTerm_Clicked method
            //so that I don't need to create a new method for that.
            foreach (Term term in terms)
            {
                NewTerm_Clicked(null, null);
                termCount++;
            }
            //Displaying any course notifications as well as constructing courses
            //inside of the corresponding terms. I also recycle me CourseAdd_Clicked method.
            for (int i = 0; i <= courses.Count - 1; i++)
            {
                courseCount = i;
                Course course = courses[i];
                if (course.DateNotifications == true && course.EndDate.Date == DateTime.Now.Date)
                {
                    notificationNum = random.Next(1000);
                    CrossLocalNotifications.Current.Show("Course ending soon", $"{course.Name} is ending today", notificationNum);
                }
                courseTermId = course.TermId;
                CourseAdd_Clicked(null, null);
            }
            //Displaying any assessment notifications
            for (int i = 0; i<= assessments.Count - 1; i++)
            {
                Assessment assessment = assessments[i];
                if (assessment.Notifications == true && assessment.DueDate == DateTime.Now.Date) 
                {
                    notificationNum = random.Next(1000);
                    CrossLocalNotifications.Current.Show("Assessment due today", $"{assessment.Name} is due today", notificationNum);
                }
            }
        }


        #region Term methods
        
        //This is my constructor for terms visual elements. I saw that there is a part of
        //Xamarin called Xamarin.Forms.ControlTemplates that sounded like it could allow me
        //to dynamically generate objects, but I wasn't sure if it was allowed. I decided to
        //play it safe and just do it all in C#.
        public async void NewTerm_Clicked(object sender, EventArgs e)
        {
            Term term;
            //This allows the method to be used with the OnAppearing method by
            //using term data pulled from the database
            if (sender == null)
            {
                term = terms[termCount];
            }
            //If not called from OnAppearing, it creates a new term object
            else
            {
                term = new Term
                {
                    Name = "New Term",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(180),
                };
                await DatabaseService.AddTerm(term.Name, term.StartDate, term.EndDate);
            }
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
                Text = term.Name,
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
                Text = term.StartDate.ToString("MM/dd/yy"),
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
                Text = term.EndDate.ToString("MM/dd/yy"),
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

            Entry nameEntry = new Entry()
            {
                Text = string.Empty,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            Grid.SetRow(nameEntry, 0);
            Grid.SetColumn(nameEntry, 0);
            grid.Children.Add(nameEntry);

            DatePicker startDatePicker = new DatePicker()
            {
                Date = term.StartDate.Date,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            Grid.SetRow(startDatePicker, 1);
            Grid.SetColumn(startDatePicker, 0);
            startDatePicker.DateSelected += TermDateChanged;
            grid.Children.Add(startDatePicker);

            DatePicker endDatePicker = new DatePicker()
            {
                Date = term.EndDate.Date,
                MinimumDate = startDatePicker.Date,
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            Grid.SetRow(endDatePicker, 2);
            Grid.SetColumn(endDatePicker, 0);
            endDatePicker.DateSelected += TermDateChanged;
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

        //This method ensures that the term start date is always before the end date
        private void TermDateChanged(object sender, EventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            Grid parent = (Grid)datePicker.Parent;
            DatePicker startDate = (DatePicker)parent.Children[5];
            DatePicker endDate = (DatePicker)parent.Children[6];

            if (sender == startDate)
            {
                endDate.MinimumDate = startDate.Date;

                if (startDate.Date > endDate.Date)
                {
                    endDate.Date = startDate.Date.AddDays(1);
                }
            }
            else if (sender == endDate)
            {
                startDate.MaximumDate = endDate.Date;

                if (endDate.Date < startDate.Date)
                {
                    startDate.Date = endDate.Date.AddDays(-1);
                }
            }
        }

        private void Term_Clicked(object sender, EventArgs e)
        {
            //This hides and shows courses inside a term

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
            //This allows term information to be edited. 
            //Editable views start off as not visible and readonly views begin as visible.
            //This toggles the visibility to show the invisible editable views.

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
                        }
                        else if (child is Grid grid)
                        {
                            Label label = (Label)grid.Children[1];
                            string childText = label.Text;
                            if (child2 is DatePicker datePicker)
                            {
                                datePicker.Date = DateTime.ParseExact(childText, "MM/dd/yy", CultureInfo.InvariantCulture);
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
            //This checks for valid name and dates and saves data if all are valid.
            //It also toggles the visibility back so readonly views are visible again

            Label save = (Label)sender;
            Grid container = (Grid)save.Parent;
            Frame frame = (Frame)container.Parent;
            StackLayout stackLayout = (StackLayout)frame.Parent;
            int row1 = -1;
            int row2 = -1;
            int column1 = -1;
            int column2 = -1;
            Term term = new Term();
            Label nameLabel = new Label();
            Label startDateLabel = new Label();
            Label endDateLabel = new Label();


            term.Id = (termList.Children.IndexOf(stackLayout) + 1);
            Term currentTerm = terms[term.Id - 1];
            bool canSave = true;

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
                            Child.Text = Child2.Text;
                            term.Name = Child2.Text;
                            nameLabel = Child;
                            break;
                        }
                        else if (child is Grid grid)
                        {
                            if (grid.Children[1] is Label startEndDateLabel && grid.Children[0] is Label startEndDateStartText && child2 is DatePicker startEndDatePicker)
                            {
                                if(startEndDateStartText.Text == "Start Date: ")
                                {
                                    term.StartDate = startEndDatePicker.Date;
                                    startDateLabel = startEndDateLabel;
                                }
                                else if (startEndDateStartText.Text == "End Date: ")
                                {
                                    term.EndDate = startEndDatePicker.Date;
                                    endDateLabel = startEndDateLabel;
                                }
                            }                            
                        }
                    }
                }                
            }

            //These alerts "shouldn't" ever trigger, but they're there as a failsafe
            if (term.StartDate.Date > term.EndDate.Date)
            {
                await DisplayAlert("Invalid Dates", "Start date must be before end date", "OK");
                canSave = false;                
            }
            else if (string.IsNullOrWhiteSpace(term.Name))
            {
                await DisplayAlert("Invalid Name", "Please enter a term name", "OK");
                canSave = false;
            }
            else
            {
                canSave = true;
            }
            //If data is all valid, the readonly views are updated to reflect
            //the values from the editable views.
            if (canSave == true)
            {
                await DatabaseService.UpdateTerm(term.Id, term.Name, term.StartDate, term.EndDate);

                nameLabel.Text = term.Name;
                startDateLabel.Text = term.StartDate.ToString("MM/dd/yyyy");
                endDateLabel.Text = term.EndDate.ToString("MM/dd/yyyy");

                //Toggling visibility of all elements to hide editable elements
                //and show readonly ones
                foreach (View view in container.Children)
                {
                    if (view.IsVisible == true)
                    {
                        view.IsVisible = false;
                    }
                    else
                    {
                        view.IsVisible = true;
                    }
                }
            }
            //If data entered is invalid when save is clicked, the data is reverted to
            //its previous state. This is also a failsafe that should never trigger
            else
            {
                nameLabel.Text = currentTerm.Name;
                startDateLabel.Text = currentTerm.StartDate.ToString("MM/dd/yyyy");
                endDateLabel.Text = currentTerm.EndDate.ToString("MM/dd/yyyy");
            }
        }

        //This method toggles the visibility of all the views
        //without changing any data.
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
        
        //This method asks for verification and if yes, the term is deleted.
        //This deletes it from the database as well as deleting any courses
        //contained inside that term.
        private async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            Label delete = (Label)sender;
            Grid container = (Grid)delete.Parent;
            Frame frame = (Frame)container.Parent;
            StackLayout stackLayout = (StackLayout)frame.Parent;
            int ID = termList.Children.IndexOf(stackLayout) + 1;
            bool check = await DisplayAlert("Delete Term?", "All courses inside this term will also be deleted. Are you sure?", "Yes", "No");
            if (check == true)
            {
                await DatabaseService.RemoveTerm(ID);
                termList.Children.Remove(stackLayout);
            }
        }

        #endregion

        #region Course methods

        //Like TermAdd_Clicked, CourseAdd_Clicked dynamically generates a new course
        //visual object. The course is added to the term that the clicked "Add New Course"
        //button is inside of. It is also written to be able to be used in the
        //OnAppearing method when the page is first generated.
        private async void CourseAdd_Clicked(object sender, EventArgs e)
        {
            Course course;
            StackLayout container = new StackLayout();
            if (sender == null)
            {
                course = courses[courseCount];
                container = (StackLayout)termList.Children[course.TermId - 1];
            
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
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };

            //I have the sample courses set to use a 4 character course code
            //like WGU's courses do. I don't really have a way set up to detect
            //formatting since it's not a requirement. If it follows the format
            //I'm using where the course code is at the end after a "-", it will
            //use it. Otherwise it will just use whatever you type in.
            if (course.Name.Length >= 5 && course.Name.Contains("-"))
            {
                courseName.Text = course.Name.Substring(course.Name.Length - 4);
            }
            else
            {
                courseName.Text = course.Name;
            }

            grid.Children.Add(courseName); 
            Label completionStatus = new Label()
            {
                Text = status[course.CompletionStatus],
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End
            };
            Grid.SetColumn(completionStatus, 1);
            grid.Children.Add(completionStatus);
            Picker completionPicker = new Picker()
            {
                ItemsSource = status,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                IsVisible = false
            };
            Grid.SetColumn(completionPicker, 1);
            completionPicker.SelectedIndexChanged += PickerSelection_Clicked;
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
                Text = course.StartDate.ToString("MM/dd/yy"),
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
                Text = course.EndDate.ToString("MM/dd/yy"),
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start
            };
            Grid.SetColumn(courseEndDate1 , 1);
            endDateGrid.Children.Add(courseEndDate1);
            grid.Children.Add(endDateGrid);
            Label editCourse = new Label()
            {
                Text = "Edit",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
            };
            Grid.SetRow(editCourse, 1);
            Grid.SetColumn(editCourse , 1);
            TapGestureRecognizer courseEdit = new TapGestureRecognizer();
            courseEdit.Tapped += CourseEdit_Clicked;
            editCourse.GestureRecognizers.Add(courseEdit);
            grid.Children.Add(editCourse);
            Label deleteCourse = new Label()
            {
                Text = "Delete",
                FontSize = 18,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End
            };
            Grid.SetRow(deleteCourse, 2);
            Grid.SetColumn(deleteCourse , 1);
            TapGestureRecognizer courseDelete = new TapGestureRecognizer();
            courseDelete.Tapped += CourseDelete_Clicked;
            deleteCourse.GestureRecognizers.Add(courseDelete);
            grid.Children.Add(deleteCourse);

            //This hidden Label stores the courseId and is used with my
            //CourseEdit method so that it knows which course to edit.
            Label ID = new Label()
            {
                Text = course.CourseId.ToString(),
                IsVisible = false
            };
            grid.Children.Add(ID);


            frame.Content = grid;
            container.Children.Insert((container.Children.Count() - 1), frame);

            }
            else
            {
                Label button = (Label)sender;
                container = (StackLayout)button.Parent;
                //course = new Course()
                //{
                //    CourseId = courses.Last().CourseId + 1,
                //    Name = "New Course - X000",
                //    Description = "No description",
                //    TermId = termList.Children.IndexOf(container) + 1
                //};
                EditCourse.courseId = courses.Last().CourseId + 1;
                EditCourse.termId = termList.Children.IndexOf(container) + 1;
                EditCourse.editNew = true;
                await Navigation.PushAsync(new EditCourse(-1));
                return;
            }
        }

        //Status Picker for course completion status. It is a Label that, when clicked
        //turns into a Picker.
        private void ChangeStatus_Clicked(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                Grid container = (Grid)label.Parent;
                foreach (View child in container.Children)
                {
                    if (child is Picker picker)
                    {
                        if (Grid.GetRow(picker) == 0 && Grid.GetColumn(picker) == 1)
                        {
                            label.IsVisible = false;
                            picker.SelectedItem = label.Text;
                            picker.IsVisible = true;
                            break;
                        }
                    }
                }
            }
        }

        //When a selection is made, the Picker turns back into a Label with the selected value
        private async void PickerSelection_Clicked(object sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                Grid container = (Grid)picker.Parent;
                Label IdLabel = container.Children.Last() as Label;
                int ID = int.Parse(IdLabel.Text);
                foreach (View child in container.Children)
                {
                    if (child is Label label)
                    {                        
                        if (Grid.GetRow(label) == 0 && Grid.GetColumn(label) == 1)
                        {
                            if (picker.IsVisible == false)
                            {
                                return;
                            }
                            else
                            {
                                picker.IsVisible = false;
                                label.Text = picker.SelectedItem.ToString();
                                label.IsVisible = true;
                                await DatabaseService.UpdateCourse(ID, picker.SelectedIndex);
                                break;
                            }
                        }                        
                    }
                }
            }
        }

        //Opens a new detailed course view page
        private async void CourseView_Clicked(object sender, EventArgs e)
        {
            Grid grid = (Grid)sender;
            Label label = grid.Children.Last() as Label;
            int id = int.Parse(label.Text);
            await Navigation.PushAsync(new ViewCourse(id));
        }

        //Opens a new page to edit a course
        public async void CourseEdit_Clicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Grid grid = (Grid)label.Parent;
            Label idLabel = grid.Children.Last() as Label;
            int id = int.Parse(idLabel.Text);

            EditCourse.editNew = false;
            await Navigation.PushAsync(new EditCourse(id));
        }

        //Asks for verification and deletes the course if yes
        private async void CourseDelete_Clicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Grid grid = (Grid)label.Parent;

            bool confirmation = await DisplayAlert("Delete course?", "Are you sure you want to delete this course?", "Yes", "No");
            if (confirmation == true)
            {
                Label idLabel = grid.Children.Last() as Label;
                int id = int.Parse(idLabel.Text);
                Frame frame = (Frame)grid.Parent;
                StackLayout stackLayout = (StackLayout)frame.Parent;
                stackLayout.Children.Remove(frame);
                await DatabaseService.RemoveCourse(id);
            }
        }

        #endregion

        //This is for the secondary toolbar option to clear the database
        private async void DeleteData(object sender, EventArgs e)
        {
            await DatabaseService.ClearSampleData();
            termList.Children.Clear();
            termCount = 0;
        }

        //This is for the secondary toolbar option to load in sample data
        private async void LoadData(object sender, EventArgs e)
        {
            if (termList.Children.Count == 0)
            { 
            await DatabaseService.LoadSampleData();
            termCount = 0;
            OnAppearing();
            }
        }

        private async void EditInstructor_Clicked(object sender, EventArgs e)
        {
            string selection = await DisplayActionSheet("Select Instructor", "Cancel", null, instructorList.ToArray());
            int index = instructorList.IndexOf(selection);
            if (index == 0)
            {
                EditInstructor.instructorId = instructorList.Count + 1;
            }
            else
            {
                EditInstructor.instructorId = index;
            }
            await Navigation.PushAsync(new EditInstructor());
        }
    }

}

    
