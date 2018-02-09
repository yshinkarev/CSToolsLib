using CommandLine;

namespace CSToolsLib.SevenZgpack
{
    [Verb("7zgpack", HelpText = "Run 7Z GUI dialogbox for packing from Total Commander.")]
    class SevenZgPackOptions
    {
        [Option('d', "directory", Required = true,
            HelpText = "Source Directory of packing")]
        public string Directory { get; set; }

        [Option('l', "listfile", Required = true,
            HelpText = "File with list of selected files/directories")]
        public string ListFile { get; set; }

        [Option('d', "debug",
           HelpText = "Show debug information.")]
        public bool Debug { get; set; }
    }
}