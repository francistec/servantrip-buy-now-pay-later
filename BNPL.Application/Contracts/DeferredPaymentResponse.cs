using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Application.Contracts
{
    public class DeferredPaymentResponse
    {
        public string BookingId { get; set; }
        public decimal AmountToCharge { get; set; }
        public DateTime ScheduledChargeDate { get; set; }
        public string Status { get; set; }
        public string StripeScheduleId { get; set; }
    }
}
