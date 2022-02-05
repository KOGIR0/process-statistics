using System;
using System.Diagnostics;
using System.IO;

namespace ProcessStatistics.Statistics
{
    class WindowsCollector : ICollector
    {
        public void Collect(string input, string output = "statistics.txt", int interval = 1000)
        {
            // Start the process.
            using (Process process = Process.Start(input))
            {
                PerformanceCounter theCPUCounter = new PerformanceCounter("Process", "% Processor Time",
                    process.ProcessName);

                string folderName = "Statistics" + DateTime.Now.ToString("-dd-MM-yyyy");
                Directory.CreateDirectory(folderName);
                string outputFilePath = folderName + "\\" + output;
                try
                {
                    // empty output text file
                    File.WriteAllText(outputFilePath, String.Empty);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                // open output file to append
                using (StreamWriter file = File.AppendText(outputFilePath))
                {
                    file.WriteLine("Collecting " + process.ProcessName + " statistics");
                    Console.WriteLine("Collecting " + process.ProcessName + " statistics");
                    // Collect the process statistics until
                    // the user closes the program.
                    do
                    {
                        if (!process.HasExited)
                        {
                            // Refresh the current process property values.
                            process.Refresh();

                            Console.WriteLine();
                            Console.WriteLine($"Physical_memory_usage_: {process.WorkingSet64}");
                            Console.WriteLine($"Private_memory_size___: {process.PrivateMemorySize64}");
                            Console.WriteLine($"Handle_Count__________: {process.HandleCount}");
                            Console.WriteLine($"CPU_Usage_%___________: {theCPUCounter.NextValue()}");
                            Console.WriteLine();

                            file.WriteLine();
                            file.WriteLine($"Physical_memory_usage_: {process.WorkingSet64}");
                            file.WriteLine($"Private_memory_size___: {process.PrivateMemorySize64}");
                            file.WriteLine($"Handle_Count__________: {process.HandleCount}");
                            file.WriteLine($"CPU_Usage_%___________: {theCPUCounter.NextValue()}");
                            file.WriteLine();
                        }
                    }
                    while (!process.WaitForExit(interval));

                    Console.WriteLine($"  Process exit code          : {process.ExitCode}");
                }
            }
        }
    }
}
