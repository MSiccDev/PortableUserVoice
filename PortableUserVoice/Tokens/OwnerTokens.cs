using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Tokens
{
    /// <summary>
    /// class to easily handle owner tokens
    /// </summary>
    public class OwnerTokens
    {
        /// <summary>
        /// your received access token
        /// </summary>
        public static string AccessToken {get; set;}


        /// <summary>
        /// your received access token secret
        /// </summary>
        public static string AccessTokenSecret { get; set; }
    }
}
