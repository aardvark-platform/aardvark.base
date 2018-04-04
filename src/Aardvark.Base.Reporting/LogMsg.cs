using System;

namespace Aardvark.Base
{
    [Flags]
    public enum LogType
    {
        Unknown = 0,
        Info = 1,
        Begin = 2,
        End = 3,
        Progress = 4,
        Warn = 5,
        Trace = 6,
        Debug = 7,
        Error = 8,
        Fatal = 9,
    }

    [Flags]
    public enum LogOpt
    {
        None = 0x00,
        EndLine = 0x01,
        Timed = 0x02,
        Join = 0x04,
        Wrap = 0x08,
        NewText = 0x10, // new text on Report.End
    };

    public struct LogMsg
    {
        public LogType Type;
        public LogOpt Opt;
        public int Level;
        public int LeftPos; public string LeftText;
        public int RightPos; public string RightText;

        public LogMsg(LogType type, LogOpt opt, int level,
                      int leftPos, string leftText, int rightPos = 0, string rightText = null)
        {
            Type = type; Opt = opt; Level = level;
            LeftPos = leftPos; LeftText = leftText; RightPos = rightPos; RightText = rightText;
        }
    }
}
