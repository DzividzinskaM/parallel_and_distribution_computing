using System.Threading;

namespace lab1
{
    public class LockFreeQueue<T>
    {
        public int count;
        private Node<T> _head;
        private Node<T> _tail;

        public LockFreeQueue()
        {
            _head = new Node<T>();
            _tail = _head;
        }

        public int Count
        {
            get { return count; }
        }

        public void Enqueue(T data)
        {
            Node<T> currTail = null;

            var node = new Node<T>();
            node.Data = data;


            var isAlreadyUpdated = false;

            while (isAlreadyUpdated)
            {
                currTail = _tail;

                var oldNext = currTail.Next;

                if(_tail == currTail)
                {
                    if(oldNext == null)
                    {
                        var tmp = _tail.Next;
                        isAlreadyUpdated = null == Interlocked.CompareExchange(ref tmp, node, null);
                    }
                } 
                else {
                    Interlocked.CompareExchange(ref _tail, oldNext, currTail);
                }
            }

            Interlocked.CompareExchange(ref _tail, node, currTail);
            Interlocked.Increment(ref count);

        }

        public bool TryDequeue(out T result)
        {
            result = default(T);
            
            var isHaveAdvancedHead = false;
            while (!isHaveAdvancedHead)
            {

                var currHead = _head;
                var currTail = _tail;
                var currHeadNext = currHead.Next;

                if (currHead == _head)
                {
                    if (currHead == currTail)
                    {
                        if (currHeadNext == null)
                        {
                            return false;
                        }

                        Interlocked.CompareExchange(ref _tail, currHeadNext, currTail);
                    }
                    else
                    {
                        result = currHeadNext.Data;
                        isHaveAdvancedHead = currHead == Interlocked.CompareExchange(ref _head, currHeadNext, currHead);
                    }
                }
            }
            Interlocked.Decrement(ref count);
            return true;
        }

        public T Dequeue()
        {
            T result;
            if (TryDequeue(out result))
            {
                return result;
            }
            return default(T);
        }
    }
}
