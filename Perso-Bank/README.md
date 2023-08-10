**420 – 942 – VA APPLICATION DEVELOPMENT**

**Teacher** : Kawser Wazed Nafi

**Team Partners**

Shareef T. Abdelhafiz (# **2295545** )

Hany Banna (# **2295366** )

**Submission Date**

May 26th 2023

**PROJECT TITLE: Perso-Bank**

(Git-Hub repository is called Perso-Bank)

**This repository is already created as a "private repository" and the team members are added to it)**

https://github.com/sharif-t-abdelhafiz/Perso-Bank.git

**Project Properties:**

**Backend Languages:**

- C# version 10 (released 2022)
- .NET Framework 6.0 (LTS)
- XAML will be used to do the front-end design.

**RDBMS Used:**

- SQL Server 16.0.1050.5 – developer edition (The latest)

**Tools and Libraries used in development:**

- Microsoft Visual Studio Community 2022 - Version 17.6.2
- SQL Server Management Studio 18 will be used to help with management of the database.
- SQL Server ADO.NET Library
- LINQ (if needed)
- Basic C# Libraries and Classes.
- Git and GitHub will be used for version control.

**Project Summary:**

Perso-Bank is a banking system that will have the ability to perform basic banking operations.

**Project Actors:**

The system will have two (2) different types of users:

1. Bank Customer: This user will have limited access to the system. He can only view his own information and will not be able to perform administrative functions such as adding new customers (this is not his role)
2. Bank Employee: This type of employee will have access to all the information and data of a Bank Customer, but will have administrative functions such as creating, updating, or deleting customers.

**Project Scope:**

**REALISTIC SCOPE:**

The Perso-Bank's project scope will include:

1. A seed "super user" employee will be created manually in the database.
2. The system will allow the "super user" for the addition of "regular employee" to the bank.
3. The "super user" system will allow for the addition of a location to the bank.
4. An employee will be able to create customers and other employees. This will include gathering information like the name of the customer, his address, phone number, and other vital information. It will also ask for two pieces of identification.
5. Upon creation of a customer the customer will be given a unique ID.
6. The customer will have multiple bank accounts. Each bank account will be either a savings account or a checking account. A customer will be able to open new accounts on his own.
7. The bank employee will have the ability to change the interest rate for each type of account.
8. The system will allow the employee of a bank to search for a customer using a variety of criteria such as ID, first name, last name, or his phone number, it will also allow him to update, or delete a customer.
9. The system will allow a customer to transfer money from one account to another and send money to other customers (e-Transfer).
10. The software will allow a customer to withdraw and deposit money from the account. Warnings will be given if a specific withdrawal is not possible.
11. The system will have a **REST endpoint** to receive data from the database for some of the data and **straight ADO client-database** connection for others to show our ability to do both methods.
12. Customers will have the choice to view their account(s) transactions.
13. will use standard controls that come with Visual Studio 2022.

**Broader Project Scope:**

_The following is a scope that we will strive to achieve but given that we will realistically only have_ _ **1 month** _ _to complete the project (we don't know enough about XAML and C#, or REST … __**yet**__ ). We also have a 2 week break in the summer. It is unlikely that we will be able to achieve the following, but as mentioned we will stive to do it._

1. Allow users to open investment accounts such as RRSP account with the associated restrictions on an RRSP.
2. Allow the user to be able to open a capital gains account, where he can buy and sell stocks (we can get stock names, and current prices from free endpoints on the internet).
3. Allow the user to buy GIC with a locked-in date. The user will have the choice of which regular account he will withdraw the money from and the GICs will have the usual rules in place.
4. User sign-in using laptop fingerprint recognition.
5. The ability for a customer to **order** cheques from the system.
