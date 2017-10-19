using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistentePagos.Core.Models
{
    public class User
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string DocumentType { get; set; }
        public string Name { get; set; }
        public string DocumentId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return string.Format("ID={0}, Name={1}, DocumentId={2}", Id, Name, DocumentId);
        }


    }
}
