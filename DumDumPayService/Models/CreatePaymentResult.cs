using Newtonsoft.Json;

namespace DumDumPayService.Models
{
    public class CreatePaymentResult
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("transactionStatus")]
        public string TransactionStatus { get; set; } // Should we convert it to Enum?

        [JsonProperty("paReq")]
        public string PaReq { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public class CreatePaymentResponse
    {
        [JsonProperty("result")]
        public CreatePaymentResult Result { get; set; }
    }
}
