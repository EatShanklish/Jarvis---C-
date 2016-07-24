using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace Jarvis
{
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        //----------------------------------Core Scope/ Main Entry------------------------------------
        static void Main(string[] args)
        {

            //this will greet the user.
            synth.Speak("kol ayre");

            #region Performance Counters
            //this will pull the current CPU load in percentage
            PerformanceCounter perfcpucount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfcpucount.NextValue();

            //this will pull the current available memory in MB
            PerformanceCounter perfmemcount = new PerformanceCounter("Memory", "Available MBytes");
            perfmemcount.NextValue();

            // this will keep track of time it has been powered on in seconds.
            PerformanceCounter perfuptimecount = new PerformanceCounter("System", "System Up Time");
            perfuptimecount.NextValue();
            #endregion

            //this keeps track of total uptime and reads values aloud.
            TimeSpan uptimespan = TimeSpan.FromSeconds(perfuptimecount.NextValue());
            string systemuptimemessage = string.Format("The Current system up time is {0} days {1} hours {2} minutes {3} seconds",
                (int)uptimespan.TotalDays,
                (int)uptimespan.Hours,
                (int)uptimespan.Minutes,
                (int)uptimespan.Seconds);
            synth.Speak(systemuptimemessage);

            //infinite loop
            while (true)
            {
                // stores CPU and Memory usage so the data remains consistant until the end of loop.
                int currentCPUpercentage = (int)perfcpucount.NextValue();
                int currentavailablememory = (int)perfmemcount.NextValue();

                //every one second, print the CPU usage on screen, thread sleep takes milliseconds
                Console.WriteLine("CPU Load        :{0}%", (int)currentCPUpercentage);
                Console.WriteLine("Available Memory:{0} MB", (int)currentavailablememory);

                //Only read when CPU usage is greater than 80 percent
                if (currentCPUpercentage > 80)
                {
                    if (currentCPUpercentage == 100)
                    {
                        String CpuLoadVocalMessage = String.Format("GODDAMN, YOUR PC IS ON FIYAA");
                        Speak(CpuLoadVocalMessage, VoiceGender.Female, 10);
                    }
                    else
                    {
                        String CpuLoadVocalMessage = String.Format("The current CPU Load is {0} percent", currentCPUpercentage);
                        Speak(CpuLoadVocalMessage, VoiceGender.Female, 6);
                    }
                }

                //only warns when current memory is less than one GB
                if (currentavailablememory < 1024)
                {
                    String MemoryVocalMessage = String.Format("The current available memory is {0} megabytes", currentavailablememory);
                  Speak(MemoryVocalMessage, VoiceGender.Male, 5);
                }

                Thread.Sleep(1000);
            }//end of loop}
        }

        // turns the synth function into an object of it's own.
            public static void Speak(String message, VoiceGender voicegender)
             {
                synth.SelectVoiceByHints(voicegender);
                synth.Speak(message);
            }

        //speeds up talking speed
        public static void Speak(String message, VoiceGender voicegender, int rate)
            {
            synth.Rate = rate;
            Speak(message, voicegender);
            }
        }
        }
