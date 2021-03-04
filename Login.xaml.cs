using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {



       readonly ClinicUsersEntities dbContext = new ClinicUsersEntities();
        private UserWindow userWindow;
        private AdminWindow adminWindow;

        public Login()
        {
            InitializeComponent();
            this.MaxHeight = this.MinHeight = this.Height;
            this.MaxWidth = this.MinWidth = this.Width;

        }

       
       

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
           
            var foundInDatabase = dbContext.ClinicUsers.Where(d => d.UserLogin == UserName.Text && d.UserPassword == UserPassword.Password).FirstOrDefault();

            if (foundInDatabase != null)
            {
                var profession = foundInDatabase.UserProfession;

                if (profession.Equals("Administrator"))
                {
                    adminWindow = new AdminWindow(UserName.Text, UserPassword.Password);
                    adminWindow.Show();
                }
                else
                {
                    userWindow = new UserWindow(UserName.Text, UserPassword.Password);
                    userWindow.Show();
                }
                

                 this.Close();
            }
            else
            {
                MessageBox.Show("Nieudana próba logowania!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                UserName.Clear();
                UserPassword.Clear();
            }
            
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                Submit_Click(e, e);
            }

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            
            Application.Current.Shutdown();
        }
    }
}
