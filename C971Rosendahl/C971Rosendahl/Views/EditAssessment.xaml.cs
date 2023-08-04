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
    public partial class EditAssessment : ContentPage
    {
        Assessment currentAssessment = new Assessment();
        public bool nameCheck = false;
        public bool descriptionCheck = false;
        

        public EditAssessment(Assessment assessment)
        {
            currentAssessment = assessment;
            if (assessment.Name == string.Empty)
            {
                Title = "New Assessment";
                nameCheck = false;
                descriptionCheck = false;
            }
            else
            {
                Title = "Edit Assessment";
                nameCheck = true;
                descriptionCheck = true;
            }
            InitializeComponent();
            dueDate.MinimumDate = DateTime.Now;
            assessmentName.Text = currentAssessment.Name;
            if (Title == "Edit Assessment")
            {
                assessmentDescription.Text = currentAssessment.Description;
            }
            if (currentAssessment.DueDate.Date < dueDate.MinimumDate)
            {
                currentAssessment.DueDate = dueDate.MinimumDate.Date;
            }
            else
            {
                dueDate.Date = currentAssessment.DueDate.Date;
            }
        }
        
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            assessmentName.Unfocus();
            assessmentDescription.Unfocus();

            if (Title == "New Assessment")
            {
                if (nameCheck == true && descriptionCheck == true)
                {
                    await DatabaseService.AddAssessment(currentAssessment.CourseId, currentAssessment.Name, currentAssessment.Type, currentAssessment.Description, currentAssessment.DueDate, false, "Not Completed");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Invalid Data", "Please enter a valid name and description", "OK");
                }
            }
            else if (Title == "Edit Assessment")
            {
                if (nameCheck == true && descriptionCheck == true)
                {
                    await DatabaseService.UpdateAssessment(currentAssessment.AssessmentId, currentAssessment.Name, currentAssessment.Description, currentAssessment.DueDate.Date, currentAssessment.CompletionStatus);
                    await Navigation.PopAsync();
                }
            }
        }

        private void DateChanged(object sender, EventArgs e)
        {
            currentAssessment.DueDate = dueDate.Date;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            bool check = await DisplayAlert("Discard changes?", "Are you sure you want to go back? Any changes made will be lost.", "Yes", "No");
            if (check == true)
            {
                await Navigation.PopAsync();
            }
        }
        //This checks to make sure my blanks are not empty. If they are, it toggles
        //off a flag that will not allow saving.

        private void OnBlankUnfocus(object sender, FocusEventArgs e)
        {
            if (sender is Editor editor)
            {
                if (editor == assessmentName)
                {
                    if (!string.IsNullOrWhiteSpace(assessmentName.Text))
                    {
                        nameCheck = true;
                        currentAssessment.Name = assessmentName.Text;
                    }
                    else
                    {
                        nameCheck = false;
                    }
                }
                if (editor == assessmentDescription)
                {
                    if (!string.IsNullOrWhiteSpace(assessmentDescription.Text))
                    {
                        descriptionCheck = true;
                        currentAssessment.Description = assessmentDescription.Text;
                    }
                    else
                    {
                        descriptionCheck = false;
                    }
                }
            }
        }

        //This deletes the current assessment. If it is an existing assessment,
        //it is deleted and you go to the previous page. If it is not,
        //then it just takes you to the previous page

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlert("Delete Assessment?", "Are you sure you would like to delete this assessment?", "Yes", "No");
            if (confirmation == true)
            {
                if (Title == "Edit Assessment")
                {
                await DatabaseService.RemoveAssessment(currentAssessment.AssessmentId);
                await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
        }
    }
}