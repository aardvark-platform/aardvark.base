using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    public sealed class Unit
    {
        private Unit() { }

        public static Unit Default { get { return null; } }
    }

    internal class SubjectDisposable<T> : IDisposable
    {
        private Dictionary<IObserver<T>, int> m_store;
        private IObserver<T> m_self;

        public SubjectDisposable(Dictionary<IObserver<T>, int> store, IObserver<T> self)
        {
            m_store = store;
            m_self = self;
        }

        public void Dispose()
        {
            if (m_self == null) return;

            lock(m_store)
            {
                if(m_store.TryGetValue(m_self, out var cnt))
                {
                    cnt--;
                    if (cnt > 0) m_store[m_self] = cnt;
                    else m_store.Remove(m_self);
                }
                m_store = null;
                m_self = null;
            }
        }

    }


    internal class Subject<T> : IObservable<T>, IObserver<T>
    {
        private readonly Dictionary<IObserver<T>, int> m_observers;

        public Subject()
        {
            m_observers = new Dictionary<IObserver<T>, int>();
        }

        public void OnCompleted()
        {
            IObserver<T>[] arr = null;
            lock (m_observers)
            {
                arr = m_observers.Keys.ToArray(m_observers.Count);
            }
            foreach (var obs in arr)
            {
                obs.OnCompleted();
            }
        }

        public void OnNext(T value)
        {
            IObserver<T>[] arr = null;
            lock (m_observers)
            {
                arr = m_observers.Keys.ToArray(m_observers.Count);
            }
            foreach (var obs in arr)
            {
                obs.OnNext(value);
            }
        }

        public void OnError(Exception e)
        {
            IObserver<T>[] arr = null;
            lock (m_observers)
            {
                arr = m_observers.Keys.ToArray(m_observers.Count);
            }
            foreach (var obs in arr)
            {
                obs.OnError(e);
            }
        }

        public IDisposable Subscribe(IObserver<T> obs)
        {
            lock (m_observers)
            {
                if(m_observers.TryGetValue(obs, out var cnt))
                {
                    m_observers[obs] = cnt + 1;
                }
                else
                {
                    m_observers[obs] = 1;
                }

                return new SubjectDisposable<T>(m_observers, obs);
            }
        }

    }

    internal class LambdaObserver<T> : IObserver<T>
    {
        private readonly Action<T> m_action;

        public LambdaObserver(Action<T> action)
        {
            m_action = action;
        }

        public void OnNext(T value)
        {
            m_action(value);
        }

        public void OnCompleted()
        { }

        public void OnError(Exception e)
        { }
    }

    internal class MapObserver<T1, T2> : IObserver<T1>
    {
        private readonly Func<T1, T2> m_mapping;
        private readonly IObserver<T2> m_target;

        public MapObserver(IObserver<T2> target, Func<T1, T2> mapping)
        {
            m_mapping = mapping;
            m_target = target;
        }

        public void OnNext(T1 value)
        {
            m_target.OnNext(m_mapping(value));
        }
        public void OnCompleted()
        {
            m_target.OnCompleted();
        }
        public void OnError(Exception error)
        {
            m_target.OnError(error);
        }
    }

    internal class MapObservable<T1, T2> : IObservable<T2>
    {
        private readonly IObservable<T1> m_input;
        private readonly Func<T1, T2> m_mapping;

        public MapObservable(IObservable<T1> input, Func<T1, T2> mapping)
        {
            m_input = input;
            m_mapping = mapping;
        }

        public IDisposable Subscribe(IObserver<T2> obs)
        {
            return m_input.Subscribe(new MapObserver<T1, T2>(obs, m_mapping));
        }

    }

    internal class NoDisposable : IDisposable
    {
        public NoDisposable() { }
        public void Dispose()
        {}
    }
    internal class NeverObservable<T> : IObservable<T>
    {
        public NeverObservable()
        {

        }
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return new NoDisposable();
        }
    }


}
