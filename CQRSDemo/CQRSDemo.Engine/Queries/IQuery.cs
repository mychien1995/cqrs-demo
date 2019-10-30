using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Queries
{
    public interface IQuery<TOutput>
    {
        TOutput Execute();
    }
}
