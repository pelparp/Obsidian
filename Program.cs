
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CommandLine;
using System.Collections.Generic;
using NLog.Targets;
using NLog;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Globalization;


namespace Obsidian
{
    public class Program
    {
        public class Options
        {
            [Option('i', "infile", Required = true, HelpText = "Absolute path of JSE file to be processed.")]
            public string InFile { get; set; }

            [Option('o', "output", Required = false, HelpText = "Output path.")]
            public string OutDir { get; set; }

        }

        public static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            /* Handle Command Line Arguments Parse Errors*/
            CommandLine.Parser.Default.ParseArguments<Options>(args).WithNotParsed(HandleParseError);
            static void HandleParseError(IEnumerable<Error> errs)
            {
                /* If there are any errors in the command line argument 
                 * parsing, kill the program with exit code 1 */
                System.Environment.Exit(1);
            }

            /* Set up logging configuration */
            var config = new NLog.Config.LoggingConfiguration();
            var logconsole = new ColoredConsoleTarget("logconsole") { Layout = @"[${longdate}] [${level}] ${message} ${exception}" };
            var lognullstream = new NLog.Targets.NullTarget();
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole, "Obsidian.*");
            NLog.LogManager.Configuration = config;
            Console.WriteLine("   ___ _         _     _ _             ");
            Console.WriteLine("  /___\\ |__  ___(_) __| (_) __ _ _ __  ");
            Console.WriteLine(" //  // '_ \\/ __| |/ _` | |/ _` | '_ \\ ");
            Console.WriteLine("/ \\_//| |_) \\__ \\ | (_| | | (_| | | | |");
            Console.WriteLine("\\___/ |_.__/|___/_|\\__,_|_|\\__,_|_| |_|");
            Console.WriteLine("                                       ");
            Console.WriteLine("=========== OSTAP Malware String Recovery ===========");
            Console.WriteLine("                                       ");
            _log.Info("Obsidian OSTAP Malware String Recovery started at {DateNow}", DateTime.UtcNow.ToString());
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    try 
                    {
                        Analyze.Process(o);
                    }
                    catch (Exception err)
                    {
                        _log.Error(err.Message);
                    }
                    
                });

        }
    }

}