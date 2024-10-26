using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.Converters;

namespace XStudio.Users
{
    public class LoginDto
    {
        [JsonProperty("password")]
        [JsonConverter(typeof(PurpleMinMaxLengthCheckConverter))]
        public string Password { get; set; } = string.Empty;

        [JsonProperty("rememberMe", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RememberMe { get; set; }

        [JsonProperty("userNameOrEmailAddress")]
        [JsonConverter(typeof(FluffyMinMaxLengthCheckConverter))]
        public string UserNameOrEmailAddress { get; set; } = string.Empty;
    }
}
