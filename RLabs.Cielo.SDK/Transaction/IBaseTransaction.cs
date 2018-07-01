using RLabs.Cielo.SDK.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Transaction
{
    internal interface IBaseTransaction<TRequest, TResponse>
    {        
        TResponse Execute(TRequest requestData);
    }
}
