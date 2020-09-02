using CitizenFX.Core.Native;
using System;
using System.IO;

namespace Server.Utils
{
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error
    }

    public static class Logger
    {
        private static object lockObj = new object();

        public static LogLevel Level { get; } = LogLevel.Trace;

        public static string Prefix { get; } = "SERVER";

        public static void Trace(string message)
        {
            Log(message, LogLevel.Trace);
        }

        public static void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Log(message, LogLevel.Debug);
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Log(message, LogLevel.Info);
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Log(message, LogLevel.Warn);
        }

        public static void Error(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Error(exception, "ERROR");
        }

        public static void Error(Exception exception, string message)
        {
            var output = $"{message}:{Environment.NewLine}{exception.Message}{Environment.NewLine}";
            var ex = exception;

            while (ex.InnerException != null)
            {
                output += $"{ex.InnerException.Message}{Environment.NewLine}";
                ex = ex.InnerException;
            }

            Log($"{output} {exception.TargetSite} in {exception.Source}{Environment.NewLine}{exception.StackTrace}", LogLevel.Error);
        }

        private static void Log(string args, LogLevel logLevel = LogLevel.Trace)
        {
            lock (lockObj)
            {
                if (Level > logLevel)
                    return;

                //var path = Path.Combine(API.GetResourcePath(API.GetCurrentResourceName()), "Logs");
                var path = "Logs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var writerPath = Path.Combine(path, $"Log_{DateTime.Now.ToString("dd-MM-yyyy")}.log");
                var writer = new StreamWriter(writerPath, true);

                var text = $"[{DateTime.Now.ToString("HH:mm:ss.fff")}] {logLevel}: {args}";
                writer.WriteLine(text);
                Console.WriteLine(text);
                writer.Close();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void Exception(Exception ex, string args = "")
        {
            Log($"{args} {ex.ToString()}", LogLevel.Error);
        }
    }
}