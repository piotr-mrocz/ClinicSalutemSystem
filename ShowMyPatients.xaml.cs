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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy ShowMyPatients.xaml
    /// </summary>
    public partial class ShowMyPatients : UserControl
    {

        readonly ClinicUsersEntities dbContext = new ClinicUsersEntities();
        CollectionView view;
        string CurrentUser;
        
        public ShowMyPatients(string CurrentUser)
        {
            InitializeComponent();
            this.CurrentUser = CurrentUser;
        }

        private void Load_Data(object sender, RoutedEventArgs e)
        {
            var dataFromDatabase = (from patients in dbContext.Patients
                                    join nurse in dbContext.ClinicUsers
                                    on patients.IdUserNurse equals nurse.IdUser
                                    join doctor in dbContext.ClinicUsers
                                    on patients.IdUserDoctor equals doctor.IdUser
                                    where nurse.UserLogin.Equals(this.CurrentUser) || doctor.UserLogin.Equals(this.CurrentUser)
                                    orderby patients.PatientVisitDay
                                    select patients).ToList();



            var checkIfAdmin = (from profession in dbContext.ClinicUsers
                                where profession.UserLogin.Equals(this.CurrentUser) && profession.UserProfession.Equals("Administrator")
                                select profession.UserProfession).FirstOrDefault();

            if (checkIfAdmin != null)
            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfPatients.ItemsSource = dbContext.Patients.OrderBy(x => x.PatientVisitDay).ToList());
            }
            else
            {
                view = (CollectionView)CollectionViewSource.GetDefaultView(ListOfPatients.ItemsSource = dataFromDatabase);
            }
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
                return ((item as Patients).PatientName.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as Patients).PatientSurname.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as Patients).PatientVisitDay.ToString().IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
             CollectionViewSource.GetDefaultView(ListOfPatients.ItemsSource).Refresh();
        }

        private void Load(object sender, MouseEventArgs e)
        {
            Load_Data(e, e);
        }
    }
}
