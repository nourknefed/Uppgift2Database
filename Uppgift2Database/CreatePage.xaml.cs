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
using DataAccessLibrary.Data;
using DataAccess.Models;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uppgift2Database
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePage : Page
    {
        public CreatePage()
        {
            this.InitializeComponent();
            LoadCustomersAsync().GetAwaiter();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            await SqliteAccess.CreateCustomerAsync(new Customer { Name = TBName.Text});
           await LoadCustomersAsync();

        }
        private async Task LoadCustomersAsync()
        {
            cmbCustomers.ItemsSource = await SqliteAccess.GetCustomerNames();
        }

        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            await SqliteAccess.CreateProblemAsync(new Problem
            {
                Category = TBTitle.Text,
                Description = TBDescription.Text,
                CustomerId = (int)await SqliteAccess.GetCustomerIdByName(cmbCustomers.SelectedItem.ToString())
            });
        }
    }
}
