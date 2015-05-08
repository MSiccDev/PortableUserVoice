using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class NewTicketResult
    {
        public Ticket ticket { get; set; }

        public class Ticket
        {
            public int id { get; set; }
            public int ticket_number { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

    }
}
