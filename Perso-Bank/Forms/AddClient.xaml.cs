using Newtonsoft.Json.Linq;
using Perso_Bank.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Perso_Bank.Forms
{
    /// <summary>
    /// Interaction logic for Add_Client.xaml
    /// </summary>
    public partial class Add_Client : Window
    {
        public Add_Client()
        {
            InitializeComponent();
        }

        private List<Clients> clients = new List<Clients>();

        private List<Clients> getClientList ()
        {
            DataTable? dt = AuxFunctions.executeSQLQuery("select * from tbl_Customers");
            foreach (DataRow row in dt.Rows)
            {
                clients.Add(new Clients()
                {
                    ID = (int)row["ID"],
                    firstName = (string)row["customerFName"],
                    lastName = (string)row["customerLName"],
                    Address = (string)row["customerAddress"],
                    eMail = (string)row["customerEmail"],
                    telephone = (string)row["customerPhone"]
                });
            }

            return clients;

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            DG_datagrid.ItemsSource = getClientList();
            DG_datagrid.Items.Refresh();

        }

        private async Task<List<string>> GetAutocompletePlaces(string input)
        {

            var country = "CA";  
            var httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                { "input", input },
                { "types", "" },  
                { "components", $"country:{country}" },
                { "key", Globals.ApiKey }
            };

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in parameters)
            {
                queryString[parameter.Key] = parameter.Value;
            }

            var url = $"{Globals.AutocompleteApiUrl}?{queryString}";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var places = getPlaces(responseBody);

                return places;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "API Request Error");
                return new List<string>();
            }
        }

        private List<string> getPlaces(string responseBody)
        {
            var places = new List<string>();
            var jsonObject = JObject.Parse(responseBody);
            var predictions = jsonObject["predictions"];


            foreach (var prediction in predictions)
            {
                var description = prediction["description"].ToString();

                places.Add(description);
            }

            return places;
        }


        private async void AutoCompleteComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            var userInput = (sender as ComboBox).Text;
            (sender as ComboBox).SelectedIndex = -1;

            var places = await GetAutocompletePlaces(userInput);
            comboAddress.Items.Clear();

            foreach (var place in places)
            {
                comboAddress.Items.Add(place);
            }

            comboAddress.IsDropDownOpen = true;
            var textBox = comboAddress.Template.FindName("PART_EditableTextBox", comboAddress) as TextBox;
            if (textBox != null)
            {
           
                textBox.CaretIndex = textBox.Text.Length;
       
                textBox.ScrollToEnd();
            }

             (sender as ComboBox).SelectedIndex = -1;

        }

        private void AutoCompleteComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Regex pattern to allow only letters and numbers
            var regex = new Regex("^[a-zA-Z0-9]+$");
            var text = e.Text;
            var isValidInput = regex.IsMatch(text);
            e.Handled = !isValidInput;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string fName = txt_fName.Text;
            string lName = txt_lName.Text;
            string address = comboAddress.Text;
            string phone = txtPhone.Text;
            string email = txtEmail.Text;
            string login = txtLogin.Text;
            string password = txtPassword.Text;
            string role = "CLIENT";
            string status = "ACTIVE";

            if (fName == "" || lName == "" || address == "" || phone == "" || email == "" || login == "" || password == "")
            {
                MessageBox.Show("Please fill all the fields");
            }
            else
            {
                if (! AuxFunctions.IsValidEmail(email)) {
                    AuxFunctions.showGeneralErrorDialog("The email you entered is not valid!!!"); 
                    return;
                }
                try
                {
                    string sql = "insert into tbl_Users (userName, pwd ,isAdmin, userRole) values ('" + login + "', '" + password + "', '" + "false" + "', '" + "CLIENT" + "')";
                    int x = AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.INSERT);                  
                    sql = "insert into tbl_Customers (customerFName, customerLName, customerAddress ,customerPhone, customerEmail, userID) values ('" + fName + "', '" + lName + "', '" + address + "', '" + phone + "', '" + email + "', " + x +")";
                    x = AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.INSERT);
                    clients.Add(new Clients()
                    {
                        ID = x,
                        firstName = fName,
                        lastName = lName,
                        Address = address,
                        eMail = email,
                        telephone = phone
                    });
                    DG_datagrid.ItemsSource = null; 
                    DG_datagrid.Items.Clear();
                    DG_datagrid.ItemsSource = clients;
                    DG_datagrid.Items.Refresh();
                    MessageBox.Show("Client added successfully");

                }
                catch (Exception ex)
                {
                    AuxFunctions.showDatabaseErrorDialog(ex.Message);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (DG_datagrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a row to delete");
                return;
            }
            Clients c = (Clients)DG_datagrid.SelectedItem;
            //  products.FirstOrDefault(x => x.ID == s.ID).ProductQuantity += s.ProductQuantity;           
            clients.Remove(c);
            AuxFunctions.executeSQLQuery("delete from tbl_Customers where id = " + c.ID );
            DG_datagrid.ItemsSource = null;
            DG_datagrid.Items.Remove(DG_datagrid.SelectedItem);
            DG_datagrid.ItemsSource = clients;
            AuxFunctions.showSuccessDialog("Client deleted successfully");
        }

        private void comboAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}