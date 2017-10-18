using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistentePagos.Core.Models
{
    public class InvoiceModel
    {
        public string merchantId { get; set; }
        public int amount { get; set; }
        public DateTime creationDate { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string documentId { get; set; }
        public DateTime dueDate { get; set; }
        public string invoiceNumber { get; set; }
        public string paymentReference { get; set; }
        public string documentType { get; set; }
        public string merchantName { get; set; }
        public string id { get; set; }
    }
}
