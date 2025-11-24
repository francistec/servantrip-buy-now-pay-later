using BNPL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Policies
{
    public class DebugPolicy : ICancellationPolicy
    {
        public DateTime CalculateChargeDate(Booking booking)
        {
            // Cobrar ahora mismo
            return DateTime.UtcNow;
        }
    }
}
