using BNPL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Domain.Entities
{
    public class Booking
    {
        public string BookingId { get; set; }
        public ProductType ProductType { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ServiceDate { get; set; }
        public string UserEmail { get; set; }
    }
}
