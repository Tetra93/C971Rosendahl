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
	public partial class EditInstructor : ContentPage
	{
        public static int instructorId;
        public static int courseId;
        public Instructor instructor = new Instructor();
		public EditInstructor ()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            instructor = await DatabaseService.GetInstructorById(instructorId);
            if (instructor != null)
            {
                instructorName.Text = instructor.Name;
                phoneNumber.Text = instructor.Phone;
                emailAddress.Text = instructor.EmailAddress;
            }
        }
        
        //This checks for valid names, emails, and phone numbers. The email
        //is the only one that does any kind of validation other than simply testing
        //for null values.

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(instructorName.Text) &&  !string.IsNullOrWhiteSpace(phoneNumber.Text) && !string.IsNullOrWhiteSpace(emailAddress.Text))
            {
                if (emailAddress.Text.Contains("@") && emailAddress.Text.Contains(".")) 
                {
                    if (instructor == null)
                    {
                        await DatabaseService.AddInstructor(instructorName.Text, phoneNumber.Text, emailAddress.Text);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DatabaseService.UpdateInstructor(instructorId, instructorName.Text, phoneNumber.Text, emailAddress.Text);
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Invalid email address", "Please ensure that the email address is in the format of 'username@server.domain'", "OK");
                }
            }
            else
            {
                await DisplayAlert("Invalid data", "Instructor name, email address, and phone number cannot be empty", "OK");
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlert("Cancel?", "Are you sure you want to go back? Any changes made will be lost", "Yes", "No");
            if (confirmation)
            {
                await Navigation.PopAsync();
            }
        }

        //This deletes the current instructor. If the instructor is assigned to a course,
        //then that course is set to essentially a null instructor. The user will be 
        //prompted to change the instructor next time they view that course.

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlert("Delete Instructor?", "Are you sure you want to delete this instructor data?", "Yes", "No");
            if (confirmation)
            {
                if (instructor != null)
                {
                    foreach (Course course in DegreePlan.courses)
                    {
                        if (course.InstructorId == instructorId)
                        {
                            await DatabaseService.UpdateCourseInstructor(course.CourseId, -1);
                        }
                    }
                    await DatabaseService.RemoveInstructor(instructorId);
                }
                await Navigation.PopAsync();
            }
        }
    }
}