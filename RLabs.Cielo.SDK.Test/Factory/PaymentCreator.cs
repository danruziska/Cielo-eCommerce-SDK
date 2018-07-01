using RLabs.Cielo.SDK.Enum;
using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentClass = RLabs.Cielo.SDK.Model.Entity.Payment;

namespace RLabs.Cielo.SDK.Test.Factory
{
    internal class PaymentCreator
    {
        private PaymentClass createdPayment;

        public PaymentCreator()
        {
            createdPayment = new PaymentClass();
            createdPayment.Type = PaymentType.CreditCard;
            createdPayment.Amount = 15700;
            createdPayment.Installments = 1;
            createdPayment.CreditCard = new CreditCardCreator().Result;
        }

        public PaymentCreator WithType(PaymentType newPaymentType)
        {
            createdPayment.Type = newPaymentType;
            return this;
        }

        public PaymentCreator WithAmount(int newAmount)
        {
            createdPayment.Amount = newAmount;
            return this;
        }

        public PaymentCreator WithInstallments(int newInstallments)
        {
            createdPayment.Installments = newInstallments;
            return this;
        }

        public PaymentCreator WithCreditCard(CreditCard creditCard)
        {
            createdPayment.CreditCard = creditCard;
            return this;
        }

        public PaymentClass Result
        {
            get
            {
                return createdPayment;
            }
        }
    }
}
