using System;
using System.Diagnostics;
using System.IO;
using CSToolsLib.SevenZgpack;

namespace CSToolsLib.SevenZgpack
{
    class SevenZgPackExecutor
    {
        private bool _isDebug;

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        public int Execute(SevenZgPackOptions opts)
        {
            _isDebug = opts.Debug;

            if (!_isDebug)
                FreeConsole();

            Log("Directory: {0}", opts.Directory);
            Log("List file: {0}", opts.ListFile);

            string archiveFileName = ParseArchiveFileName(opts.Directory, opts.ListFile) + ".7z";
            Log("Archive name: {0}", archiveFileName);

            int exitCode = Run7zg(archiveFileName, opts.ListFile);
            Log("Exit Code: {0}", exitCode);

            return exitCode;
        }

        private int Run7zg(string archiveFileName, string listFile)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "7zg",
                Arguments = string.Format("a -ad {0} @{1}", archiveFileName, listFile),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process process = Process.Start(psi);

            if (process == null)
                return 1;

            process.WaitForExit();
            return process.ExitCode;
        }

        private string ParseArchiveFileName(string directory, string listFileName)
        {
            StreamReader file = null;

            try
            {
                file = new StreamReader(listFileName);
                string line, first = null;

                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (first == null)
                        first = line;
                    else
                        return Path.GetFileName(directory.TrimEnd(Path.DirectorySeparatorChar));
                }

                return Path.GetFileNameWithoutExtension(first);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }

        private void Log(String format, params Object[] args)
        {
            if (_isDebug)
                DL.DebugLine(format, args);
        }
    }
}