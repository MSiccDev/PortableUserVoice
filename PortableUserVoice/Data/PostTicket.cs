using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PortableUserVoice.Data
{
    public class PostTicket
    {
        [JsonProperty("ticket")]
        public TicketDetails TicketDetail { get; set; }

        public class TicketDetails
        {
            [JsonProperty("email")]
            public string UserMailAddress { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("state")]
            public string State { get; set; }

            //todo: add attachments to tickets we are opening

        }

    }
}
