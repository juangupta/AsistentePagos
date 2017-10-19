using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistentePagos.Core.Models
{
    public class Account
    {
        public int Balance { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string AccountNumber { get; set; }
        public string UserId { get; set; }
    }
}
