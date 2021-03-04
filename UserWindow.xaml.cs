using System;
using System.Windows;
using System.Windows.Threading;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        
        DispatcherTimer timer = new DispatcherTimer();
        ListOfEmployees EmployeeList = new ListOfEmployees();
        ShowMyPatients showMyPatients;

        ChangeYourPassword ChangePassword;
        private string CurrentUser; 
        private string CurrentUserPassword; 

        public UserWindow(string UserName, string UserPassword)
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

        private void ListOfMyPatients(object sender, RoutedEventArgs e)
        {
            showMyPatients = new ShowMyPatients(this.CurrentUser);
            MainContent.Content = showMyPatients;
        }

        private void ChangeYourPassword(object sender, RoutedEventArgs e)
        {
            ChangePassword = new ChangeYourPassword(this.CurrentUser, this.CurrentUserPassword); 
            MainContent.Content = ChangePassword; 
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
        }


    }
}
