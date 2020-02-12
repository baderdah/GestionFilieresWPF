using ProjetGestionFilieres.Model;
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

namespace ProjetGestionFilieres
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerDB.checkLogin(login.Text, password.Password))
            {
                new Profil().Show();
                this.Close();
            }
            else
            {
                error.Visibility = Visibility.Visible;
            }
            
        }
    }
}
