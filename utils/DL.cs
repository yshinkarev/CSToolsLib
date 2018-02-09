using System;

namespace CSToolsLib
{
    static class DL
    {
        public static void DebugLine()
        {
            Console.WriteLine();
        }

        // Error.

        public static void ErrorLine(String format, params Object[] args)
        {
            LogErrorInternal(true /* writeLine */, format, args);
        }

        public static void Error(String format, params Object[] args)
        {
            LogErrorInternal(false /* writeLine */, format, args);
        }

        // Warn.

        public static void WarnLine(String format, params Object[] args)
        {
            LogWarnInternal(true /* writeLine */, format, args);
        }

        public static void Warn(String format, params Object[] args)
        {
            LogWarnInternal(false /* writeLine */, format, args);
        }

        // Debug.

        public static void DebugLine(String format, params Object[] args)
        {
            LogDebugInternal(true /* writeLine */, format, args);
        }

        public static void Debug(String format, params Object[] args)
        {
            LogDebugInternal(false /* writeLine */, format, args);
        }

        #region Private

        private static void LogWarnInternal(bool writeLine, String format, Object[] args)
        {
            LogInternal(ConsoleColor.DarkGreen, writeLine, format, args);
        }

        private static void LogErrorInternal(bool writeLine, String format, Object[] args)
        {
            LogInternal(ConsoleColor.DarkRed, writeLine, format, args);
        }

        private static void LogDebugInternal(bool writeLine, String format, Object[] args)
        {
            LogInternal(ConsoleColor.DarkGray, writeLine, format, args);
        }

        private static void LogInternal(ConsoleColor color, bool writeLine, String format, Object[] args)
        {
            Console.ForegroundColor = color;

            try
            {
                if (writeLine)
                    Console.WriteLine(format, args);
                else
                    Console.Write(format, args);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        #endregion
    }
}