using DumDumPayService.Models;

namespace DumDumPayService.TestWebApp
{
    /// <summary>
    /// Helper for test purposes
    /// </summary>
    public static class PaymentProcessing
    {
        /// <summary>
        /// Create test payment
        /// </summary>
        /// <returns>CreatePayment object</returns>
        public static CreatePayment CreateTestPayment()
        {
            var payment = new CreatePayment();

            payment.OrderId = "DBB99946-A10A-4D1B-A742-577FA026BC57";
            payment.Amount = 12312;
            payment.Currency = "USD";
            payment.Country = "CY";
            payment.CardNumber = "4111111111111111";
            payment.CardHolder = "TEST TESTER";
            payment.CardExpiryDate = "1123";
            payment.CVV = "111";

            return payment;
        }

        /// <summary>
        /// Create test confirm payment
        /// </summary>
        /// <param name="result">CreatePayment data</param>
        /// <returns>ConfirmPaymentObject</returns>
        public static ConfirmPayment CreateTestConfirmPayment(CreatePaymentResult result)
        {
            var confirm = new ConfirmPayment();

            confirm.TransactionId = result.TransactionId;
            confirm.PaRes = result.PaReq;

            return confirm;
        }
    }
}
