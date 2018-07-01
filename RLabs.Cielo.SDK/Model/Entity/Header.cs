using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Model.Entity
{
    public class Header
    {
        public Dictionary<string, string> Headerlist { get; private set; }

        public Header()
        {
            Headerlist = new Dictionary<string, string>();
        }

        public void AddHeaderItem(string fieldName, string fieldValue)
        {
            if (Headerlist != null)
                Headerlist.Add(fieldName, fieldValue);
        }
    }
}
