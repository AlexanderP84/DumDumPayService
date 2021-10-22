using Microsoft.Extensions.DependencyInjection;
using DumDumPayService.Services;

namespace DumDumPayService
{
    public static class Bootstrapper
    {
        public static void UseServices(this IServiceCollection services)
        {
            services.AddHttpClient<IPaymentService, PaymentService>();
        }
    }
}
