using CommandLine;
using CSToolsLib.ReadingFromJar;
using CSToolsLib.SevenZgpack;

namespace CSToolsLib
{
    static class Program
    {
        //        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        //        static extern bool FreeConsole();

        public static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<RmdirOptions, CopyOption, SevenZgPackOptions, ReadingFromJarOptions>(args);

            int errCode = result.MapResult(
                (RmdirOptions opts) => new RmdirExecutor().Execute(opts),
                (CopyOption opts) => new CopyExecutor().Execute(opts),
                (SevenZgPackOptions opts) => new SevenZgPackExecutor().Execute(opts),
                (ReadingFromJarOptions opts) => new ReadingFromJarExecutor().Execute(opts),
                errors => 2);

#if DEBUG
            System.Console.ReadKey();
#endif

            return errCode;
        }
    }
}