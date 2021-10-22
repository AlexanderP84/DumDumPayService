using DumDumPayService.Models;
using DumDumPayService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DumDumPayService.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        [TestMethod]
        public async Task CreatePayment()
        {
            var services = new ServiceCollection();
            services.UseServices();

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IPaymentService>();

            var payment = new CreatePayment();

            payment.OrderId = "DBB99946-A10A-4D1B-A742-577FA026BC57";
            payment.Amount = 12312;
            payment.Currency = "USD";
            payment.Country = "CY";
            payment.CardNumber = "4111111111111111";
            payment.CardHolder = "TEST TESTER";
            payment.CardExpiryDate = "1123";
            payment.CVV = "111";

            var createdTask = await service.CreatePaymentAsync(payment);

            Assert.IsNotNull(createdTask.Result);
            Assert.AreEqual("7349bc64-719d-45b7-bc0f-4e4ac1549201", createdTask.Result.TransactionId);
        }

        [TestMethod]
        public async Task ConfirmPayment()
        {
            var services = new ServiceCollection();
            services.UseServices();

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IPaymentService>();

            var payment = new ConfirmPayment();

            payment.TransactionId = "94ac5a85-8b81-4aaa-89dd-00e968f05d01";
            payment.PaRes = "MzgxMmYwNjItODY4MC00ZTQzLTkxMjUtMDQzNTU4Zjc4Yjc4";

            var confirmedTask = await service.ConfirmPaymentAsync(payment);

            Assert.IsNotNull(confirmedTask.Result);
            Assert.AreEqual("2454", confirmedTask.Result.LastFourDigits);
        }

        [TestMethod]
        public async Task GetPaymentStatus()
        {
            var services = new ServiceCollection();
            services.UseServices();
            var serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetRequiredService<IPaymentService>();

            var task = await service.GetPaymentStatusAsync("94ac5a85-8b81-4aaa-89dd-00e968f05d01");

            Assert.IsNotNull(task.Result);
            Assert.AreEqual("DBB99946-A10A-4D1B-A742-577FA026BC57", task.Result.OrderId);
        }
    }
}
