using Newtonsoft.Json;
using Perso_Bank.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace Perso_Bank.Forms
{
    /// <summary>
    /// Interaction logic for ManipulateAccount.xaml
    /// </summary>
    public partial class ManipulateAccount : Window
    {
        public ManipulateAccount()
        {
            InitializeComponent();
        }



        private async void setComboBox() {

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://localhost:7122/api/Trans/getCustomerAccount/{Globals.ID}");


            dd_Account.ItemsSource = null;
            dd_Account.Items.Clear();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add
                (new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            CustomerAccountResponse? customerAccountResponses = new CustomerAccountResponse();
            var res = await httpClient.GetStringAsync($"https://localhost:7122/api/Tran/getCustomerAccount/{Globals.ID}");
            customerAccountResponses = JsonConvert.DeserializeObject<CustomerAccountResponse>(res);
            List<CustomerAccount> customerAccounts = customerAccountResponses.customerAccount;

            foreach (var item in customerAccounts)
            {
                dd_Account.ItemsSource = customerAccounts;
                dd_Account.DisplayMemberPath = "displayName";
                dd_Account.SelectedValuePath = "accountNumber";

            }

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            setComboBox();

        }

        private void opt_Close_Checked(object sender, RoutedEventArgs e)
        {
            txtBox_Amount.IsEnabled = false;
            txtBox_Amount.Text = "0";

        }

        private void opt_Deposit_Checked(object sender, RoutedEventArgs e)
        {

            txtBox_Amount.IsEnabled = true;
            txtBox_Amount.Text = "0";
        }

        private void opt_Withdraw_Checked(object sender, RoutedEventArgs e)
        {
            txtBox_Amount.IsEnabled = true;
            txtBox_Amount.Text = "0";

        }

        private int getSelectedAccount()
        {
            if (dd_Account.SelectedValue == null)
            {
                return 0;
            }

            return (int)dd_Account.SelectedValue;
        }

        private decimal getAmount()
        {
            if (txtBox_Amount.Text == "")
            {
                return 0;
            }
            return decimal.Parse(txtBox_Amount.Text);
        }

        private async void doAction(int? acc, string type, decimal amount)
        {

            HttpClient httpClient = new HttpClient();
            //     httpClient.BaseAddress = new Uri($"https://localhost:7122/api/Trans/getCustomerAccount/{Globals.ID}");

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add
                (new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            CustomerAccountResponse? customerAccountResponses = new CustomerAccountResponse();
            Trans t = new Trans();
            t.accountNumber = acc;
            t.amount = amount;
            t.transType = type;
            var res = await httpClient.PutAsJsonAsync("https://localhost:7122/api/Tran/PerformTransaction", t);
            Response? r = JsonConvert.DeserializeObject<Response>(await res.Content.ReadAsStringAsync());

            if (r.status == 200)
            {

                MessageBox.Show("Transaction Successful");
                setComboBox();

            }
            else
            {
                MessageBox.Show("Transaction Failed:" + r.message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (opt_Deposit.IsChecked == false && opt_Withdraw.IsChecked == false && opt_Close.IsChecked == false)
            {
                MessageBox.Show("Please select an action");
                return;
            }

            if (getSelectedAccount() == 0)
            {
                MessageBox.Show("Please select an account");
                return;
            }

            if (opt_Deposit.IsChecked == true)
            {
                if (getAmount() == 0)
                {
                    MessageBox.Show("Please enter an amount");
                    return;
                }
                else
                {
                    int accountNumber = getSelectedAccount();
                    decimal amount = getAmount();                    
                    doAction(accountNumber, "DEPOSIT", amount);
                }
            }
            else if (opt_Withdraw.IsChecked == true)
            {
                if (getAmount() == 0)
                {
                    MessageBox.Show("Please enter an amount");
                    return;
                }
                else
                {
                    int accountNumber = getSelectedAccount();
                    decimal amount = getAmount();                
                    doAction(accountNumber, "WITHDRAW", amount);
                }
            }
            else if (opt_Close.IsChecked == true)
            {
                int accountNumber = getSelectedAccount();
                decimal amount = 0;                
                doAction(accountNumber, "CLOSE", amount);
            }

        }
    }
}
