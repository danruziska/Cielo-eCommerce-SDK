using RLabs.Cielo.SDK.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Model.Entity
{
    public class Wallet
    {
        public WalletBrand Type { get; set; }
        public string Eci { get; set; }
        public string Cavv { get; set; }
        public AdditionalData AdditionalData { get; set; }
    }
}
