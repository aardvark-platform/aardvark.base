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

public class NullReporter : IJobReporter
{
    public int Indent
    {
        get { return 0; }
        set { }
    }

    public void AddReporter(IReporter reporter)
    {
    }

    public ReportJob Begin(ReportJob parentJob, int level, ILogTarget target, string text, bool timed, bool noLog = false)
    {
        return null;
    }

    public double End(int level, ILogTarget target, string text, bool addTimeToParent)
    {
        return 0.0;
    }

    public void Line(LogType type, int level, ILogTarget target, string leftText, int rightPos = 0, string rightText = null)
    {
    }

    public void Progress(int level, ILogTarget target, double progress, bool relative = false)
    {
    }

    public void RemoveReporter(IReporter reporter)
    {
    }

    public void Tests(TestInfo testInfo)
    {
    }

    public void Text(int level, ILogTarget target, string text)
    {
    }

    public void Values(int level, ILogTarget target, string name, string separator, object[] values)
    {
    }

    public void Wrap(int level, ILogTarget target, string text)
    {
    }
}
