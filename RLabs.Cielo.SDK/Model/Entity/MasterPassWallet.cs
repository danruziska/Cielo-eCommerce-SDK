using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Integrator.CieloV2.Model.Entity
{
    public class MasterPassWallet
    {
        public string Type { get; set; }
        public int Eci { get; set; }
        public string Cavv { get; set; }    
        public AdditionalData AdditionalData { get; set; }
    }

    public class AdditionalData
    {
        public string CaptureCode { get; set; }
    }
}
