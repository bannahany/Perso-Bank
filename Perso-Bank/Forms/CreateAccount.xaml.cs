using System;
using System.Collections.Generic;
using System.Data;
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
using Perso_Bank.Models;

namespace Perso_Bank.Forms
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            dd_AccountType.Items.Add("Checking");
            dd_AccountType.Items.Add("Savings");
            dd_AccountType.SelectedIndex = 0;
        }


        private List<Clients> getClients(string name) {

            //AuxFunctions.showSuccessDialog(dd_Client.Text); 
            string sql = "SELECT * FROM tbl_Customers WHERE customerFName LIKE '" + name + "%' OR customerLName LIKE '" + name + "%'";

            DataTable? dt = AuxFunctions.executeSQLQuery(sql);
            List<Clients> list = new List<Clients>();
            dt.AsEnumerable().ToList().ForEach(row =>
            {
                list.Add(new Clients
                {
                    ID = row.Field<int>("Id"),
                    firstName = row.Field<string>("customerFName") + " " + row.Field<string>("customerLName")
                });
            });

            return list;
        }


        private void dd_Client_KeyUp(object sender, KeyEventArgs e)
        {

            if ((e.Key >= Key.A && e.Key <= Key.Z))
            {

                List<Clients> list = getClients(dd_Client.Text);

                if (list.Count > 0)
                {
                    dd_Client.ItemsSource = list;
                    dd_Client.DisplayMemberPath = "firstName";
                    dd_Client.SelectedValuePath = "ID";
                    dd_Client.IsDropDownOpen = false;
                    dd_Client.SelectedIndex = 0;
                    return;
                }

            }
            else {
                dd_Client.IsDropDownOpen = false;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int clientID = Convert.ToInt32(dd_Client.SelectedValue);


            int accountType = dd_accountType.SelectedIndex + 1 ;

            accountType = dd_accountType.Text.Equals("Savings") ? 1 : 2;
            
            string accountBalance = txtbox_Deposit.Text;


           DateTime accountStartDate = DateTime.Now;
            accountStartDate.ToString("yyyy-MM-dd");


            if ( dd_AccountType.SelectedIndex == -1 )
            {
                AuxFunctions.showGeneralErrorDialog("Please select an account type");
                return;
                
            }

            if (txtbox_Deposit.Text == "")
            {
                AuxFunctions.showGeneralErrorDialog("Please enter a deposit amount");
                return;
            }

            string sql = "INSERT INTO tbl_Accounts (accountTypeID, accountStartDate, accountEndDate, isActive, accountBalance) VALUES  (" + accountType + ", '" + accountStartDate.ToString("yyyy-MM-dd") + "', NULL, 1, " + accountBalance + ")";

            try
            {
                int x = AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.INSERT);
                string sql2 = "INSERT INTO tbl_brdg_CustomerAccounts (customerID, accountNumber) VALUES (" + clientID + ", " + x + ")";
                int y = AuxFunctions.executeSingleNonQuery(sql2, AuxFunctions.DB_CRUD_TYPE.INSERT_NO_AUTO);
                AuxFunctions.showSuccessDialog("Account created successfully");
            }
            catch (Exception ex)
            {
                AuxFunctions.showGeneralErrorDialog(ex.Message);
            }   



        }
    }
}
