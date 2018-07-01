using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Model.Response
{
    public class AuthorizationResponse : BaseResponse
    {
        public Entity.Payment Payment { get; set; }
    }
}
