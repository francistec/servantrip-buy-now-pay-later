using BNPL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Entities
{
    public class PaymentSchedule
    {
        public decimal AmountToCharge { get; set; }
        public DateTime ChargeDate { get; set; }
        public PaymentStatus Status { get; set; }

        public DateTime StripeCreationDate { get; set; }
    }
}
