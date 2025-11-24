using BNPL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Policies
{
    public class TransferPolicy : ICancellationPolicy
    {
        public DateTime CalculateChargeDate(Booking booking)
        {
            // Para traslados: cobrar 48 horas antes
            return booking.ServiceDate.AddHours(-48);
        }
    }
}
