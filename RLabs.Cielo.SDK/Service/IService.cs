using RLabs.Cielo.SDK.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Service
{
    public interface IService<TRequest, TResponse>
    {
        TResponse Execute(TRequest request);
    }
}
