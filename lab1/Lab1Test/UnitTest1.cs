using lab1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Lab1Test
{
    [TestClass]
    public class CustomMutexTest
    {
        [TestMethod]
        public void WaitTest()
        {
            CustomMutex mutex = new CustomMutex();

            Thread newThread = new Thread(new ThreadStart(ThreadProc));
            mutex.Wait();

            Assert.AreEqual(1, mutex._waitingThreads.Count);
        }

        private void ThreadProc()
        {
            Thread.Sleep(20);
           
        }

        [TestMethod]
        public void Notify()
        {
            CustomMutex mutex = new CustomMutex();

            Thread newThread = new Thread(new ThreadStart(ThreadProc));

            mutex._waitingThreads.Add(Thread.CurrentThread);

            mutex.Notify();

            Assert.AreEqual(0, mutex._waitingThreads.Count);
            
        }

        [TestMethod]
        public void NotifyAll()
        {
            CustomMutex mutex = new CustomMutex();

            Thread newThread = new Thread(new ThreadStart(ThreadProc));

            mutex._waitingThreads.Add(Thread.CurrentThread);
            mutex._waitingThreads.Add(Thread.CurrentThread);
            mutex._waitingThreads.Add(Thread.CurrentThread);

            mutex.NotifyAll();

            Assert.AreEqual(0, mutex._waitingThreads.Count);

        }
    }
}
