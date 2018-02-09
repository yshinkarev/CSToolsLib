using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSToolsLib.ReadingFromJar
{
    class ReadingFromJarExecutor
    {
        private bool _isDebug;
        private List<int> _longPathsIndexes = new List<int>();

        public int Execute(ReadingFromJarOptions opts)
        {
            _isDebug = opts.Debug;

            Log("Log file: {0}", opts.LogFile);

            List<string> paths = GetPaths(opts);

            if (paths.Count == 0)
                return 0;

            List<string> name = GetTargetNames(paths);

            for (int i = 0; i < paths.Count; i++)
                CopyFile(paths[i], name[i]);

            return 0;
        }

        private void CopyFile(string path, string name)
        {
            Log("File: {0}", path);

            Console.Write("{0} -> {1} ",
                Path.GetFileName(path), name);

            try
            {
                File.Copy(path, name, true);
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                DL.ErrorLine(ex.Message);
            }
        }

        private List<string> GetTargetNames(List<string> paths)
        {
            List<string> names = paths.Select(Path.GetFileName).ToList();

            List<String> dublicates = names.GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            for (int i = 0; i < names.Count; i++)
            {
                String n = names[i];

                if (!dublicates.Contains(n))
                    continue;

                names[i] = GetLongName(n, paths[i]);
                _longPathsIndexes.Add(i);
            }

            if (_longPathsIndexes.Count > 0)
                names = ReduceLongNames(names);

            return names;
        }

        private List<string> ReduceLongNames(List<string> names)
        {
            List<string> longNames = _longPathsIndexes.Select(i => names[i]).ToList();
            int minLen = longNames.Select(name => name.Length).Min();

            int start = -1;
            bool equals = true;

            while (equals && start < minLen)
            {
                start++;
                char c = longNames[0][start];

                equals = longNames.All(name => name[start] == c);
            }

            if (start == 0)
                return names;

            List<String> result = new List<string>(names);

            foreach (int i in _longPathsIndexes)
                result[i] = names[i].Substring(start);

            return result;
        }

        private string GetLongName(string filename, string path)
        {
            List<string> paths = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).ToList();
            paths.RemoveAt(0);
            return paths.Aggregate((i, j) => i + "__" + j) + "__" + filename;
        }

        private List<string> GetPaths(ReadingFromJarOptions opts)
        {
            StreamReader file = null;

            List<String> paths = new List<string>();

            try
            {
                file = new StreamReader(opts.LogFile);
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    MatchCollection matches = Regex.Matches(line, @"^Reading program jar \[([^]]*)\]");

                    if (matches.Count != 1)
                        continue;

                    string path = matches[0].Groups[1].Value;
                    paths.Add(path);
                }
            }
            finally
            {
                if (file != null)
                    file.Close();
            }

            return paths;
        }

        private void Log(String format, params Object[] args)
        {
            if (_isDebug)
                DL.DebugLine(format, args);
        }
    }
}