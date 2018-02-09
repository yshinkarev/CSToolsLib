using System;
using CommandLine;

namespace CSToolsLib
{
    [Verb("copy", HelpText = "????.")]
    public class CopyOption
    {
        [Option('f', "file", Required = true,
            HelpText = "????")]
        public String File { get; set; }
    }
}