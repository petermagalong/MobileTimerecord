using System;
using Newtonsoft.Json;

namespace LandBankOfThePhillipinesTLC.Models
{
    public class LoginDto
    {
        [JsonProperty("username",NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("password",NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }
}
