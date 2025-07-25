/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;

namespace Aardvark.Base;

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

public struct LogMsg(
    LogType type, LogOpt opt, int level,
    int leftPos, string leftText,
    int rightPos = 0, string rightText = null
    )
{
    public LogType Type = type;
    public LogOpt Opt = opt;
    public int Level = level;
    public int LeftPos = leftPos; public string LeftText = leftText;
    public int RightPos = rightPos; public string RightText = rightText;
}
