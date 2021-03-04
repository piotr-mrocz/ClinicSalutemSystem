using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClinicSalutemSystem
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Make_First_Letter_Upper(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.Length > 0)
            {
                tb.Text = Char.ToUpper(tb.Text[0]) + tb.Text.Substring(1);
            }
        }

        private void Letter_Validation_TextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-ź]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
