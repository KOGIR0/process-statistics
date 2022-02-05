using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessStatistics.Statistics
{
    interface ICollector
    {
        public void Collect(string input, string output, int interval);
    }
}
