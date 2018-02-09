using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common;

namespace CSToolsLib
{
    class RmdirExecutor
    {
        private bool _isDebug;
        private RmdirLogger _logger;

        public int Execute(RmdirOptions opts)
        {
            _isDebug = opts.Debug;

            if (_isDebug)
                _logger = new RmdirLogger();

            ICollection<string> exclude = ParseExclude(opts.Exclude);
            LogHeader(opts, exclude);

            ICollection<string> dirsToDelete = GetDirsToDelete(opts.Name, opts.Recursively);

            if (dirsToDelete == null)
                return 1;

            if (exclude.Count > 0)
                dirsToDelete = ApplyMask(dirsToDelete, exclude.ToList(), false /* isIncludeMode */);

            if (dirsToDelete.Count == 0)
            {
                Console.WriteLine(" [Nothing todo]");
                return 0;
            }

            Console.Write(". To delete: {0}", dirsToDelete.Count);

            if (_isDebug)
                _logger.LogDirsToDelete(dirsToDelete);

            int err = opts.Simulate ? 0 : DeleteInternal(dirsToDelete);
            Console.WriteLine(". OK");

            if (err == 0)
                return 0;

            DL.WarnLine("Warning. Errors: {0}", err);
            return 1;
        }

        private int DeleteInternal(ICollection<string> dirsToDelete)
        {
            int err = 0;

            foreach (string dir in dirsToDelete)
            {
                try
                {
                    if (_isDebug)
                        _logger.LogDeleting(dir);

                    Directory.Delete(dir, true /* recursive */);
                }
                catch (Exception ex)
                {
                    if (ex is DirectoryNotFoundException)
                        continue;

                    if (TryDeleteOnUnauthorizedAccess(ex, dir))
                        continue;

                    err++;

                    if (_isDebug)
                        _logger.LogOnError(dir, ex);
                }
            }

            return err;
        }

        private bool TryDeleteOnUnauthorizedAccess(Exception ex, string dir)
        {
            if (!(ex is UnauthorizedAccessException))
                return false;

            // For each file change attribute and manual delete.
            string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

            if (U2.IsEmpty(files))
                return false;

            foreach (string f in files)
            {
                File.SetAttributes(f, FileAttributes.Normal);
                try
                {
                    File.Delete(f);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            // Again try delete dir.
            try
            {
                Directory.Delete(dir, true /* recursive */);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private ICollection<string> GetDirsToDelete(string name, bool recursively)
        {
            if (name == "*")
                recursively = false;

            string parentDir = Path.GetDirectoryName(name);
            string patternDir = Path.GetFileName(name);

            if (string.IsNullOrEmpty(parentDir))
                parentDir = Directory.GetCurrentDirectory();
            else
                if (isMasked(parentDir))
            {
                DL.DebugLine();
                DL.ErrorLine("App do not support mask in path (only in last dir)");
                return null;
            }

            ICollection<string> dirs = null;

            if (recursively)
                dirs = Directory.GetDirectories(parentDir, name, SearchOption.AllDirectories);

            if (isMasked(patternDir))
            {
                if (!recursively)
                    dirs = Directory.GetDirectories(parentDir);

                dirs = ApplyMask(dirs, new[] { patternDir }, true /* isIncludeMode */);
            }
            else
            {
                if (!recursively)
                    dirs = new[] { name };
            }

            return dirs;
        }

        private ICollection<string> ApplyMask(ICollection<string> dirs, ICollection<string> filter, bool isIncludeMode)
        {
            if (_isDebug)
                DL.DebugLine();

            if (U2.IsEmpty(filter))
                return isIncludeMode ? new List<string>() : dirs;

            List<string> result = new List<string>();

            // http://stackoverflow.com/a/725352/238089
            Regex[] regex = new Regex[filter.Count];

            int i = 0;

            foreach (string f in filter)
                regex[i++] = new Regex(f.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));

            foreach (string dir in dirs)
            {
                string dirName = Path.GetFileName(dir);

                bool isDrop = (dirName == null);

                if (!isDrop)
                {
                    isDrop = regex.Any(t => t.IsMatch(dirName));

                    if (isIncludeMode)
                        isDrop = !isDrop;
                }

                if (isDrop)
                {
                    if (_isDebug)
                        _logger.LogDropDir(dir, dirName);
                }
                else
                    result.Add(dir);
            }

            return result;
        }

        // Private.

        private void LogHeader(RmdirOptions opts, ICollection<string> exclude)
        {
            Console.Write("Delete dir: {0}", opts.Name);

            if (exclude.Count > 0)
                Console.Write(" (Exclude: [{0}])", exclude.Count);

            if (_isDebug)
                _logger.LogExcludeList(exclude);
        }

        private ICollection<string> ParseExclude(ICollection<string> exclude)
        {
            if (U2.IsEmpty(exclude))
                return new List<string>();

            return exclude;
        }

        private bool isMasked(String dir)
        {
            if (dir == null)
                return false;

            return dir.Contains("*") || dir.Contains("?");
        }
    }
}