using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Data
{
    public class UserResult
    {
        [JsonProperty("user")]
        public User data { get; set; }

        public class Authentication
        {
            public string provider { get; set; }
        }

        public class Roles
        {
            public bool owner { get; set; }
            public bool admin { get; set; }
        }

        public class SupportedSuggestion
        {
            public int id { get; set; }
            public int votes { get; set; }
        }

        public class ForumActivity
        {
            public int votes_available { get; set; }
            public List<SupportedSuggestion> supported_suggestions { get; set; }
        }

        public class VisibleForum
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool is_private { get; set; }
            public int idea_count { get; set; }
            public string url { get; set; }
            public int max_votes { get; set; }
            public ForumActivity forum_activity { get; set; }
        }

        public class User
        {
            public string url { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public object title { get; set; }
            public object guid { get; set; }
            public bool anonymous { get; set; }
            public string email { get; set; }
            public bool email_confirmed { get; set; }
            public Authentication authentication { get; set; }
            public Roles roles { get; set; }
            public string avatar_url { get; set; }
            public int karma_score { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public int supported_suggestions_count { get; set; }
            public int created_suggestions_count { get; set; }
            public List<VisibleForum> visible_forums { get; set; }
        }
    }
}
