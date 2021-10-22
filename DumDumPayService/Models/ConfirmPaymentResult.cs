using Newtonsoft.Json;

namespace DumDumPayService.Models
{
    public class ConfirmPaymentResult
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("lastFourDigits")]
        public string LastFourDigits { get; set; }
    }

    public class ConfirmPaymentResponse
    {
        [JsonProperty("result")]
        public ConfirmPaymentResult Result { get; set; }
    }
}
