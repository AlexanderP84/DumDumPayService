using DumDumPayService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DumDumPayService.TestWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IPaymentService, PaymentService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IPaymentService service, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }                        

            app.Run(async (context) =>
            {
                logger.LogInformation("Processing has been started.");

                #region Creating payment
                logger.LogInformation("Creating payment, using test data.");
                var payment = PaymentProcessing.CreateTestPayment();                
                var createdTask = await service.CreatePaymentAsync(payment);

                if (createdTask.Result.TransactionStatus == "Declined" || createdTask.Result.TransactionStatus == "DeclinedDueToInvalidCreditCard")
                {
                    logger.LogError("Payment was not created, received an error status: {0}", createdTask.Result.TransactionStatus);
                    throw new Exception("Payment was not created. See the log for details.");
                }

                logger.LogInformation("Payment created, received data.");
                await context.Response.WriteAsync("Created payment, transaction id is " + createdTask.Result.TransactionId.ToString() + Environment.NewLine);
                #endregion

                #region Check payment status

                string status = "Init"; // Assume we have Init after successfull creating payment

                while (status == "Init" || status == "Pending")
                {
                    logger.LogInformation("Checking status for transaction id {0}", createdTask.Result.TransactionId);

                    var getStatusTask = await service.GetPaymentStatusAsync(createdTask.Result.TransactionId);
                    await context.Response.WriteAsync("Current payment status is " + getStatusTask.Result.Status.ToString() + Environment.NewLine);

                    status = getStatusTask.Result.Status;

                    await System.Threading.Tasks.Task.Delay(30 * 1000); // waiting for 30 seconds
                }

                if (status == "Declined" || status == "DeclinedDueToInvalidCreditCard")
                {
                    logger.LogError("Payment was not created, received an error status: {0}", createdTask.Result.TransactionStatus);
                    throw new Exception("Payment was not created. See the log for details.");
                }
                #endregion

                #region Confirm payment

                // Assuming we have "Approved" status now.

                logger.LogInformation("Confirming payment, using results from previous step");
                var confirm = PaymentProcessing.CreateTestConfirmPayment(createdTask.Result);
                
                var confirmTask = await service.ConfirmPaymentAsync(confirm);

                if (confirmTask.Result.Status != "Approved")
                {
                    logger.LogError("Payment was not confirmed, received an error status: {0}", confirmTask.Result.Status);
                    throw new Exception("Payment was not confirmed. See the log for details.");
                }                

                await context.Response.WriteAsync(confirmTask.Result.LastFourDigits.ToString());
                #endregion

                logger.LogInformation("Processing ended.");
            });
        }
    }
}
