using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DataAccessLibrary.Data;
using DataAccess.Models;
using DataAccess.Data;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uppgift2Database
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ActivePage : Page
    {
        public ViewModels viewModels { get; set; }
        private IEnumerable<Problem> problems { get; set; }

        public ActivePage()
        {
            this.InitializeComponent();
            viewModels = new ViewModels();
            LoadIssuesAsync().GetAwaiter();
            LoadStatus().GetAwaiter();
           
        }

        private async Task LoadIssuesAsync()
        {


          problems = (IEnumerable<Problem>)(LvActiveIssues.ItemsSource = await SqliteAccess.GetAllProblems());
            LoadActiveIssues();



        }
        private async Task LoadStatus()
        {
            cmbStatus.ItemsSource = SettingsContext.GetStatus();
        }

        private async Task LoadComments()
        {
            await SqliteAccess.GetCommentsByProblemId(Convert.ToInt32(id.Text));
        }

       
        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            await SqliteAccess.UpdateProblemStatus(Convert.ToInt32(id.Text), cmbStatus.SelectedItem.ToString());
            await LoadIssuesAsync();
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            await SqliteAccess.CreateCommentAsync(new Comment() { ProblemId= Convert.ToInt32(id.Text), Description = TBComment.Text });
          
        }

        private void LoadActiveIssues()
        {
            LvActiveIssues.ItemsSource = problems
                .Where(i => i.Status != "closed")
                .OrderByDescending(i => i.Created)
                .Take(SettingsContext.GetMaxItemsCount())
                .ToList();
        }
    }
}
   