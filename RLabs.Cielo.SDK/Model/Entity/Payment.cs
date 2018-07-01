using RLabs.Cielo.SDK.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Model.Entity
{
    public class Payment
    {
        public PaymentType Type { get; set; }

        public int Amount { get; set; }

        public int Installments { get; set; }

        public CreditCard CreditCard { get; set; }

        public string PaymentId { get; set; }

        public int ReturnCode { get; set; }

        public string ReturnMessage { get; set; }

        public string TransactionId { get; set; }

        public string AuthenticationUrl { get; set; }

        public string AuthorizationCode { get; set; }

        public string ProofOfSale { get; set; }

        public int Status { get; set; }
    }
}
