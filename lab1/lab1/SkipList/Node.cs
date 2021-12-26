using lab1.SkipList.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab1.SkipList
{
    public class Node<T>
    {
        private static uint _randomSeed;

        public T Value { get; }

        public int NodeKey { get; }

        public MarkedAtomicReference<Node<T>>[] Next { get; }

        public int TopPoint { get; }

        public Node(int key)
        {
            NodeKey = key;
            Next = new MarkedAtomicReference<Node<T>>[Levels.MaxLevel + 1];
            for (var i = 0; i < Next.Length; ++i)
            {
                Next[i] = new MarkedAtomicReference<Node<T>>(null, false);
            }
            TopPoint = Levels.MaxLevel;
        }

        public Node(T value, int key)
        {
            Value = value;
            NodeKey = key;
            var height = RandomLevel();
            Next = new MarkedAtomicReference<Node<T>>[height + 1];
            for (var i = 0; i < Next.Length; ++i)
            {
                Next[i] = new MarkedAtomicReference<Node<T>>(null, false);
            }
            TopPoint = height;
        }


        private static int RandomLevel()
        {
            var x = _randomSeed;
            x ^= x << 13;
            x ^= x >> 17;
            _randomSeed = x ^= x << 5;
            if ((x & 0x80000001) != 0)
            {
                return 0;
            }

            var level = 1;
            while (((x >>= 1) & 1) != 0)
            {
                level++;
            }

            return Math.Min(level, Levels.MaxLevel);
        }

    }
}
