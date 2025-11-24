using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNPL.Application.Contracts
{
    public class BookingRequest
    {
        public string BookingId { get; set; }
        public string ProductType { get; set; } // Transfer | Activity
        public decimal TotalAmount { get; set; }
        public DateTime ServiceDate { get; set; }
        public string UserEmail { get; set; }
    }
}
