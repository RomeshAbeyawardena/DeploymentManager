using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentManager.Contracts
{
    public interface IDateRange<TDate> : IComparable<IDateRange<TDate>>
        where TDate : struct
    {
        TDate? Start { get; }
        TDate? End { get; }
    }
}
