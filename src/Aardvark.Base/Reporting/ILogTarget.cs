using System;

namespace Aardvark.Base
{
    public interface ILogTarget : IDisposable
    {
        void NewThreadIndex(int threadIndex);
        void Log(int threadIndex, LogMsg msg);
    }
}
