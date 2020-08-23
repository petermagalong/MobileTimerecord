using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LandBankOfThePhillipinesTLC.Models
{
    public class Datum
    {
        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("isfirstlogin")]
        public string isfirstlogin { get; set; }

        [JsonProperty("full_name")]
        public string full_name { get; set; }

        [JsonProperty("scanning_number")]
        public string scanning_number { get; set; }

        [JsonProperty("location")]
        public string location { get; set; }

        [JsonProperty("lat1")]
        public string lat1 { get; set; }

        [JsonProperty("lat2")]
        public string lat2 { get; set; }

        [JsonProperty("long1")]
        public string long1 { get; set; }

        [JsonProperty("long2")]
        public string long2 { get; set; }
    }

    public class UserData
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Data")]
        public IList<Datum> Data { get; set; }
        
    }
    
    
    
}
