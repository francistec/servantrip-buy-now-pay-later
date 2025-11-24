using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Entities
{
    public class BnplSchedule
    {
        public string BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ChargeDate { get; set; }
        public DateTime StripeActivationDate { get; set; } // 15 días antes
        public string CustomerEmail { get; set; }
    }
}
