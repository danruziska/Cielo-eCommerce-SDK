using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Model.Entity
{
    public class Credential
    {
        internal Dictionary<string, string> CredentialsList { get; private set; }

        public Credential()
        {
            CredentialsList = new Dictionary<string, string>();
        }

        public void AddCredentialItem(string fieldName, string fieldValue)
        {
            if (CredentialsList != null)
                CredentialsList.Add(fieldName, fieldValue);
        }
    }
}
