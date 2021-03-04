using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy ChangeYourPassword.xaml
    /// </summary>
    public partial class ChangeYourPassword : UserControl
    {
        ClinicUsersEntities dbContext = new ClinicUsersEntities();
        string CurrentUser;
        string CurrentUserPassword;


        public ChangeYourPassword(string CurrentUser, string CurrentUserPassword)
        {
            InitializeComponent();
            this.CurrentUser = CurrentUser;
            this.CurrentUserPassword = CurrentUserPassword;
        }


        private void Clear()
        {
            OldPassword.Clear();
            NewPassword.Clear();
            RepeatPassword.Clear();
        }

        private bool CheckOldPasswordIfCorrect()
        {

            if (OldPassword.Password.Equals(CurrentUserPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfNewPasswordIsTheSameAsOld()
        {
            if (NewPassword.Password.Equals(CurrentUserPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckNewPasswordAndRepeatPassword()
        {
            if (NewPassword.Password.Equals(RepeatPassword.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfNotEmpty()
        {
            if (String.IsNullOrEmpty(OldPassword.Password) || String.IsNullOrEmpty(NewPassword.Password) || String.IsNullOrEmpty(RepeatPassword.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfCanChange()
        {
            if (CheckOldPasswordIfCorrect() == true && CheckNewPasswordAndRepeatPassword() == true && CheckIfNewPasswordIsTheSameAsOld() == false)
            {
                MessageBox.Show("Hasło zostało zmienione", "Udało się", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else if (CheckOldPasswordIfCorrect() == false)
            {
                MessageBox.Show("Obecne hasło jest błędne", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Clear();
                return false;
            }
            else if (CheckNewPasswordAndRepeatPassword() == false)
            {
                MessageBox.Show("Podane hasła są różne", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Clear();
                return false;
            }
            else if (CheckIfNewPasswordIsTheSameAsOld() == true)
            {
                MessageBox.Show("Nowe hasło nie może być takie jak obecne", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Clear();
                return false;
            }
            else
            {
                MessageBox.Show("Coś poszło nie tak", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Clear();
                return false;
            }
        }


        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfNotEmpty() == true)
            {
                MessageBox.Show("Nie podano wszystkich danych", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Clear();

            }
            else
            {
                if (CheckIfCanChange() == true)
                {
                    try
                    {
                        var password = dbContext.ClinicUsers.Where(x => x.UserLogin.Equals(CurrentUser)).FirstOrDefault();
                        password.UserPassword = NewPassword.Password;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Coś poszło nie tak", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        dbContext.SaveChanges();
                    }
                }
            }

        }



    }
}
