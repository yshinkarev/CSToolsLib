using System;
using System.Collections.Generic;

namespace CSToolsLib
{
    class RmdirLogger
    {
        public void LogExcludeList(ICollection<string> exclude)
        {
            DL.DebugLine();

            if (exclude.Count == 0)
                DL.DebugLine("Exclude list is empty");
            else
            {
                DL.DebugLine("Exclude: ");

                foreach (string dir in exclude)
                    DL.DebugLine("   {0}", dir);
            }
        }

        public void LogDirsToDelete(ICollection<string> dirs)
        {
            DL.DebugLine();
            DL.DebugLine("Directories to delete:");

            foreach (string dir in dirs)
                DL.DebugLine("   {0}", dir);
        }

        public void LogOnError(string dir, Exception ex)
        {
            DL.ErrorLine("Can not delete dir: {0}", dir);
            DL.ErrorLine("Exception: {0}", ex);
        }

        public void LogDropDir(string dir, string dirName)
        {
            DL.DebugLine("Drop dir: {0} ({1})", dir, dirName);
        }

        public void LogDeleting(string dir)
        {
            DL.DebugLine("Deleting {0}", dir);
        }
    }
}
