using Newtonsoft.Json.Linq;
using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Formatters.Json.Model
{
    internal static class CustomerFormatter
    {
        internal static dynamic BuildCustomer(Customer customer)
        {
            dynamic customerInfo = new JObject();
            customerInfo.Name = customer.Name;
            return customerInfo;
        }
    }
}
