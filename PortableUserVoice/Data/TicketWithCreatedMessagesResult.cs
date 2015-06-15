using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class TicketWithCreatedMessagesResult
    {

        public Ticket ticket { get; set; }


        public class Recipient
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public object title { get; set; }
            public string url { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Sender
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public object title { get; set; }
            public string url { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Message
        {
            public int id { get; set; }
            public string body { get; set; }
            public bool is_admin_response { get; set; }
            public Recipient recipient { get; set; }
            public Sender sender { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Assignee
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public object title { get; set; }
            public string url { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class CreatedBy
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public object title { get; set; }
            public string url { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class UpdatedBy
        {
            public string name { get; set; }
            public string email { get; set; }
            public object title { get; set; }
            public string url { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Ticket
        {
            public int id { get; set; }
            public int ticket_number { get; set; }
            public string subject { get; set; }
            public string state { get; set; }
            public object tags { get; set; }
            public List<object> custom_fields { get; set; }
            public List<Message> messages { get; set; }
            public Assignee assignee { get; set; }
            public CreatedBy created_by { get; set; }
            public UpdatedBy updated_by { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }


        
    }
}
