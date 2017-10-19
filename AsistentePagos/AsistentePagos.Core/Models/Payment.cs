using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistentePagos.Core.Models
{
    public class Payment
    {

            public string sourceAccount { get; set; }
            public string targetTypeDocument { get; set; }
            public int amountPayment { get; set; }
            public string description { get; set; }
            public string targetDocumentId { get; set; }
            public string merchantId { get; set; }
            public string targetTypeAccount { get; set; }
            public string sourceTypeAccount { get; set; }
            public string currency { get; set; }
            public string invoiceId { get; set; }
            public string targetAccount { get; set; }
            public string id { get; set; }
            public DateTime paymentDate { get; set; }

    }
}
