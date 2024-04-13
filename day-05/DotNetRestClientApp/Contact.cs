using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetRestClientApp
{
    public class Contact
    {
        [JsonProperty("id")]
        public string? Id { set; get; }
        [JsonProperty("firstname")]
        public string? Firstname { set; get; }
        [JsonProperty("lastname")]
        public string? Lastname { set; get; }
        [JsonProperty("gender")]
        public string? Gender { set; get; }
        [JsonProperty("email")]
        public string? Email { set; get; }
        [JsonProperty("phone")]
        public string? Phone { set; get; }
        [JsonProperty("address")]
        public string? Address { set; get; }
        [JsonProperty("city")]
        public string? City { set; get; }
        [JsonProperty("state")]
        public string? State { set; get; }
        [JsonProperty("country")]
        public string? Country { set; get; }
        [JsonProperty("pincode")]
        public string? Pincode { set; get; }
        [JsonProperty("picture")]
        public string? Picture { set; get; }

    }
}
