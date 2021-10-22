using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DumDumPayService.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumDumPayService.Services
{
    /// <summary>
    /// Using HttpClient via DI, calls API
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private const string BaseUrl = "https://private-anon-c57a37968f-dumdumpay.apiary-mock.com/api/payment/"; // Should convert to parameter?
        private const string MerchantId = "6fc3aa31-7afd-4df1-825f-192e60950ca1"; // Should convert to parameter?
        private const string SecretKey = "53cr3t"; // Should convert to parameter?

        private readonly HttpClient _client;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(HttpClient client, ILogger<PaymentService> logger)
        {
            _client = client;

            _client.DefaultRequestHeaders.Add("MerchantId", MerchantId);
            _client.DefaultRequestHeaders.Add("SecretKey", SecretKey);

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Create payment
        /// </summary>
        /// <param name="createPayment">CreatePayment data</param>
        /// <returns>CreatePaymentResponse object</returns>
        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePayment createPayment)
        {
            _logger.LogInformation("sending request to DumDumPay API: create payment");
            var content = JsonConvert.SerializeObject(createPayment);
            _logger.LogDebug("payment json: {0}", content);

            var httpResponse = await _client.PostAsync($"{BaseUrl}create", new StringContent(content, Encoding.Default, "application/json"));

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var errors = JsonConvert.DeserializeObject<ErrorResult>(await httpResponse.Content.ReadAsStringAsync());
                _logger.LogError("API has returned one or more errors:");

                foreach (var error in errors.Errors)
                {
                    _logger.LogError("Type: {0}, Message: {1}", error.Type, error.Message);
                }

                throw new Exception("Cannot create payment. See the log for details.");
            }

            var response = await httpResponse.Content.ReadAsStringAsync();
            var createdTask = JsonConvert.DeserializeObject<CreatePaymentResponse>(response);

            _logger.LogInformation("CreatePayment request was completed successfully.");
            _logger.LogInformation("CreatePayment response json: {0}", response);

            return createdTask;
        }

        /// <summary>
        /// Confirm payment
        /// </summary>
        /// <param name="confirmPayment">Confirm payment data</param>
        /// <returns>ConfirmPaymentResponse object</returns>
        public async Task<ConfirmPaymentResponse> ConfirmPaymentAsync(ConfirmPayment confirmPayment)
        {
            _logger.LogInformation("sending request to DumDumPay API: confirm payment");
            var content = JsonConvert.SerializeObject(confirmPayment);
            _logger.LogDebug("confirm payment json: {0}", content);

            var httpResponse = await _client.PostAsync($"{BaseUrl}confirm", new StringContent(content, Encoding.Default, "application/json"));

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var errors = JsonConvert.DeserializeObject<ErrorResult>(await httpResponse.Content.ReadAsStringAsync());
                _logger.LogError("API has returned one or more errors:");

                foreach (var error in errors.Errors)
                {
                    _logger.LogError("Type: {0}, Message: {1}", error.Type, error.Message);
                }

                throw new Exception("Cannot confirm payment. See the log for details.");                
            }

            var response = await httpResponse.Content.ReadAsStringAsync();
            var confirmedTask = JsonConvert.DeserializeObject<ConfirmPaymentResponse>(response);

            _logger.LogInformation("ConfirmPayment request was completed successfully.");
            _logger.LogInformation("ConfirmPayment response json: {0}", response);

            return confirmedTask;
        }

        /// <summary>
        /// Get payment status
        /// </summary>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>ConfirmPaymentResponse object</returns>
        public async Task<ConfirmPaymentResponse> GetPaymentStatusAsync(string transactionId)
        {
            _logger.LogInformation("sending request to DumDumPay API: get payment status");

            var httpResponse = await _client.GetAsync($"{BaseUrl}{transactionId}/status");

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var errors = JsonConvert.DeserializeObject<ErrorResult>(await httpResponse.Content.ReadAsStringAsync());

                _logger.LogError("API has returned one or more errors:");

                foreach (var error in errors.Errors)
                {
                    _logger.LogError("Type: {0}, Message: {1}", error.Type, error.Message);
                }

                throw new Exception("Cannot get payment status. See the log for details.");
            }

            var response = await httpResponse.Content.ReadAsStringAsync();
            var confirmedTask = JsonConvert.DeserializeObject<ConfirmPaymentResponse>(response);

            _logger.LogInformation("GetPaymentStatus request was completed successfully.");
            _logger.LogInformation("GetPaymentStatus response json: {0}", response);

            return confirmedTask;
        }
    }
}
