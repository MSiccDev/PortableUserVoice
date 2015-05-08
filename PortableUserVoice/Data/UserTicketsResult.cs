using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class UserTicketsResult
    {
        public ResponseData response_data { get; set; }
        public List<Ticket> tickets { get; set; }

        public class ResponseData
        {
            public string query { get; set; }
            public int page { get; set; }
            public int per_page { get; set; }
            public int total_records { get; set; }
            public string filter { get; set; }
            public string sort { get; set; }
        }

        public class Sender
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string title { get; set; }
            public string url { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Message
        {
            public int id { get; set; }
            public string channel { get; set; }
            public string body { get; set; }
            public string plaintext_body { get; set; }
            public bool is_admin_response { get; set; }
            public Sender sender { get; set; }
            public List<object> attachments { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Assignee
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string title { get; set; }
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
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string title { get; set; }
            public string url { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Contact
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

        public class Ticket
        {
            public int id { get; set; }
            public int ticket_number { get; set; }
            public string subject { get; set; }
            public string state { get; set; }
            public string url { get; set; }
            public List<object> custom_fields { get; set; }
            public List<Message> messages { get; set; }
            public List<object> notes { get; set; }
            public Assignee assignee { get; set; }
            public CreatedBy created_by { get; set; }
            public UpdatedBy updated_by { get; set; }
            public Contact contact { get; set; }
            public string last_message_at { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }



        
    }
}
