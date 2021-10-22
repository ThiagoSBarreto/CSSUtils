using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.ViewModelResolver
{
    public interface IModelFinder
    {
        void Resolve(object View);
    }
}
