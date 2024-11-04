using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Models.Requests {
    public class TokenReq {
        [JsonProperty("client_id")]
        public string ClientId { get; set; } = string.Empty;

        [JsonProperty("client_secret", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientSecret { get; set; } = string.Empty;

        [JsonProperty("grant_type")]
        public string GrantType { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public string Scope { get; set; } = string.Empty;

        [JsonProperty("username")]
        public string UserName { get; set; } = string.Empty;

        public static TokenReq? FromJson(string json) => JsonConvert.DeserializeObject<TokenReq>(json);
    }
}
