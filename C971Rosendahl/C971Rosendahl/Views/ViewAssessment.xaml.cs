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
	public partial class ViewAssessment : ContentPage
	{
		public Assessment currentAssessment = new Assessment();

		public ViewAssessment(Assessment assessment)
		{
			InitializeComponent();
			currentAssessment = assessment;
			assessmentName.Text = currentAssessment.Name;
			dueDate.Text = currentAssessment.DueDate.ToString("MM/dd/yyyy");
			description.Text = currentAssessment.Description;
			if (currentAssessment.Type == "Objective Assessment")
			{
				startButton.Text = "Start Exam";
			}
			else if (currentAssessment.Type == "Performance Assessment")
			{
				startButton.Text = "Submit project";
			}
		}
		
		//This is what would "complete" a course if it was a real fully functional app.
		//If it's an objective assessment, it prompts as if you are about to take an exam.
		//If it's a performance assessment, it prompts as if you are submitting a project.
		//"Completing" the project removes notifications for it and displays it as complete, instead.

		private async void Button_Clicked(object sender, EventArgs e)
		{

			if (currentAssessment.CompletionStatus == "Not Completed")
			{
				if (startButton.Text == "Start Exam")
				{
					bool confirmation = await DisplayAlert("Start exam?", "Would you like to start your exam now? It is recommended that you take the exam on a computer.", "Yes", "No");
					if (confirmation)
					{
						await DisplayAlert("Exam started", "There is no actual exam implemented. Click yes or no to complete.", "Yes", "No");
						await DatabaseService.UpdateAssessment(currentAssessment.AssessmentId, false);
						await DatabaseService.UpdateAssessment(currentAssessment.AssessmentId, "Completed");
						await Navigation.PopAsync();
					}
				}

				else if (startButton.Text == "Submit project")
				{
					bool confirmation = await DisplayAlert("Submit project?", "Are you ready to submit your project?", "Yes", "No");
					if (confirmation)
					{
						await DisplayAlert("Project submitted", "Your project has been successfully submitted.", "OK");
						await DatabaseService.UpdateAssessment(currentAssessment.AssessmentId, false);
						await DatabaseService.UpdateAssessment(currentAssessment.AssessmentId, "Completed");
						await Navigation.PopAsync();
					}
				}
			}
			else
			{
				await DisplayAlert("Assessment complete", "This assessment has already been completed", "OK");
				await Navigation.PopAsync();
			}
		}
	}
}
