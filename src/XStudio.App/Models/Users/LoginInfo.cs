using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Models.Users {
    public class LoginInfo {
        [JsonProperty("userNameOrEmailAddress")]
        public string UserNameOrEmailAddress { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;

        [JsonProperty("rememberMe", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RememberMe { get; set; }

        [JsonProperty("clientId", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string ClientId { get; set; } = "XStudio_App";

        [JsonProperty("scope", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Scope { get; set; } = "XStudio";
    }
}
