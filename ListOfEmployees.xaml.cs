using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy ListOfEmployees.xaml
    /// </summary>
    public partial class ListOfEmployees : UserControl
    {
        
        readonly ClinicUsersEntities dbContext = new ClinicUsersEntities();
        CollectionView view;
        public ListOfEmployees()
        {
            InitializeComponent();
        }

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                return true;
            }
            else
            {
                return ((item as ClinicUsers).UserName.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as ClinicUsers).UserSurname.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as ClinicUsers).UserProfession.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as ClinicUsers).UserSpecialization.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            }
        }

        private void Load_Data(object sender, RoutedEventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource = dbContext.ClinicUsers.ToList());
            view.Filter = Filter;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource).Refresh();
        }

        private void Load(object sender, MouseEventArgs e)
        {
            Load_Data(e, e);
        }

        private void PrintAllUser(object sender, EventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource = dbContext.ClinicUsers.ToList());
            view.Filter = Filter;
        }

        private void PrintOnlyDoctors(object sender, EventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource = dbContext.ClinicUsers.Where(x => x.UserProfession.Equals("Lekarz")).ToList());
            view.Filter = Filter;
        }
        private void PrintOnlyNurses(object sender, EventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource = dbContext.ClinicUsers.Where(x => x.UserProfession.Equals("Pielegniarka")).ToList());
            view.Filter = Filter;
        }

        private void PrintOnlyAdmins(object sender, EventArgs e)
        {
            view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfUsers.ItemsSource = dbContext.ClinicUsers.Where(x => x.UserProfession.Equals("Administrator")).ToList());
            view.Filter = Filter;
        }
    }
}
