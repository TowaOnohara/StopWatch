using System;
using System.Collections.Generic;
using System.Text;

namespace StopWatchCore.Models
{
    public interface IModelPool
    {
        StopWatchModel StopWatch { get; }
    }
}
