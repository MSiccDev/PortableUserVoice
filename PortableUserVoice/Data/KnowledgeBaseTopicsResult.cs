using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class KnowledgeBaseTopicsResult
    {
        public ResponseData response_data { get; set; }
        public List<Topic> topics { get; set; }

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
            public string url { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string position { get; set; }
            public string article_count { get; set; }
        }


        
    }
}
