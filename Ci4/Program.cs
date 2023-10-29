using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommandLine;
using Colorify;
using Colorify.UI;
using ToolBox.Platform;
using System.IO;
using CliWrap.Buffered;
using CliWrap;
using System.Threading;
using ShellProgressBar;
using System.Threading.Tasks;

namespace Ci4
{
    class Program
    {
        public static Format _colorify { get; set; }

        static async Task Main(string[] args)
        {
            switch (OS.GetCurrent())
            {
                case "win":
                case "gnu":
                    _colorify = new Format(Theme.Dark);
                    break;
                case "mac":
                    _colorify = new Format(Theme.Light);
                    break;
            }

            Parser.Default.ParseArguments<Options>(args)
              .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
              .WithNotParsed<Options>((errs) => HandleParseError(errs));

            var arg = args.FirstOrDefault<string>();
            switch (arg)
            {
                case "-n":
                case "--new":
                    if (args.Count() >= 2)
                    {
                        if (args.Count() >= 3)
                        {
                            switch (args[1].ToString())
                            {
                                case "project":
                                    try
                                    {
                                        var project = args[2].ToString();
                                        var options = new ProgressBarOptions
                                        {
                                            BackgroundColor = ConsoleColor.DarkGray,
                                            ProgressCharacter = '\u2593'
                                        };
                                        var resultCode = 1;
                                        Random random = new Random();
                                        using (var pbar = new FixedDurationBar(TimeSpan.FromSeconds(random.Next(30, 38)), "Please wait downloading Codeigniter 4", options))
                                        {
                                            var result = await Process.Composer.Create(pbar, project);
                                            resultCode = result.ExitCode;
                                        }
                                        Process.Composer.Env(project);
                                        _colorify.ResetColor();
                                        Console.WriteLine("");
                                        if (resultCode == 0)
                                        {
                                            _colorify.WriteLine($"Project {project} successfully created!", Colors.bgSuccess);
                                        }
                                        else
                                        {
                                            _colorify.WriteLine($"Project {project} errors occurred while being processed!", Colors.bgDanger);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _colorify.WriteLine($"Error during command execution: {ex.Message}", Colors.bgDanger);
                                    }
                                    break;
                                case "template":
                                    if (Function.Validation.ExistDirectory())
                                    {
                                        _colorify.WriteLine($"Tempalte {args[2]} successfully created!", Colors.bgSuccess);
                                    }
                                    else
                                    {
                                        Function.Alert.ExistDirectory();
                                    }
                                    break;
                                case "api":
                                    if (Function.Validation.ExistDirectory())
                                    {
                                        _colorify.WriteLine($"Api {args[2]} successfully created!", Colors.bgSuccess);
                                    }
                                    else
                                    {
                                        Function.Alert.ExistDirectory();
                                    }
                                    break;
                                default:
                                    _colorify.WriteLine($"Command {args[1]} is not valid, use one of these commands: project, template or api.", Colors.bgDanger);
                                    _colorify.ResetColor();
                                    break;
                            }
                        }
                        else
                        {
                            _colorify.WriteLine("\nCreating a new project or derivations with null or empty name is not allowed.", Colors.bgDanger);
                            _colorify.ResetColor();
                        }
                        _colorify.ResetColor();
                    }
                    break;
                case "-c":
                case "--create":
                    if (args.Count() >= 2)
                    {
                        if (args.Count() >= 3)
                        {
                            if (Function.Validation.ExistDirectory())
                            {
                                if (!File.Exists(".env"))
                                {
                                    Function.Alert.ExistFileEnv();
                                    break;
                                }

                                switch (args[1].ToString())
                                {
                                    case "cell":
                                        Process.Maker.Cell(args[2].ToString());
                                        break;
                                    case "command":
                                        Process.Maker.Command(args[2].ToString());
                                        break;
                                    case "config":
                                        Process.Maker.Config(args[2].ToString());
                                        break;
                                    case "controller":
                                        Process.Maker.Controller(args[2].ToString());
                                        break;
                                    case "entitie":
                                        Process.Maker.Entitie(args[2].ToString());
                                        break;
                                    case "filter":
                                        Process.Maker.Filter(args[2].ToString());
                                        break;
                                    case "migration":
                                        Process.Maker.Migration(args[2].ToString());
                                        break;
                                    case "seed":
                                        Process.Maker.Seed(args[2].ToString());
                                        break;
                                    case "model":
                                        Process.Maker.Model(args[2].ToString());
                                        break;
                                    case "validation":
                                        Process.Maker.Validation(args[2].ToString());
                                        break;
                                    case "helper":
                                        Process.Maker.Helper(args[2].ToString());
                                        break;
                                    default:
                                        _colorify.WriteLine("Command \"" + args[1].ToString() + "\" is not valid, use one of these commands: page, crud, entity or helper.", Colors.bgDanger);
                                        _colorify.ResetColor();
                                        break;
                                }
                            }
                            else
                            {
                                Function.Alert.ExistDirectory();
                            }

                        }
                        else
                        {
                            _colorify.WriteLine("\nCreating a new page or derivations with null or empty name is not allowed.", Colors.bgDanger);
                            _colorify.ResetColor();
                        }
                        _colorify.ResetColor();
                    }
                    break;
                case "-l":
                case "--list":
                    if (args.Count() >= 2)
                    {
                        switch (args[1].ToString())
                        {
                            case "translate":
                                Process.List.Translate();
                                break;
                            case "template":
                                Process.List.Template();
                                break;
                            default:
                                _colorify.WriteLine("Command \"" + args[1].ToString() + "\" is not valid, use one of these commands: translate or template.", Colors.bgDanger);
                                _colorify.ResetColor();
                                break;
                        }
                    }
                    break;
                case "-s":
                case "--server":
                    if (args.Count() >= 2)
                    {
                        if (!Function.Validation.ExistDirectory())
                        {
                            Function.Alert.ExistDirectory();
                            break;
                        }
           
                        switch (args[1].ToString())
                        {
                            case "start":
                                var port = "8080";
                                if (args.Count() >= 3)
                                {
                                    port = args[2].ToString();
                                }
                                _colorify.WriteLine("Wait for starting server at port " + port, Colors.bgMuted);
                                Cli.Wrap("php").WithArguments("spark serve --port=" + port).ExecuteBufferedAsync().Select(r => r.StandardOutput);
                                Thread.Sleep(3000);
                                var urlServer = "http://localhost:" + port;
                                _colorify.WriteLine("Server successfully started on:", Colors.bgSuccess);
                                _colorify.WriteLine(urlServer, Colors.bgSuccess);
                                var chrome = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                                if (File.Exists(chrome))
                                {
                                    _colorify.WriteLine("Wait by opening google chromes...", Colors.bgMuted);
                                    Thread.Sleep(2000);
                                    Cli.Wrap(chrome).WithArguments(urlServer).ExecuteAsync();
                                }
                                break;
                            case "stop":
                                
                                break;
                            default:
                                _colorify.WriteLine("Command \"" + args[1].ToString() + "\" is not valid, use one of these commands: start or stop.", Colors.bgDanger);
                                _colorify.ResetColor();
                                break;
                        }
                    }
                    else
                    {
                        _colorify.WriteLine("\nThe server command must start or stop.", Colors.bgDanger);
                        _colorify.ResetColor();
                    }
                    break;
                case "--help":
                    break;
                case "--version":
                    break;
                default:
                    if (string.IsNullOrEmpty(arg))
                    {
                        Header();
                    }
                    else if (!arg.Contains("-"))
                    {
                        Console.WriteLine("\nCommand entered is invalid or does not exist, please type --help to see options.");
                    }
                    break;
            }
        }

        private static void Header()
        {
            _colorify.Clear();
            _colorify.WriteLine(@"  _________   ___   __    __    _________   ___          ___   ", Colors.bgDanger);
            _colorify.WriteLine(@" |         | |   | |  |  |  |  |         | |   |        |   |  ", Colors.bgDanger);
            _colorify.WriteLine(@" |    _____| |   | |  |  |  |  |    _____| |   |        |   |  ", Colors.bgDanger);
            _colorify.WriteLine(@" |   |       |   | |  |__|  |  |   |       |   |        |   |  ", Colors.bgDanger);
            _colorify.WriteLine(@" |   |       |   | |_____   |  |   |       |   |        |   |  ", Colors.bgDanger);
            _colorify.WriteLine(@" |   |_____  |   |       |  |  |   |_____  |   |______  |   |  ", Colors.bgDanger);
            _colorify.WriteLine(@" |         | |   |       |  |  |         | |          | |   |  ", Colors.bgDanger);
            _colorify.WriteLine(@" |_________| |___|       |__|  |_________| |__________| |___|  ", Colors.bgDanger);
            _colorify.WriteLine(@"                                                               ", Colors.bgDanger);
            _colorify.WriteLine("", Colors.bgDefault);
            _colorify.WriteLine("  Welcome to the Ci4CLI ", Colors.bgDefault);
            _colorify.WriteLine("  Version: " + Version(), Colors.bgDefault);
            _colorify.WriteLine("  https://github.com/eduardofiorini/ci4cli", Colors.bgDefault);
            _colorify.WriteLine("", Colors.bgDefault);
            _colorify.WriteLine("", Colors.bgDanger);
            _colorify.ResetColor();
        }

        private static string Version()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }



        private static int RunOptionsAndReturnExitCode(Options options)
        {
            return 0;
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
        }
    }
}
