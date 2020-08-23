using System;
namespace LandBankOfThePhillipinesTLC.Models
{
    public class UserTimelogData
    {
        public string scanning_number { get; set; }
        public string transaction_date { get; set; }
        public string transaction_time { get; set; }
        public string transaction_type { get; set; }
        public string source_location { get; set; }
        public string source_device { get; set; }
    }
}
