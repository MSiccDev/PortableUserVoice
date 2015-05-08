using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class KnowledgeBaseResult
    {
        public ResponseData response_data { get; set; }

        public List<Article> articles { get; set; }


        public class ResponseData
        {
            public int page { get; set; }
            public int per_page { get; set; }
            public int total_records { get; set; }
            public string filter { get; set; }
            public string sort { get; set; }
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
            public string posted_tickets_count { get; set; }
            public string last_message_posted_at { get; set; }
            public string last_ticket_state { get; set; }
            public string last_subscribed_idea_closed_at { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }

        public class Topic
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Article
        {
            public string url { get; set; }
            public string path { get; set; }
            public int id { get; set; }
            public string title { get; set; }
            public string text { get; set; }
            public string formatted_text { get; set; }
            public int uses { get; set; }
            public int instant_answers { get; set; }
            public bool published { get; set; }
            public UpdatedBy updated_by { get; set; }
            public Topic topic { get; set; }
            public int position { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public string question { get; set; }
            public string answer_html { get; set; }
        }

        
    }
}
