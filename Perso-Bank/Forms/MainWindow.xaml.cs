using Perso_Bank.Forms;
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

namespace Perso_Bank
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
   
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee f = new AddEmployee();
            f.Show();
        }

        private void addAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            createAccount.Show();
        }

        private void editAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addClient_Click(object sender, RoutedEventArgs e)
        {

            Add_Client addClient = new ();
            addClient.Show();
        }

        private void manageExistingAccount_Click(object sender, RoutedEventArgs e)
        {
            ManipulateAccount f = new ManipulateAccount();
            f.Show();

        }
    }
}
