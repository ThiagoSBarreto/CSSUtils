using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.Services
{
    public interface IServiceBase
    {
        void Configure();
        void Dispose();
    }
}
