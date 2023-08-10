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
using System.Windows.Xps;
using Perso_Bank.Models;

namespace Perso_Bank.Forms
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        List<Emp> emps = new List<Emp>();
        public AddEmployee()
        {
            InitializeComponent();
        }

        private List<Emp> getAllEmps()
        {
            List<Emp> emps = new List<Emp>();
            string sql = "SELECT * FROM tbl_Emps";
            DataTable dt = AuxFunctions.executeSQLQuery(sql);

            dt.AsEnumerable().ToList().ForEach(row =>
            {
                emps.Add(new Emp(Convert.ToInt32(row["id"]), row["FirstName"].ToString(), row["LastName"].ToString(), row["Extension"].ToString(), row["email"].ToString(), Convert.ToInt32( row["userID"])));
            });

            return emps;
        }          
            
            
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            string FName = txtbox_FName.Text;
            string LName = txtbox_LName.Text;
            string Email = txtbox_email.Text;
            string Password = txtbox_password.Text;
            string Ext = txtbox_ext.Text;
            string login = txtbox_login.Text;
            bool isAdmin  = chkbox_isAdmin.IsChecked.Value;

            if (FName.Equals (""))
            {
                MessageBox.Show("Please enter a first name");
                return;
            }
            if (LName.Equals(""))
            {
                MessageBox.Show("Please enter a last name");
                return;
            }
            if (Email.Equals(""))
            {
                MessageBox.Show("Please enter an email");
                return;
            }
            if (Password.Equals(""))
            {
                MessageBox.Show("Please enter a password");
                return;
            }
            if (Ext.Equals(""))
            {
                MessageBox.Show("Please enter an extension");
                return;
            }
            if (login.Equals(""))
            {
                MessageBox.Show("Please enter a login");
                return;
            }
            if (!AuxFunctions.IsValidEmail(Email))
            {
                MessageBox.Show("Please enter a valid email");
                return;
            }       

            string sql  = "INSERT INTO tbl_Users (userName, pwd, isAdmin, userRole) VALUES ('" + login + "', '" + Password + "', '" + isAdmin + "', '" + "EMP" + "')";

            int result;
            try {
                result = AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.INSERT);
                if (result > 0)
                {               
                    sql = "INSERT INTO tbl_Emps (FirstName, LastName,  Extension, email, UserID) VALUES ('" + FName + "', '" + LName + "', '" + Ext + "', '" + Email + "', '" + result + "')";
                    AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.INSERT);
                    DG_datagrid.ItemsSource = null;
                    DG_datagrid.ItemsSource = getAllEmps();
                    MessageBox.Show("Employee added successfully to users table.");
                }
                else
                {
                    MessageBox.Show("Employee not added.");
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Emp> emps = getAllEmps();
            DG_datagrid.ItemsSource = emps;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DG_datagrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a row to delete");
                return;
            }
            Emp emp = (Emp) DG_datagrid.SelectedItem;           
            string sql1 = "delete from tbl_Emps where id = " + emp.id;
            string sql2 = "delete from tbl_Users where ID = " + emp.userID;

            List<string> sqls = new List<string>();
            sqls.Add(sql1);
            sqls.Add(sql2);

            AuxFunctions.executeMultipleStatements (sqls);            
         //   int d1 = AuxFunctions.executeSingleNonQuery("delete from tbl_Emps where id = " + emp.id, AuxFunctions.DB_CRUD_TYPE.DELETE);
          //  AuxFunctions.executeSQLQuery("delete from tbl_Users where id = " + emp.userID);

            DG_datagrid.ItemsSource = null;           
            DG_datagrid.ItemsSource = getAllEmps();
            AuxFunctions.showSuccessDialog("Employee deleted successfully");
        }
    }
}
