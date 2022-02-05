using System.Threading.Tasks;
using System.Runtime.InteropServices;

using ProcessStatistics.Statistics;
using ProcessStatistics.CommandLine;

namespace ProcessStatistics
{
    class Program
    {

        public static async Task<int> Main(params string[] args)
        {
            ICmdParser cmdParser;
            ICollector collector;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                collector = new WindowsCollector();
                cmdParser = new ProcessStatsArgsParser(collector.Collect);
            } else
            {
                collector = new LinuxCollector();
                cmdParser = new ProcessStatsArgsParser(collector.Collect);
            }
            return await cmdParser.invoke(args);
        }
    }
}
