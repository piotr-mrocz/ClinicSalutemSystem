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

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy ChangeDataDialogWindow.xaml
    /// </summary>
    public partial class ChangeDataDialogWindow : Window
    {
        public ChangeDataDialogWindow(string previousName, string previousSurname)
        {
            InitializeComponent();
            this.newName.Text = previousName;
            this.newSurname.Text = previousSurname;
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (Check_If_Empty() == true)
            {
                MessageBox.Show("Nie podano wszystkich danych", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.Close();
            }
        }

        private bool Check_If_Empty()
        {
            if (String.IsNullOrWhiteSpace(newName.Text) || String.IsNullOrWhiteSpace(newSurname.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
