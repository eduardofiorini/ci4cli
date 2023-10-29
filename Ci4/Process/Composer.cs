using System;
using System.Threading.Tasks;
using ShellProgressBar;
using CliWrap;
using CliWrap.Buffered;
using Colorify;

namespace Ci4.Process
{
    class Composer
    {
        public static async Task<CommandResult> Create(IProgressBar progressBar, string project)
        {
            var cmd = Cli.Wrap("composer")
                .WithArguments(new[] { "create-project", "codeigniter4/appstarter", project })
                .WithValidation(CommandResultValidation.None)
                .WithStandardOutputPipe(PipeTarget.ToDelegate(line =>
                {
                    progressBar.Tick();
                }));
            var cmdResult = await cmd.ExecuteBufferedAsync();
            return cmdResult;
        }

        public static void Env(string project)
        {
            Cli.Wrap("php").WithArguments("-r \"file_exists('" + project + "/.env') || copy('" + project + "/env', '" + project + "/.env');\"").ExecuteBufferedAsync();
        }
    }
}
