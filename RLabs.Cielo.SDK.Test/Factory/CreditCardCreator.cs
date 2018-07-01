using RLabs.Cielo.SDK.Enum;
using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Test.Factory
{
    internal class CreditCardCreator
    {
        private CreditCard createdCreditCard;

        public CreditCardCreator()
        {
            createdCreditCard = new CreditCard();
            createdCreditCard.CardNumber = "0000000000000001";
            createdCreditCard.Holder = "Danilo C Ruziska";
            createdCreditCard.ExpirationDate = "12/2026";
            createdCreditCard.SecurityCode = "123";
            createdCreditCard.Brand = CreditCardBrand.Visa;
        }

        public CreditCardCreator WithCardNumber(string newCardNumber)
        {
            createdCreditCard.CardNumber = newCardNumber;
            return this;
        }

        public CreditCardCreator WithHolder(string newHolder)
        {
            createdCreditCard.Holder = newHolder;
            return this;
        }

        public CreditCardCreator WithExpirationDate(string newExpirationDate)
        {
            createdCreditCard.ExpirationDate = newExpirationDate;
            return this;
        }

        public CreditCardCreator WithSecurityCode(string newSecurityCode)
        {
            createdCreditCard.SecurityCode = newSecurityCode;
            return this;
        }

        public CreditCardCreator WithBrand(CreditCardBrand newCreditCardBrand)
        {
            createdCreditCard.Brand = newCreditCardBrand;
            return this;
        }

        public CreditCard Result
        {
            get
            {
                return createdCreditCard;
            }
        }
    }
}
