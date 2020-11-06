using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataAccess.Models;
using DataAccessLibrary.Data;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uppgift2Database
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Closed : Page
    {
        public ViewModels viewModels { get; set; }
        private IEnumerable<Problem> problems { get; set; }

        public Closed()
        {
            this.InitializeComponent();
            viewModels = new ViewModels();
            LoadIssuesAsync().GetAwaiter();
        }

        private async Task LoadIssuesAsync()
        {


            problems = (IEnumerable<Problem>)(lvClosedIssues.ItemsSource = await SqliteAccess.GetAllProblems());
            LoadClosedIssues();

        }

        private void LoadClosedIssues()
        {
            lvClosedIssues.ItemsSource = problems.Where(i => i.Status == "closed").ToList();
        }
    }
}
