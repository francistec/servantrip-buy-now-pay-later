using BNPL.Domain.Entities;
using System;
using Stripe;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Infrastructure.Stripe
{
    public class PaymentSchedulerService
    {
        public PaymentSchedulerService(string stripeApiKey)
        {
            StripeConfiguration.ApiKey = stripeApiKey;
        }

        public async Task<string> CreateScheduleAsync(BnplSchedule schedule)
        {

            Console.WriteLine("🔵 CreateScheduleAsync() iniciado");
            Console.WriteLine($"BookingId: {schedule.BookingId}");
            Console.WriteLine($"ChargeDate: {schedule.ChargeDate}");
            Console.WriteLine($"StripeActivationDate: {schedule.StripeActivationDate}");
            Console.WriteLine($"CustomerEmail: {schedule.CustomerEmail}");

            // 1. Crear o buscar customer en Stripe
            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = schedule.CustomerEmail
            });

            // 2. Crear invoice item (monto a cobrar)
            var invoiceItemService = new InvoiceItemService();
            await invoiceItemService.CreateAsync(new InvoiceItemCreateOptions
            {
                Customer = customer.Id,
                Amount = (long)(schedule.Amount * 100), // Stripe usa centavos
                Currency = "mxn",
                Description = $"BNPL Payment for booking {schedule.BookingId}"
            });

            // 3. Crear Subscription dummy (Stripe lo exige)
            var priceService = new PriceService();
            var price = await priceService.CreateAsync(new PriceCreateOptions
            {
                Currency = "mxn",
                UnitAmount = 0,                       // €0 - NO COBRA
                Recurring = new PriceRecurringOptions
                {
                    Interval = "day",
                    IntervalCount = 1                  // Intervalo dummy
                },
                ProductData = new PriceProductDataOptions
                {
                    Name = "BNPL Dummy Product"
                }
            });


            // 4. Crear el schedule
            var scheduleService = new SubscriptionScheduleService();
            var createdSchedule = await scheduleService.CreateAsync(new SubscriptionScheduleCreateOptions
            {
                Customer = customer.Id,
                StartDate = schedule.StripeActivationDate,
                EndBehavior = "cancel",
                Phases = new List<SubscriptionSchedulePhaseOptions>
            {
                new SubscriptionSchedulePhaseOptions
                {
                    //StartDate = schedule.StripeActivationDate,
                    EndDate = schedule.ChargeDate,
                    BillingCycleAnchor = "automatic",
                    ProrationBehavior = "none",
                    Items = new List<SubscriptionSchedulePhaseItemOptions>
                {
                    new SubscriptionSchedulePhaseItemOptions
                    {
                        Price = price.Id,
                        Quantity = 1
                    }
                }
                }
            }
            });

            return createdSchedule.Id;
        }

        private long ToUnix(DateTime date)
        {
            return new DateTimeOffset(date).ToUnixTimeSeconds();
        }
    }
}
