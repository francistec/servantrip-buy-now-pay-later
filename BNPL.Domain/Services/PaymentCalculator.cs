using BNPL.Domain.Entities;
using BNPL.Domain.Enums;
using BNPL.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Services
{
    public class PaymentCalculator
    {
        public PaymentSchedule CalculateSchedule(Booking booking)
        {
            ICancellationPolicy policy = booking.ProductType switch
            {
                ProductType.Transfer => new TransferPolicy(),
                ProductType.Activity => new ActivityPolicy(),
                ProductType.Debug => new DebugPolicy(),
                _ => throw new Exception("Invalid product type")
            };

            var chargeDate = policy.CalculateChargeDate(booking);

            if (booking.ProductType == ProductType.Debug)
            {
                // Para debug queremos cobrar YA
                chargeDate = DateTime.UtcNow;

                // StripeActivationDate debe ser antes de chargeDate
                return new PaymentSchedule
                {
                    AmountToCharge = booking.TotalAmount * 0.30m,
                    ChargeDate = chargeDate.AddMinutes(1), // cobrará en 1 min
                    StripeCreationDate = DateTime.UtcNow, // schedule se activa YA
                    Status = PaymentStatus.Scheduled
                };
            }

            return new PaymentSchedule
            {
                AmountToCharge = booking.TotalAmount * 0.30m, // regla inicial
                ChargeDate = chargeDate,
                Status = PaymentStatus.Scheduled,
                StripeCreationDate = chargeDate.AddDays(-15),
            };
        }
    }
}
