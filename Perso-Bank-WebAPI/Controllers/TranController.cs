using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Perso_Bank;
using Perso_Bank_WebAPI.Models;
using System.Data;
using System.Transactions;

namespace Perso_Bank_WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TranController : ControllerBase
    {

        private bool isCustomerValid(int ID) { 
            string sql = $"SELECT ID FROM tbl_Customers WHERE ID = {ID}";
            DataTable? dt = AuxFunctions.executeSQLQuery(sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }        
        }

        private decimal getAccountBalance(int? accountNumber)
        {
            string sql = $"SELECT accountBalance FROM tbl_Accounts WHERE accountNumber = {accountNumber}";
            DataTable dt = AuxFunctions.executeSQLQuery(sql);
            decimal accountBalance = decimal.Parse(dt.Rows[0]["accountBalance"].ToString());
            return accountBalance;
        }

        private bool isAccountValid(int? accountNumber)
        {
            string sql = $"SELECT accountNumber FROM tbl_Accounts WHERE accountNumber = {accountNumber}";
            DataTable? dt = AuxFunctions.executeSQLQuery(sql);
            if (dt.Rows.Count > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isAccountActive(int? accountNumber)
        {
            string sql = $"SELECT isActive FROM tbl_Accounts WHERE accountNumber = {accountNumber}";
            DataTable? dt = AuxFunctions.executeSQLQuery(sql);
            return ((bool) dt.Rows[0]["isActive"]);
        }

        [HttpGet]
        [Route("getCustomerAccount/{ID}")]
        public CustomerAccountResponse getAllCustomerAccounts(int ID)
        {

            if (isCustomerValid(ID) == false)
            {
                CustomerAccountResponse response = new CustomerAccountResponse();
                response.response.status = 400;
                response.response.message = "Customer does not exist";
                return response;
            }   

            string sql = "";

            sql = "Select ";
            sql += "tbl_brdg_CustomerAccounts.customerID, ";
            sql += "tbl_AccountTypes.accountType, ";
            sql += "tbl_Accounts.accountNumber, ";
            sql += "tbl_Accounts.accountTypeID, ";
            sql += "tbl_Accounts.accountStartDate, ";
            sql += "tbl_Accounts.accountEndDate, ";
            sql += "tbl_Accounts.isActive, ";
            sql += "tbl_Accounts.accountBalance ";
            sql += "From ";
            sql += "tbl_brdg_CustomerAccounts Inner Join ";
            sql += "tbl_Accounts On tbl_brdg_CustomerAccounts.accountNumber = tbl_Accounts.accountNumber Inner Join ";
            sql += "tbl_AccountTypes On tbl_Accounts.accountTypeID = tbl_AccountTypes.ID ";
            sql += "Where ";
            sql +=  $"tbl_brdg_CustomerAccounts.customerID = {ID} ";

            DataTable? dt = AuxFunctions.executeSQLQuery(sql);
            if (dt.Rows.Count == 0)
            {
                CustomerAccountResponse car = new CustomerAccountResponse();
                car.response.status = 400;
                car.response.message = "Customer does not have any accounts";
                return car;
            }
            else
            {

                CustomerAccountResponse customerAccountResponse = new CustomerAccountResponse();
                customerAccountResponse.response.status = 200;
                customerAccountResponse.response.message = "Customer account retrieved successfully";
                foreach (DataRow row in dt.Rows)
                {
                    CustomerAccount customerAccount = new CustomerAccount();
                    customerAccount.accountNumber = row["accountNumber"].ToString();
                    customerAccount.accountType = row["accountType"].ToString();
                    customerAccount.accountBalance = decimal.Parse(row["accountBalance"].ToString());
                    customerAccount.isActive = (bool)row["isActive"];
                    customerAccount.displayName = customerAccount.accountNumber + " - " + customerAccount.accountType + " - $" + customerAccount.accountBalance;

                    customerAccountResponse.customerAccount.Add(customerAccount);
                }
                return customerAccountResponse;
            }            
        }

        [HttpGet]
        [Route("getAccountInfo/{accountNumber}")]
        public AccountInfoResponse getAccountInfo(int accountNumber)
        {

            if (isAccountValid(accountNumber) == false)
            {
                AccountInfoResponse response = new AccountInfoResponse();
                response.response.status = 400;
                response.response.message = "Account is inactive";
                return response;
            }

           string sql = $"Select tbl_Accounts.accountNumber, tbl_Accounts.isActive, tbl_Accounts.accountBalance, tbl_AccountTypes.accountType From tbl_Accounts Inner Join tbl_AccountTypes On tbl_AccountTypes.ID = tbl_Accounts.accountTypeID WHERE accountNumber = {accountNumber}";

           // string sql = $"SELECT * FROM tbl_Accounts WHERE accountNumber = {accountNumber}";
            DataTable? dt = AuxFunctions.executeSQLQuery(sql);
            AccountInfoResponse accountInfoRes = new AccountInfoResponse();
            accountInfoRes.accountInfo.accountNumber = dt.Rows[0]["accountNumber"].ToString();
            accountInfoRes.accountInfo.accountType = dt.Rows[0]["accountType"].ToString();
            accountInfoRes.accountInfo.accountBalance = decimal.Parse(dt.Rows[0]["accountBalance"].ToString());
            accountInfoRes.accountInfo.isActive = (bool)dt.Rows[0]["isActive"];
            accountInfoRes.response.status = 200;
            accountInfoRes.response.message = "Account information retrieved successfully";

            return accountInfoRes;
        }

        [HttpPut]
        [Route("PerformTransaction")]
        public Response performTransaction (Trans trans)
        {
            if (isAccountValid (trans.accountNumber) == false)
            {
                Response response = new Response();
                response.status = 400;
                response.message = "Account number is invalid";
                return response;
            }

            if (isAccountActive(trans.accountNumber) == false)
            {
                Response response = new Response();
                response.status = 400;
                response.message = "Account is inactive";
                return response;
            } 

            if (trans.transType == "WITHDRAW")
            {
                if (getAccountBalance(trans.accountNumber) < trans.amount)
                {
                    Response response = new Response();
                    response.status = 400;
                    response.message = "Insufficient funds";
                    return response;

                }
                else
                {
                    string sql = $"UPDATE tbl_Accounts SET accountBalance = accountBalance - {trans.amount} WHERE accountNumber = {trans.accountNumber}";
                    AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.UPDATE);
                    Response response = new Response();
                    response.status = 200;
                    response.message = "Transaction successful";
                    return response;
                }
            }
            else if (trans.transType == "DEPOSIT")
            {
                string sql = $"UPDATE tbl_Accounts SET accountBalance = accountBalance + {trans.amount} WHERE accountNumber = {trans.accountNumber}";
                AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.UPDATE);
                Response response = new Response();
                response.status = 200;
                response.message = "Transaction successful";
                return response;

            }
            else if (trans.transType == "CLOSE") {
                string sql = $"UPDATE tbl_Accounts SET accountBalance = 0 WHERE accountNumber = {trans.accountNumber}";
                AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.UPDATE);
                sql = $"UPDATE tbl_Accounts SET isActive = 'False' WHERE accountNumber = {trans.accountNumber}";
                AuxFunctions.executeSingleNonQuery(sql, AuxFunctions.DB_CRUD_TYPE.UPDATE);
                Response response = new Response();
                response.status = 200;
                response.message = "Transaction successful";
                return response;
            } else
            {
                Response response = new Response();
                response.status = 400;
                response.message = "Invalid transaction type";
                return response;
            }
        }

        //[HttpGet]
        //[Route("getAllTrans")]

        //public List<Trans> getAllTrans(int accountNumber) { 

        
        //} 
    }
}
