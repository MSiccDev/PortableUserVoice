using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class SuggestionStatusesResult
    {


        public ResponseData response_data { get; set; }
        public List<Status> statuses { get; set; }

        public class ResponseData
        {
            public int page { get; set; }
            public int per_page { get; set; }
            public int total_records { get; set; }
            public string filter { get; set; }
            public string sort { get; set; }
        }

        public class Status
        {
            public int id { get; set; }
            public string name { get; set; }
            public string @event { get; set; }
        }



    }
}
