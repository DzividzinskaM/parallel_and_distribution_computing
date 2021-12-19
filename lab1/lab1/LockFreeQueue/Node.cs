using Akka.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab1
{ 
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data, Node<T> next)
        {
            Data = data;
            Next = next;
        }
    }
}
