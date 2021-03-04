using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        private ListOfEmployees EmployeeList = new ListOfEmployees();
        private ShowMyPatients showPatients;
        ChangeYourPassword ChangePassword;
        ChangeUserData changeUserData;
        ChangeVisit changeVisit;

        private string CurrentUser;
        private string CurrentUserPassword;
        public AdminWindow(string UserName, string UserPassword)
        {
            InitializeComponent();
            this.MaxHeight = this.MinHeight = this.Height;
            this.MaxWidth = this.MinWidth = this.Width;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += dataTimer;
            timer.Start();

            this.CurrentUser = UserName;
            this.CurrentUserPassword = UserPassword;
        }

        private void dataTimer(object sender, EventArgs e)
        {
            actualTime.Content = DateTime.Now.ToLocalTime();
        }
        private void ListOfEmployees(object sender, RoutedEventArgs e)
        {
            MainContent.Content = EmployeeList;
        }

        private void ListOfPatients(object sender, RoutedEventArgs e)
        {
            showPatients = new ShowMyPatients(this.CurrentUser);
            MainContent.Content = showPatients;
        }

        private void ChangeYourPassword(object sender, RoutedEventArgs e)
        {
            ChangePassword = new ChangeYourPassword(this.CurrentUser, this.CurrentUserPassword);
            MainContent.Content = ChangePassword;
        }

        private void ChangeUsers(object sender, RoutedEventArgs e)
        {
            changeUserData = new ChangeUserData();
            MainContent.Content = changeUserData;
        }

        private void Visit(object sender, RoutedEventArgs e)
        {
            changeVisit = new ChangeVisit();
            MainContent.Content = changeVisit;
        }

        private void Exit(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
        }
    }
}
