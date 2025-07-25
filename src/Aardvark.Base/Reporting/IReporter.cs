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

/// <summary>
/// The IReporter interface can be implemented for objects to catch
/// reports on a level, where some info is passed with typed parameters
/// (e.g. timing seconds as doubles, values as objects).
/// As soon as a reporter object is added, all Report calls from the
/// same thread are also delivered to that reporter object. Reporting
/// on other threads, where the reporter object was not added, does not
/// appear. If the same reporter object is added multiple times from
/// different threads, all these methods need to do their own locking.
/// If only one reporter object is added from one thread, no locking is
/// necessary. The supplied threadIndex, is a running number for each
/// thread assigned based on their first sending of a Report message.
/// It can be directly used as a key in e.g. an IntDict.
/// </summary>
public interface IReporter
{
    void Line(int threadIndex, LogType type, int level, ILogTarget target,
              string leftText, int rightPos = 0, string rightText = null);
    // the following methods are regarded as LogType.Info
    void Text(int threadIndex, int level, ILogTarget target, string text);
    void Wrap(int threadIndex, int level, ILogTarget target, string text);
    void Begin(int threadIndex, int level, ILogTarget target, string text, bool timed);
    void End(int threadIndex, int level, ILogTarget target, string text, double seconds);
    void Progress(int threadIndex, int level, ILogTarget target, string text, double progress, double seconds);
    void Values(int threadIndex, int level, ILogTarget target, string name, string separator, object[] values);
}
