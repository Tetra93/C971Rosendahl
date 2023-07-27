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
            if (assessment.Name == "New Assessment")
            {
                Title = "New Assessment";
            }
            else
            {
                Title = "Edit Assessment";
                nameCheck = true;
                descriptionCheck = true;
            }
            InitializeComponent();
            assessmentName.Text = currentAssessment.Name;
            dueDate.Date = currentAssessment.DueDate.Date;
            assessmentDescription.Text = currentAssessment.Description;
        }
        
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (Title == "New Assessment")
            {
                if (nameCheck == true && descriptionCheck == true)
                {
                    await DatabaseService.AddAssessment(currentAssessment.AssessmentId, currentAssessment.Name, currentAssessment.Type, currentAssessment.Description, currentAssessment.DueDate, false, "Not Submitted", "Not Completed");
                }
            }
            else if (Title == "Edit Assessment")
            {
                if (nameCheck == true && descriptionCheck == true)
                {
                    await DatabaseService.UpdateAssessment(currentAssessment.AssessmentId, currentAssessment.Name, currentAssessment.Description, currentAssessment.DueDate, currentAssessment.Notifications, currentAssessment.SubmissionStatus, currentAssessment.CompletionStatus);
                }
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            bool check = await DisplayAlert("Discard changes?", "Are you sure you want to go back without saving? All changes will be lost.", "Yes", "No");
            if (check == true)
            {
                await Navigation.PopAsync();
            }
        }

        private void OnBlankUnfocus(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                if (!string.IsNullOrWhiteSpace(entry.Text))
                {
                    nameCheck = true;
                }
                else
                {
                    nameCheck = false;
                }
            }
            else if (sender is Editor editor)
            {
                if (!string.IsNullOrWhiteSpace(editor.Text))
                {
                    descriptionCheck = true;
                }
                else
                {
                    descriptionCheck = false;
                }
            }
        }
    }
}