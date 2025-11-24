using BNPL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Policies
{
    public class ActivityPolicy : ICancellationPolicy
    {
        public DateTime CalculateChargeDate(Booking booking)
        {
            // Para actividades: cobrar 72 horas antes
            return booking.ServiceDate.AddHours(-72);
        }
    }
}
