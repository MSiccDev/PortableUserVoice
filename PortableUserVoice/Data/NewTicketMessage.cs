using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PortableUserVoice.Data
{
    
    public class NewTicketMessage
    {
        [JsonProperty("ticket_message")]
        public TicketMessage NewMessage { get; set; }



        public class TicketMessage
        {
            [JsonProperty("text")]
            public string MessageText { get; set; }

            //todo: add support for attachements
        }


    }
}
