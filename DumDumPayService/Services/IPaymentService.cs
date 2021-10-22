using System.Threading.Tasks;
using DumDumPayService.Models;

namespace DumDumPayService.Services
{
    /// <summary>
    /// Main interface for DI
    /// </summary>
    public interface IPaymentService
    {
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePayment task);

        Task<ConfirmPaymentResponse> ConfirmPaymentAsync(ConfirmPayment task);

        Task<ConfirmPaymentResponse> GetPaymentStatusAsync(string transactionId);
    }
}
