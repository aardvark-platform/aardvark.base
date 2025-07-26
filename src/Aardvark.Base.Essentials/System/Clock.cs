﻿/*
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Aardvark.Base;

/// <summary>
/// </summary>
public class Clock : IDisposable
{
    private Task m_timeThread;
    private List<Action> m_waiters;
    private object m_queueLock;
    private SemaphoreSlim m_waitersEnqueued;
    private int m_frequency;
    private int m_minimalUpdateTime;
    private CancellationTokenSource m_source;
    private readonly Dictionary<Action, DateTime> m_lastTime;

    #region Constructors

    /// <summary>
    /// </summary>
    public Clock(int maxUpdateFrequency = 0)
    {
        m_waiters = [];
        m_queueLock = new object();
        m_waitersEnqueued = new SemaphoreSlim(0);
        m_frequency = maxUpdateFrequency;
        m_minimalUpdateTime = m_frequency == 0 ? 0 : (int)(1000.0 / (double)m_frequency);
        m_source = new CancellationTokenSource();

        m_lastTime = [];

        m_timeThread = new Task(Run, TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler);
        //m_timeThread.Priority = ThreadPriority.Highest;
        //m_timeThread.Name = "ClockThread";
        //m_timeThread.IsBackground = true;

        m_timeThread.Start();
    }

    #endregion

    #region Properties

    /// <summary>
    /// </summary>
    public int MaximalFrequency
    {
        get { return m_frequency; }
        set
        {
            m_frequency = value;
            m_minimalUpdateTime = m_frequency == 0 ? 0 : (int)(1000.0 / (double)m_frequency);
        }
    }

    /// <summary>
    /// </summary>
    public int MinimalUpdateTime
    {
        get { return m_minimalUpdateTime; }
    }

    /// <summary>
    /// </summary>
    public TimeSpan GetTimeSpanForContinuation(Action continuation, out DateTime now)
    {
        var current = DateTime.Now;

        if (!m_lastTime.TryGetValue(continuation, out DateTime last))
        {
            last = current;
            m_lastTime[continuation] = last;
        }
        m_lastTime[continuation] = current;

        now = current;
        return current - last;
    }

    #endregion

    #region Methods

    /// <summary>
    /// </summary>
    public void Enqueue(Action a)
    {
        lock (m_queueLock)
        {
            m_waiters.Add(a);
            m_waitersEnqueued.Release();
        }
    }

    private void Run()
    {
        var w = new Stopwatch();
        var token = m_source.Token;

        try
        {
            while (true)
            {
                w.Stop();
                m_waitersEnqueued.Wait();
                token.ThrowIfCancellationRequested();

                var wait = System.Math.Max(0, m_minimalUpdateTime - (int)w.Elapsed.TotalMilliseconds);
                if (wait > 0) Thread.Sleep(wait);
                token.ThrowIfCancellationRequested();
                w.Restart();


                List<Action> current = null;
                lock (m_queueLock)
                {
                    current = m_waiters;
                    m_waiters = [];
                }

                for (int i = 1; i < current.Count; i++) m_waitersEnqueued.Wait();

                foreach (var a in current)
                {
                    a();
                }
                token.ThrowIfCancellationRequested();


            }
        }
        catch (OperationCanceledException)
        {
            Report.Warn("clock cancelled");
        }
        catch (Exception e)
        {
            Report.Warn("clock faulted: {0}", e);
        }
    }

    #endregion

    #region IDisposable Members

    /// <summary>
    /// </summary>
    public void Dispose()
    {
        if (m_timeThread != null)
        {
            m_source.Cancel();
            m_waitersEnqueued.Release();
            m_timeThread.Wait();

            m_timeThread = null;
            m_waiters = null;
            m_queueLock = null;
            m_waitersEnqueued = null;
            m_frequency = 0;
            m_minimalUpdateTime = 0;
            m_source = null;
        }
    }

    #endregion
}

/// <summary>
/// </summary>
public static class ClockExtensions
{
    /// <summary>
    /// </summary>
    public static double SanityThreshold = 10;

    #region Awaitable

    private struct FutureAwaiter(ClockExtensions.FutureAwaitable f) : IAwaiter<TimeValue>
    {
        private readonly FutureAwaitable m_future = f;
        private Action m_continuation = null;

        public readonly bool IsCompleted => false;

        private readonly void MakeSane(ref TimeSpan span)
        {
            if (m_future.TimeOut > 0)
            {
                var cap = SanityThreshold * m_future.TimeOut;
                if (span.TotalMilliseconds > cap) span = TimeSpan.FromMilliseconds(cap);
            }
            else if (m_future.Clock.MinimalUpdateTime != 0)
            {
                var cap = SanityThreshold * m_future.Clock.MinimalUpdateTime;
                if (span.TotalMilliseconds > SanityThreshold * m_future.Clock.MinimalUpdateTime) span = TimeSpan.FromMilliseconds(cap);
            }
        }

        public readonly TimeValue GetResult()
        {
            var span = m_future.Clock.GetTimeSpanForContinuation(m_continuation, out DateTime current);

            MakeSane(ref span);


            return new TimeValue(current, span.TotalSeconds);
        }

        public void OnCompleted(Action continuation)
        {
            m_continuation = continuation;
            var timeOut = m_future.TimeOut;

            if (timeOut == 0)
            {
                m_future.Clock.Enqueue(continuation);
            }
            else
            {
                var time = m_future.Clock;

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(timeOut);
                    time.Enqueue(continuation);
                });
            }
        }

        readonly void IAwaiter.GetResult()
        {
            m_future.Clock.GetTimeSpanForContinuation(m_continuation, out _);
        }
    }

    private class FutureAwaitable(Clock time, int timeout = 0) : IAwaitable<TimeValue>
    {
        private readonly Clock m_time = time;
        private readonly int m_timeOut = timeout;

        public int TimeOut
        {
            get { return m_timeOut; }
        }

        public Clock Clock
        {
            get { return m_time; }
        }

        public IAwaiter<TimeValue> GetAwaiter()
        {
            return new FutureAwaiter(this);
        }

        IAwaiter IAwaitable.GetAwaiter()
        {
            return new FutureAwaiter(this);
        }

        public TimeValue Result
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsCompleted
        {
            get { return false; }
        }
    }

    #endregion

    #region Extensions

    /// <summary>
    /// Awaits a time being approximately "timeout" milliseconds in the future.
    /// If timeout is zero the maximal clock frequency limits the execution.
    /// </summary>
    public static IAwaitable<TimeValue> Future(this Clock clock, int timeoutInMilliSeconds = 0)
    {
        return new FutureAwaitable(clock, timeoutInMilliSeconds);
    }

    /// <summary>
    /// </summary>
    public static IAwaitable<TimeValue> Tick(this Clock clock)
    {
        return new FutureAwaitable(clock, 0);
    }

    /// <summary>
    /// </summary>
    public static IEvent<double> TickEvent(this Clock clock)
    {
        var evt = new EventSource<double>(0.0);

        var w = new Stopwatch();
        w.Start();
        void a()
        {
            evt.Emit(w.Elapsed.TotalSeconds);
            //w.Restart();
            clock.Enqueue(a);
        }

        clock.Enqueue(a);

        return evt;
    }

    #endregion
}
