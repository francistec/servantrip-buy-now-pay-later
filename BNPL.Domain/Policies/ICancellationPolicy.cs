using BNPL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Policies
{
    public interface ICancellationPolicy
    {
        DateTime CalculateChargeDate(Booking booking);
    }
}
