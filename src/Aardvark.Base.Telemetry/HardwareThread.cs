using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Aardvark.Base
{
    public static class HardwareThread
    {
        private static Dictionary<int, ProcessThread> s_pts = new Dictionary<int, ProcessThread>();
        private static SpinLock s_ptsLock = new SpinLock();
        
        [DllImport("Kernel32", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        public static extern Int32 GetCurrentWin32ThreadId();

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
}
