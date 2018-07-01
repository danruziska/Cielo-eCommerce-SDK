using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Model.Request
{
    public class PaymentRequest : BaseRequest
    {
        public string PaymentId { get; set; }
        public int Amount { get; set; }
    }
}
