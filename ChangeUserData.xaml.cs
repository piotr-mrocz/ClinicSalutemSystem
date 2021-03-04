using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy ChangeUserData.xaml
    /// </summary>
    public partial class ChangeUserData : UserControl
    {
        readonly ClinicUsersEntities dbContext = new ClinicUsersEntities();
        ClinicUsers clinicUsers = new ClinicUsers();
        CollectionView view;
        PeselValidation peselValidation;
        ChangeDataDialogWindow changeDataDialog;
        ClinicUsers clinic;


        public ChangeUserData()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool PeselValidation()
        {
            peselValidation = new PeselValidation(NewUserPesel.Text);
            bool result = peselValidation.CheckPesel();
            return result;
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            if (Can_Add_New_User() == true)
            {
                clinicUsers.UserName = NewUserName.Text;
                clinicUsers.UserSurname = NewUserSurname.Text;
                clinicUsers.UserPesel = NewUserPesel.Text;
                clinicUsers.UserSex = peselValidation.Gender().ToString();
                clinicUsers.UserProfession = NewUserProfession.Text;
                clinicUsers.UserLogin = AddLogin(NewUserName.Text, NewUserSurname.Text);
                clinicUsers.UserPassword = "sys123";

                dbContext.ClinicUsers.Add(clinicUsers);
                dbContext.SaveChanges();

                MessageBox.Show("Poprawnie dodano użytkownika!", "Dodano użytkownika", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Podano niepoprawne dane. Użytkownik nie został dodany!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string AddLogin(string name, string surname)
        {
            string result = (surname + name.First()).ToLower();

            do
            {

                string Login = result;
                if (Check_If_Login_Is_Database(result) == true)
                {
                    char lastItem = Login.Last();

                    if (Char.IsNumber(lastItem) == true)
                    {
                        string Without_Last_Item = Login.Trim(lastItem);
                        int addNumber = (CharUnicodeInfo.GetDecimalDigitValue(lastItem) + 1);
                        Login = string.Concat(Without_Last_Item, addNumber);

                        result = Login;
                    }
                    else
                    {
                        result = Login + "1";
                    }
                }
                else
                {
                    result = Login;
                }
            } while (Check_If_Login_Is_Database(result) == true);

            return result;
        }

        private bool Check_If_Login_Is_Database(string Login)
        {
            bool result;
            var searchInDatabase = dbContext.ClinicUsers.Where(x => x.UserLogin.Equals(Login)).FirstOrDefault();
            if (searchInDatabase != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool Can_Add_New_User()
        {
            if (Check_If_Empty() == false)
            {
                if (Check_Pesel() == true)
                {
                    if (Check_If_Doctor() == true)
                    {
                        if (Check_If_Spetializatiion_Is_Empty() == false)
                        {
                            ErrorNoSpecialization.Text = null;
                            clinicUsers.UserSpecialization = NewUserSpecialization.Text;
                            return true;
                        }
                        else
                        {
                            ErrorNoSpecialization.Visibility = Visibility.Visible;
                            ErrorNoSpecialization.Text = "Nie wybrano specjalizacji!";
                            return false;
                        }
                    }
                    else
                    {
                        clinicUsers.UserSpecialization = "";
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Check_Pesel()
        {
            if (PeselValidation() == true)
            {
                var foundPesel = dbContext.ClinicUsers.Where(x => x.UserPesel.Equals(NewUserPesel.Text)).FirstOrDefault();
                if (foundPesel != null)
                {
                    MessageBox.Show("Użytkownik o takim numerze PESEL już istnieje!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Podany numer PESEL jest niepoprawny!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool Check_If_Empty()
        {
            if (String.IsNullOrWhiteSpace(NewUserName.Text) || String.IsNullOrWhiteSpace(NewUserSurname.Text) || String.IsNullOrWhiteSpace(NewUserPesel.Text) || NewUserProfession.SelectedItem == default)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Check_If_Spetializatiion_Is_Empty()
        {
            if (NewUserSpecialization.SelectedItem == null)
                return true;
            else
                return false;
        }

        private bool Check_If_Doctor()
        {
            if (NewUserProfession.SelectedIndex == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void VisibilityComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (Check_If_Doctor() == true)
            {
                ComboBoxSpecialization.IsEnabled = true;
                NewUserSpecialization.ToolTip = "Wybierz specjalizację dla lekarza";

            }
            else
            {
                ComboBoxSpecialization.IsEnabled = false;
                NewUserSpecialization.SelectedItem = null;
            }
        }

        private void Load_Data(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource = dbContext.ClinicUsers.ToList());
            view.Filter = Filter;
        }

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                return true;
            }
            else
            {
                return ((item as ClinicUsers).UserName.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as ClinicUsers).UserSurname.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as ClinicUsers).UserProfession.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as ClinicUsers).UserSpecialization.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as ClinicUsers).UserPesel.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource).Refresh();

        }

        private void Load(object sender, MouseEventArgs e)
        {
            Load_Data(e, e);
        }

        private void RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            if (Index_Is_Not_Null() == true)
            {
                var isInDataBase = dbContext.ClinicUsers.Where(x => x.IdUser == clinic.IdUser).FirstOrDefault();
                if (isInDataBase != null)
                {
                    dbContext.ClinicUsers.Remove(isInDataBase);
                    dbContext.SaveChanges();
                    MessageBox.Show("Usunięto użytkownika", "Operacja powiodła się", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load_Data(e, e);
                }
            }
        }

        private bool Index_Is_Not_Null()
        {
            try
            {
                clinic = (ClinicUsers)ListOfUsers.SelectedItems[0];
            }
            catch (Exception)
            {
                MessageBox.Show("Nie wybrano użytkownika", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void ShowDialog()
        {
            if (Index_Is_Not_Null() == true)
            {
                changeDataDialog = new ChangeDataDialogWindow(clinic.UserName, clinic.UserSurname);
                changeDataDialog.ShowDialog();
            }
        }


        private void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (Index_Is_Not_Null() == true)
            {
                ShowDialog();
                var personInDataBase = dbContext.ClinicUsers.Where(x => x.IdUser == clinic.IdUser).FirstOrDefault();
                if (personInDataBase != null)
                {
                    personInDataBase.UserName = changeDataDialog.newName.Text;
                    personInDataBase.UserSurname = changeDataDialog.newSurname.Text;
                    personInDataBase.UserLogin = String.Empty;
                    dbContext.SaveChanges();
                    personInDataBase.UserLogin = AddLogin(changeDataDialog.newName.Text, changeDataDialog.newSurname.Text);
                    dbContext.SaveChanges();
                    MessageBox.Show("Zmieniono dane użytkownika", "Operacja powiodła się", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load_Data(e, e);
                }
                else
                {
                    MessageBox.Show("Coś poszło nie tak. Nie można zmienić danych użytkownika", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

    }
}
