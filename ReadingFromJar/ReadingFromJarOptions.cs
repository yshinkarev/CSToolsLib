using CommandLine;

namespace CSToolsLib.ReadingFromJar
{
    [Verb("readingfromjar", HelpText = "Parse gradle build log file and copy using jar/aar files to current folder.")]
    class ReadingFromJarOptions
    {
        [Option('l', "logfile", Required = true,
            HelpText = "Source log file")]
        public string LogFile { get; set; }

        [Option('d', "debug",
           HelpText = "Show debug information.")]
        public bool Debug { get; set; }
    }
}