using System.CommandLine;
using System.Threading.Tasks;
using System.CommandLine.NamingConventionBinder;

namespace ProcessStatistics.CommandLine
{
    class ProcessStatsArgsParser: ICmdParser
    {
        private RootCommand rootCommand;

        public delegate void HandlerFunc(string input, string output, int interval);

        public ProcessStatsArgsParser(HandlerFunc commandHandler)
        {
            rootCommand = new RootCommand(
                description: "Collects statistics about executable programm.");

            Option inputExeFileOption = new Option<string>(
              aliases: new string[] { "--input", "-i" }
              , description: "The path to executable file.");
            inputExeFileOption.IsRequired = true;

            Option intervalOption = new Option<int>(
              aliases: new string[] { "--interval" }
              , description: "The interval in milliseconds."
              , getDefaultValue: () => 1000);
            intervalOption.IsRequired = true;

            Option outputOption = new Option<string>(
              aliases: new string[] { "--output", "-o" }
              , description: "The output file."
              , getDefaultValue: () => "statistics.txt");

            rootCommand.Add(inputExeFileOption);
            rootCommand.Add(outputOption);
            rootCommand.Add(intervalOption);

            rootCommand.Handler = CommandHandler.Create(commandHandler);
        }

        public async Task<int> invoke(string[] args)
        {
            return await rootCommand.InvokeAsync(args);
        }
    }
}
