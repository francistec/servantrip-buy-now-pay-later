using BNPL.Application.Contracts;
using BNPL.Domain.Entities;
using BNPL.Domain.Enums;
using BNPL.Domain.Services;
using BNPL.Infrastructure.Stripe;

namespace BNPL.Application.Services
{
  
    public class BookingService
    {
        private readonly PaymentCalculator _calculator;
        private readonly PaymentSchedulerService _stripeService;

        public BookingService(PaymentCalculator calculator, PaymentSchedulerService stripeService)
        {
            _calculator = calculator;
            _stripeService = stripeService;
        }

        public async Task<DeferredPaymentResponse> ProcessAsync(BookingRequest request)
        {
            var booking = new Booking
            {
                BookingId = request.BookingId,
                ProductType = Enum.Parse<ProductType>(request.ProductType, true),
                TotalAmount = request.TotalAmount,
                ServiceDate = request.ServiceDate,
                UserEmail = request.UserEmail
            };

            var schedule = _calculator.CalculateSchedule(booking);

            var bnplSchedule = new BnplSchedule
            {
                BookingId = booking.BookingId,
                Amount = schedule.AmountToCharge,
                ChargeDate = schedule.ChargeDate,
                StripeActivationDate = schedule.StripeCreationDate,
                CustomerEmail = booking.UserEmail
            };

            var scheduleId = await _stripeService.CreateScheduleAsync(bnplSchedule);

            return new DeferredPaymentResponse
            {
                BookingId = booking.BookingId,
                AmountToCharge = schedule.AmountToCharge,
                ScheduledChargeDate = schedule.ChargeDate,
                Status = schedule.Status.ToString().ToLower(),
                StripeScheduleId = scheduleId
            };
        }

        public DeferredPaymentResponse Process(BookingRequest request)
        {
            // Convertir el request al modelo de dominio
            var booking = new Booking
            {
                BookingId = request.BookingId,
                ProductType = Enum.Parse<ProductType>(request.ProductType, true),
                TotalAmount = request.TotalAmount,
                ServiceDate = request.ServiceDate,
                UserEmail = request.UserEmail
            };

            // Usar el dominio para calcular la fecha de cobro
            var schedule = _calculator.CalculateSchedule(booking);

            // Convertir resultado del dominio a respuesta API
            return new DeferredPaymentResponse
            {
                BookingId = booking.BookingId,
                AmountToCharge = schedule.AmountToCharge,
                ScheduledChargeDate = schedule.ChargeDate,
                Status = schedule.Status.ToString().ToLower()
            };
        }
   
    }

}
