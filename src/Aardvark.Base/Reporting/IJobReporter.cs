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
namespace Aardvark.Base;

public interface IJobReporter
{
    /// <summary>
    /// The number of spaces of indent caused by Begin / Report.End.
    /// This property should only be set BEFORE actual reporting.
    /// </summary>
    int Indent { get; set; }
    void Line(LogType type, int level, ILogTarget target,
              string leftText, int rightPos = 0, string rightText = null);
    // the following methods are regarded as LogType.Info
    void Text(int level, ILogTarget target, string text);
    void Wrap(int level, ILogTarget target, string text);
    ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false);
    double End(int level, ILogTarget target, string text, bool addTimeToParent);
    void Tests(TestInfo testInfo);
    void Progress(int level, ILogTarget target, double progress, bool relative = false);
    void Values(int level, ILogTarget target, string name, string separator, object[] values);
    // internal API for managing external reporters
    void AddReporter(IReporter reporter);
    void RemoveReporter(IReporter reporter);
}
