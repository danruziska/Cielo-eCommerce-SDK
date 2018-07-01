using FluentValidation;
using FluentValidation.Results;
using NLog;
using RLabs.Cielo.SDK.Model.Entity;
using RLabs.Cielo.SDK.Model.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Model.Request
{
    public class AuthorizationRequest : BaseRequest
    {
        public string MerchantOrderId { get; set; }
        public Customer Customer { get; set; }
        public Model.Entity.Payment Payment { get; set; }
        public Wallet Wallet { get; set; }                     
    }    
}
