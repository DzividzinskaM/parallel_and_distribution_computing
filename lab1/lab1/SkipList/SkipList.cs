using lab1.SkipList.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab1.SkipList
{
    class SkipList<T>
    {
        private Node<T> _head { get; }

        private Node<T> _tail { get; }

        public SkipList()
        {
            _head = new Node<T>(int.MinValue);
            _tail = new Node<T>(int.MaxValue);

            for (var i = 0; i < _head.Next.Length; ++i)
            {
                _head.Next[i] = new MarkedAtomicReference<Node<T>>(_tail, false);

            }
        }

        public bool Insert(Node<T> node)
        {
            var previous = new Node<T>[Levels.MaxLevel + 1];
            var next = new Node<T>[Levels.MaxLevel + 1];

            while (true)
            {
                if (Find(node, ref previous, ref next))
                {
                    return false;
                }
                var highestPoint = node.TopPoint;

                for (var level = Levels.MinLevel; level <= highestPoint; level++)
                {
                    var tempElem = next[level];
                    node.Next[level] = new MarkedAtomicReference<Node<T>>(tempElem, false);
                }

                var currentPrevious = previous[Levels.MinLevel];
                var currentNext = next[Levels.MinLevel];

                node.Next[Levels.MinLevel] = new MarkedAtomicReference<Node<T>>(currentNext, false);


                for (var level = 1; level <= highestPoint; level++)
                {
                    while (true)
                    {
                        currentPrevious = previous[level];
                        currentNext = next[level];

                        if (currentPrevious.Next[level].CompareAndExchange(node, false, currentNext, false))
                        {
                            break;
                        }

                        Find(node, ref previous, ref next);
                    }
                }

                return true;
            }
        }


        public bool Delete(Node<T> node)
        {
            var previous = new Node<T>[Levels.MaxLevel + 1];
            var next = new Node<T>[Levels.MaxLevel + 1];

            while (true)
            {


                if (!Find(node, ref previousItem, ref NextItem))
                {
                    return false;
                }

                Node<T> currentNext;
                for (var level = node.HighestPoint; level > Levels.MinLevel; level--)
                {
                    var isMarked = false;
                    currentNext = node.Next[level].Get(ref isMarked);

                    while (!isMarked)
                    {
                        node.Next[level].CompareAndExchange(currentNext, true, currentNext, false);
                        currentNext = node.Next[level].Get(ref isMarked);
                    }
                }

                var marked = false;
                currentNext = node.Next[Levels.MinLevel].Get(ref marked);

                while (true)
                {
                    var iMarkedIt = node.Next[Levels.MinLevel].CompareAndExchange(currentNext, true, currentNext, false);
                    currentNext = NextItem[Levels.MinLevel].Next[Levels.MinLevel].Get(ref marked);

                    if (iMarkedIt)
                    {
                        Find(node, ref previousItem, ref NextItem);
                        return true;
                    }

                    if (marked)
                    {
                        return false;
                    }
                }
            }
        }


        private bool Find(Node<T> node, ref Node<T>[] previous, ref Node<T>[] next)
        {
            var isMarked = false;
            var isNeedRetry = false;
            Node<T> searchPoint = null;

            while (true)
            {
                var currentPrevious = _head;
                for (var level = Levels.MaxLevel; level >= Levels.MinLevel; level--)
                {
                    searchPoint = currentPrevious.Next[level].Value;
                    while (true)
                    {
                        var currentNext = searchPoint.Next[level].Get(ref isMarked);
                        while (isMarked)
                        {
                            var snip = currentPrevious.Next[level].CompareAndExchange(currentNext, false, searchPoint, false);
                            if (!snip)
                            {
                                isNeedRetry = true;
                                break;
                            }

                            searchPoint = currentPrevious.Next[level].Value;
                            currentNext = searchPoint.Next[level].Get(ref isMarked);
                        }

                        if (isNeedRetry)
                        {
                            break;
                        }

                        if (searchPoint.NodeKey < node.NodeKey)
                        {
                            currentPrevious = searchPoint;
                            searchPoint = currentNext;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (isNeedRetry)
                    {
                        isNeedRetry = false;
                        continue;
                    }

                    previous[level] = currentPrevious;
                    next[level] = searchPoint;
                }

                return searchPoint != null && searchPoint.NodeKey == node.NodeKey;
            }
        }
    }
}
