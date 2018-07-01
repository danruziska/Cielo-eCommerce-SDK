using Newtonsoft.Json.Linq;
using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentModel = RLabs.Cielo.SDK.Model.Entity;

namespace RLabs.Cielo.SDK.Formatters.Json.Model
{
    internal static class PaymentFormatter
    {
        internal static dynamic BuildPayment(PaymentModel.Payment payment)
        {
            dynamic paymentInfo = new JObject();
            paymentInfo.Type = payment.Type.ToString();
            paymentInfo.Amount = payment.Amount;
            paymentInfo.Installments = payment.Installments;
            if (payment.CreditCard != null)
                paymentInfo.CreditCard = BuildCreditCard(payment.CreditCard);
            return paymentInfo;
        }

        private static dynamic BuildCreditCard(CreditCard creditCard)
        {
            dynamic creditCardInfo = new JObject();
            creditCardInfo.CardNumber = creditCard.CardNumber;
            creditCardInfo.Holder = creditCard.Holder;
            creditCardInfo.ExpirationDate = creditCard.ExpirationDate;
            creditCardInfo.SecurityCode = creditCard.SecurityCode;
            creditCardInfo.Brand = creditCard.Brand.ToString();
            return creditCardInfo;
        }

        internal static dynamic BuildWallet(Wallet wallet)
        {
            dynamic walletInfo = new JObject();
            walletInfo.Type = wallet.Type.ToString();
            walletInfo.Eci = wallet.Eci;
            walletInfo.Cavv = wallet.Cavv;
            if (wallet.AdditionalData != null)
            {
                walletInfo.AdditionalInfo = BuildAdditionalData(wallet.AdditionalData);

            }
            return walletInfo;
        }

        private static dynamic BuildAdditionalData(AdditionalData additionalData)
        {
            dynamic additionalDataInfo = new JObject();
            additionalDataInfo.CaptureCode = additionalData.CaptureCode;
            return additionalDataInfo;
        }
    }
}
