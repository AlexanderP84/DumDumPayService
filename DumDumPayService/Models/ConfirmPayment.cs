using Newtonsoft.Json;

namespace DumDumPayService.Models
{
    public class ConfirmPayment
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("paRes")]
        public string PaRes { get; set; }
    }
}
