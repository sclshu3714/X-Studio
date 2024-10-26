using DryIoc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Models.Users {
    public class TokenRequest {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        public string Scope { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        public static TokenRequest? FromJson(string json) => JsonConvert.DeserializeObject<TokenRequest>(json);
    }
}
