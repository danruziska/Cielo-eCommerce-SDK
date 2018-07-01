using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Util
{
    internal interface IHttpWrapper
    {
        HttpResponseMessage Post(Credential credentials, Header header, string body, string url);

        HttpResponseMessage Put(Credential credentials, Header header, string body, string url);

        HttpResponseMessage Get(Credential credentials, Header header, string body, string url);
    }
}
