using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanDemo
{
    public static class Logger
    {
        public static event EventHandler<string> NeedLog;
        public static event EventHandler NeedClear;
        public static event EventHandler<int> NeedUpdateLine;
        public static int CurrentLine { get; private set; } = 0;

        public static void Write(string message, object sender = null)
        {
            NeedLog?.Invoke(sender, message);
        }

        public static void WriteLine(string message, object sender = null)
        {
            Write(message + "\n", sender);
        }

        public static void Clear(object sender = null)
        {
            NeedClear?.Invoke(sender,null);
        }

        public static void LocateLine(int line)
        {
            CurrentLine = line;
            NeedUpdateLine?.Invoke(null, line);
        }

        public static bool IsFinish = false;

        public static void NextLine()
        {
            if (IsFinish) return;

            CurrentLine = (CurrentLine + 1) % 11;
            if (CurrentLine == 0) CurrentLine = 1;
            NeedUpdateLine?.Invoke(null, CurrentLine);
        }
    }
}
