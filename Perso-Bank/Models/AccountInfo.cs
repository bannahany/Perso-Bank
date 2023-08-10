using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perso_Bank.Models
{
    public class AccountInfo
    {
        public string? accountNumber { get; set; }
        public string? accountType { get; set; }

        public decimal accountBalance { get; set; }

        public bool isActive { get; set; }

        public AccountInfo() { }

    }
}
