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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Aardvark.Base;

public static class HardwareThread
{
    private static readonly Dictionary<int, ProcessThread> s_pts = [];
    private static SpinLock s_ptsLock = new();

    public static ProcessThread GetProcessThread(int tid)
    {
        ProcessThread result = null;

        bool lockTaken = false;
        try
        {
            s_ptsLock.Enter(ref lockTaken);
            if (s_pts.TryGetValue(tid, out result)) return result;
        }
        finally
        {
            if (lockTaken) s_ptsLock.Exit();
        }

        foreach (ProcessThread t in Process.GetCurrentProcess().Threads)
        {
            if (t.Id == tid)
            {
                lockTaken = false;
                try
                {
                    s_ptsLock.Enter(ref lockTaken);
                    s_pts[tid] = t;
                }
                finally
                {
                    if (lockTaken) s_ptsLock.Exit();
                }
                return t;
            }
        }

        return null;
    }
}
