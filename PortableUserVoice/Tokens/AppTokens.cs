using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableUserVoice.Tokens
{
    /// <summary>
    /// calls to simplify handling of tokens
    /// </summary>
    public class AppTokens
    {
        /// <summary>
        /// your consumer key
        /// </summary>
        public static string ConsumerKey { get; set; }

        /// <summary>
        /// your consumer secret
        /// </summary>
        public static string ConsumerSecret { get; set; }

        /// <summary>
        /// your callback url
        /// </summary>
        public static string CallbackUrl { get; set; }

        /// <summary>
        /// your UserVoice account's subdomain
        /// </summary>
        public static string Subdomain { get; set; }
    }

}
