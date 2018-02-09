using System.Collections.Generic;
using CommandLine;

namespace CSToolsLib
{
    [Verb("rmdir", HelpText = "Delete folders.")]
    class RmdirOptions
    {
        [Value(0, MetaName = "directory", Required = true,
            HelpText = "Directory to be deleted (available mask).")]
        public string Name { get; set; }

        [Option('e', "exclude", Separator = ':', Required = false,
            HelpText = "Exclude folders from deleting.")]
        public ICollection<string> Exclude { get; set; }

        [Option('d', "debug",
           HelpText = "Show debug information.")]
        public bool Debug { get; set; }

        [Option('s', "simulate",
           HelpText = "Simulate working, but really nothing to delete.")]
        public bool Simulate { get; set; }

        [Option('r', "recursively",
           HelpText = "Recursively delete from all sudirectories")]
        public bool Recursively { get; set; }
    }
}