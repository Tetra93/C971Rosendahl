using C971Rosendahl.Models;
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

		public ViewAssessment (Assessment assessment)
		{
			InitializeComponent ();
		}
	}
}