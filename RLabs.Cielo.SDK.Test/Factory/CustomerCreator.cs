using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Factory
{
    internal class CustomerCreator
    {
        private Customer createdCustomer;

        public CustomerCreator()
        {
            createdCustomer = new Customer();
            createdCustomer.Name = "Danilo Correia Ruziska";
        }

        public CustomerCreator WithName(string newName)
        {
            createdCustomer.Name = newName;
            return this;
        }

        public Customer Result
        {
            get
            {
                return createdCustomer;
            }
        }
    }
}
