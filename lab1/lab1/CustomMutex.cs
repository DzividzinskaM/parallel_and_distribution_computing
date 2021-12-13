using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lab1
{
    public class CustomMutex
    {
        private Thread _currentThread;
        private List<Thread> _waitingThreads;
        private int _isLocked = 0;

        private void _lock()
        {
            Interlocked.Exchange(ref _isLocked, 1);
            while (Interlocked.CompareExchange<Thread>(ref _currentThread, Thread.CurrentThread, null) == null)
            {
                Thread.Yield();
            }

        }

        private void _unlock()
        {
            Interlocked.Exchange(ref _currentThread, null);
            Interlocked.Exchange(ref _isLocked, 0);
        }


        public void Wait()
        {
            _waitingThreads.Add(Thread.CurrentThread);
            _unlock();

            while (_isLocked == 0)
            {
                Thread.Yield();
            }
            _lock();
            Interlocked.Exchange(ref _isLocked, 1);
        }

        public void Notify()
        {
            _currentThread = Thread.CurrentThread;
            if (_waitingThreads.Count > 0)
            {
                _waitingThreads.RemoveAt(_waitingThreads.Count);
                Interlocked.Exchange(ref _isLocked, 0);
            } else
            {
                throw new NullReferenceException("No one thread in waiting list");
            }
        }

        public void NotifyAll()
        {
            _waitingThreads.Clear();
        }
    }
}
