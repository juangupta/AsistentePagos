using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AsistentePagos.Core.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        [PrimaryKey]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "documentType")]
        public string DocumentType { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "documentId")]
        public string DocumentId { get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }

        public string PassUser { get; set; }

        public override string ToString()
        {
            return string.Format("ID={0}, Name={1}, DocumentId={2}", Id, Name, DocumentId);
        }


    }
}
