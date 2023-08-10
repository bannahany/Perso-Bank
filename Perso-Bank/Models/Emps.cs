using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perso_Bank.Models
{
    class Emp
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Extension { get; set; }
        public string email { get; set; }
        public int userID { get; set; }

        public Emp(int ID, string fName, string lName, string ext, string email, int userID)
        {
            id = ID;
            FirstName = fName;
            LastName = lName;
            Extension = ext;
            this.email = email;
            this.userID = userID;
        }
    }
}
