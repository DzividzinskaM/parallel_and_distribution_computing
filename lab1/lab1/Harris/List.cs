using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lab1.Harris
{
    class List<T> where T : IComparable<T>
    {
        public Node<T> head;
        public Node<T> tail;
        public Node<T> temp;

        public List()
        {
            head = new Node<T>();
            tail = new Node<T>();
            head.Next = tail;
        }

     
        public void Insert(T data)
        {
            Node<T> newNode = new Node<T>(data);
            Node<T> rightNode = new Node<T>(default);
            Node<T> leftNode = new Node<T>(default);
           


            do
            {
                rightNode = Search(data, ref leftNode);
                if (rightNode != null && rightNode.Data.CompareTo(data) == 0)
                    return;
                newNode.Next = rightNode;
                Node<T> leftNodeNext = leftNode.Next;
                if (CAS(ref leftNodeNext, newNode, rightNode))
                    return;
            } while (true); 


        }

        public void Delete(T data)
        {
            try
            {
                var rightNode = new Node<T>();
                var leftNode = new Node<T>();
                var rightNodeNext = new Node<T>();

                rightNode = Search(data, ref leftNode);
                if (rightNode.Data.CompareTo(data) != 0) return;

                rightNodeNext = rightNode;
                
                if(leftNode == null && rightNode.Data.CompareTo(data) == 0)
                {
                    do
                    {
                        temp = rightNode;
                    } while (!CAS(ref rightNode, rightNode.Next, temp));
                    head = rightNode;
                    return;
                }
                do
                {
                    temp = rightNode;
                } while (!CAS(ref rightNode, rightNode.Next, temp));

                leftNode.Next = rightNode;

                if(rightNode.Next == null)
                {
                    tail = leftNode;
                }
            }catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        private bool CAS(ref Node<T> compare, Node<T> swapVal, Node<T> compareVal)
        {
            return Interlocked.CompareExchange<Node<T>>(ref compare, swapVal, compareVal) == compareVal;
        }

        private Node<T> Search(T val, ref Node<T> leftNode)
        {
            var rightNode = new Node<T>();
            var leftNodeNext = new Node<T>();
            do
            {
                var t = head;
                var tNext = head.Next;

                do
                {
                    var isMarked = t.Data.CompareTo(val);
                    if (!(isMarked >= 0))
                    {
                        leftNode = t;
                        leftNodeNext = leftNode.Next;
                    }
                    else break;

                    t = leftNodeNext;
                    if (t == tail) break;
                } while (leftNodeNext != null);

                rightNode = t;

                if (leftNode == null && CAS(ref rightNode, head, head))
                {
                    return rightNode;
                }

                if (leftNode.Next == rightNode)
                {
                    return rightNode;
                }

                if (CAS(ref leftNodeNext, rightNode, leftNodeNext) == true)
                {
                    return rightNode;
                }

            } while (true);
        }
    }
}
