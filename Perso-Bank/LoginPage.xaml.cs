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

namespace Perso_Bank
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string sql = "select * from tbl_users where userName='" + LoginTB.Text.Trim() + "' and pwd='" + PasswordTD.Text + "'";
            DataTable? dt = null;
            try
            {
                dt = AuxFunctions.executeSQLQuery(sql);
            }
            catch (Exception ex)
            {
                AuxFunctions.showDatabaseErrorDialog(ex.Message);
                return;
            }
            if (dt.Rows.Count == 0)
            {
                AuxFunctions.showGeneralErrorDialog("Error: User not found");
                return;
            }
            else {             
             //   AuxFunctions.showSuccessDialog("User found.");
                Globals.userID =(int) dt.Rows[0]["ID"];
                Globals.isAdmin = (bool)dt.Rows[0]["isAdmin"];
                Globals.userName = (string)dt.Rows[0]["userName"];
                Globals.userRole = (string)dt.Rows[0]["userRole"];

                if (Globals.userRole.Equals("CLIENT"))
                {
                    string sql2 = $"select * from tbl_Customers where userID= {Globals.userID} ";
                    DataTable? dt2 = null;
                    try
                    {
                        dt2 = AuxFunctions.executeSQLQuery(sql2);
                        Globals.ID = (int)dt2.Rows[0]["ID"];
                    }
                    catch (Exception ex)
                    {
                        AuxFunctions.showDatabaseErrorDialog(ex.Message);
                        return;
                    }
                }
                else { 
                    string sql2 = $"select * from tbl_Emps where userID= {Globals.userID} ";
                    DataTable? dt2 = null;
                    try
                    {
                        dt2 = AuxFunctions.executeSQLQuery(sql2);
                        Globals.ID = (int)dt2.Rows[0]["ID"];
                    }
                    catch (Exception ex)
                    {
                        AuxFunctions.showDatabaseErrorDialog(ex.Message);
                        return;
                    }                               
                }
                MainWindow mw = new();
                mw.Show();
                this.Close();
            }
        }
    }
}
