using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public interface IReportable
    {
        void ReportValue(int verbosity, string name);
    }
}
