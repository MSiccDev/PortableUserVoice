using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class ForumsResult
    {
        public ResponseData response_data { get; set; }
        public List<Forum> forums { get; set; }

        public class ResponseData
        {
            public int page { get; set; }
            public int per_page { get; set; }
            public int total_records { get; set; }
            public string filter { get; set; }
            public string sort { get; set; }
        }

        public class Topic
        {
            public int id { get; set; }
            public string prompt { get; set; }
            public string example { get; set; }
            public int votes_allowed { get; set; }
            public int suggestions_count { get; set; }
            public int open_suggestions_count { get; set; }
            public object closed_at { get; set; }
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

        public class Forum
        {
            public string url { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string welcome { get; set; }
            public List<Topic> topics { get; set; }
            public UpdatedBy updated_by { get; set; }
            public bool @private { get; set; }
            public bool anonymous_access { get; set; }
            public int suggestions_count { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }


    }
}
