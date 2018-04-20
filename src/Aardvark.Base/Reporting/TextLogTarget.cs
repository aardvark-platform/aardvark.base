using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

namespace Aardvark.Base
{
    public class TextLogTarget : ILogTarget
    {
        private object m_lock;
        private ReportState m_state;
        private Dictionary<int, ReportState> m_stateTable;
        public Action<int, LogType, int, string> WriteAct;
        private int m_width = 80;
        private int m_maxIndent = 40;

        public int Verbosity = 0;
        public bool LogCompleteLinesOnly = false;
        public bool AllowBackspace = false;
        public bool Synchronized = true;
        public Func<int, string> m_prefixFun = threadIndex => String.Format("{0,2:x}: ", threadIndex);

        #region Constructor

        public TextLogTarget(Action<int, LogType, int, string> write, int threadIndex = 0)
        {
            m_lock = new object();
            m_state = new ReportState(threadIndex, m_prefixFun(threadIndex));
            m_stateTable = new Dictionary<int, ReportState>();
            WriteAct = write;
        }

        #endregion

        #region Properties

        public int Width
        {
            get { return m_width; }
            set { m_width = value; m_maxIndent = value / 2; }
        }

        public Func<int, string> PrefixFun
        {
            set
            {
                m_prefixFun = value; m_state.Prefix = value(m_state.TIdx);
                m_state.PrefixLength = m_state.Prefix.Length;
            }
        }

        #endregion

        #region Constants

        const int c_maxWidth = 160;
        const string c_spaces160 =
            "                                                                                "
            + "                                                                                ";
        const string c_dots160 =
            "................................................................................"
            + "................................................................................";
        const string c_back160 =
            "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08"
            + "\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08\x08";

        #endregion

        #region ILogTarget

        public void NewThreadIndex(int threadIndex)
        {
        }

        public void Log(int threadIndex, LogMsg msg)
        {
            if (msg.Level > Verbosity) return;
            if (!Synchronized) { Log(msg); return; }
            lock (m_lock)
            {
                if (threadIndex != m_state.TIdx)
                {
                    if (m_state.Level <= Verbosity && m_state.Buffer.Length > 0 && !LogCompleteLinesOnly)
                    {
                        WriteAct(m_state.TIdx, m_state.Type, m_state.Level, "\n"); // crlf for clean start
                        m_state.DoneCount = 0; // trigger the line to be printed again by the thread later
                    }
                    if (!m_stateTable.TryGetValue(threadIndex, out m_state))
                        m_stateTable[threadIndex] = m_state = new ReportState(threadIndex,
                                                                              m_prefixFun(threadIndex));
                }
                Log(msg);
            }
        }

        public void Dispose()
        {
            lock (m_lock)
            {
                if (m_state.Level <= Verbosity && m_state.Buffer.Length > 0)
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
            }
        }

        #endregion

        #region ReportState

        private class ReportState
        {
            public int TIdx;
            public string Prefix;
            public int PrefixLength;
            public LogType Type;
            public int Level = 0;
            public StringBuilder Buffer;
            public bool Timed;
            public int LeftPos;
            public int RightPos;
            public int DoneCount;

            public ReportState(int threadIndex, string prefix)
            {
                TIdx = threadIndex; Type = LogType.Unknown; Level = 0;
                Prefix = prefix;
                PrefixLength = Prefix.Length;
                Buffer = new StringBuilder(80); DoneCount = 0;
            }

            public void AddSpaceText(int pos, string text)
            {
                int len = Buffer.Length - PrefixLength;
                var fillCount = pos < len ? 0 : pos - len;
                if (fillCount > 0) Buffer.Append(c_spaces160, 0, fillCount);
                Buffer.Append(text);
            }

            public void AddDotsText(int pos, string text, int width)
            {
                int len = Buffer.Length;
                if (pos < 0) pos = width + pos - text.Length;
                if (len > PrefixLength && Buffer[len - 1] != ' ') { Buffer.Append(' '); len += 1; }
                var empty = text == "";
                var fillCount = pos < len ? -1 : pos - len - 1 + (empty ? 1 : 0);
                if (fillCount > 0) Buffer.Append(c_dots160, 0, fillCount);
                if (fillCount >= 0 && !empty) Buffer.Append(' ');
                Buffer.Append(text);
            }

            public string Backspace(int count)
            {
                if (count > Buffer.Length) count = Buffer.Length;
                if (count > DoneCount) count = DoneCount;
                Buffer.Length -= count; DoneCount -= count; return c_back160.Substring(160 - count);
            }

            public string GetBufferLineAndClear()
            {
                int pos = DoneCount; DoneCount = 0; Buffer.Append("\r\n");
                var text = Buffer.ToString(pos, Buffer.Length - pos);
                Buffer.Clear();
                return text;
            }
        }

        #endregion

        #region Log Message

        public void Log(LogMsg msg)
        {
            if (msg.Type == LogType.End)
            {
                if (m_state.Buffer.Length > 0 &&
                    (m_state.Type == LogType.Info
                     || (m_state.Type != LogType.Unknown && m_state.LeftPos != msg.LeftPos))) // different indent == other job
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
                if ((msg.Opt & (LogOpt.Timed | LogOpt.NewText)) == 0) return;// if End is not timed, suppress repeated start message
            }
            if (msg.Type == LogType.Progress && !AllowBackspace) msg.Opt |= LogOpt.EndLine;
            if (LogCompleteLinesOnly)
            {
                if (msg.Type == LogType.Begin)
                {
                    msg.Opt = msg.Opt | LogOpt.EndLine; if ((msg.Opt & LogOpt.Timed) != 0) msg.RightText = "";
                }
                else if (msg.Type == LogType.Progress)
                {
                    msg.Opt = msg.Opt | LogOpt.EndLine;
                }
            }
            else if (m_state.Type == LogType.Begin)
            {
                if (msg.Type == LogType.End)
                {
                    var pos = m_state.LeftPos + m_state.PrefixLength; var len = m_state.Buffer.Length - pos;
                    if (len > 0 // len < 0 if we are from a different indent == other job
                        && msg.LeftText.StartsWith(m_state.Buffer.ToString(pos, len)))
                    {
                        msg.LeftText = msg.LeftText.Substring(len); msg.LeftPos = 0;
                    }
                }
                else if (msg.Type == LogType.Progress)
                {
                    msg.LeftText = null;
                }
                else if ((msg.Opt & LogOpt.Wrap) == 0)
                {
                    if (m_state.Timed) m_state.AddDotsText(m_state.RightPos, "", m_width); // dots to line end
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
                }
            }
            else if (m_state.Type == LogType.Progress)
            {
                if (msg.Type == LogType.Progress)
                {
                    if (AllowBackspace)
                    {
                        if (m_state.DoneCount > 0)
                        {
                            WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.Backspace(msg.RightText.Length));
                            msg.LeftText = null;
                        }
                        else
                            m_state.Buffer.Clear();
                    }
                }
                else if (msg.Type == LogType.End)
                {
                    if (AllowBackspace)
                    {
                        if (m_state.DoneCount > 0)
                        {
                            var len = m_state.Buffer.Length - m_state.PrefixLength - m_state.LeftPos;
                            WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.Backspace(len));
                        }
                        else
                            m_state.Buffer.Clear();
                    }
                }
                else
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
            }
            if ((msg.Opt & LogOpt.Wrap) != 0)
            {
                if (m_state.Buffer.Length + msg.LeftText.Length + 1 > Width + msg.RightPos)
                    WriteAct(m_state.TIdx, m_state.Type, m_state.Level, m_state.GetBufferLineAndClear());
                else if (m_state.Buffer.Length > 0 && m_state.Buffer[m_state.Buffer.Length - 1] != ' ')
                    m_state.Buffer.Append(' ');
            }
            if (m_state.Buffer.Length == 0) m_state.Buffer.Append(m_state.Prefix);
            if (msg.LeftText != null) m_state.AddSpaceText(Min(msg.LeftPos, m_maxIndent), msg.LeftText);
            if (msg.RightText != null) m_state.AddDotsText(Min(msg.RightPos, m_width), msg.RightText, m_width);
            if ((msg.Opt & LogOpt.EndLine) != 0)
            {
                WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.GetBufferLineAndClear());
                m_state.Type = LogType.Unknown;
            }
            else
            {
                if (!LogCompleteLinesOnly)
                {
                    int pos = m_state.DoneCount; int len = m_state.Buffer.Length - pos;
                    WriteAct(m_state.TIdx, msg.Type, msg.Level, m_state.Buffer.ToString(pos, len));
                    m_state.DoneCount = pos + len;
                }
                m_state.Type = msg.Type;
                m_state.Timed = (msg.Opt & LogOpt.Timed) != 0;
                m_state.LeftPos = msg.LeftPos;
                m_state.RightPos = msg.RightPos; // remember EndPos of timed begins for dotting to lineEnd
            }
            m_state.Level = msg.Level;
        }

        #endregion
    }
}
