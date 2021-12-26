using Akka.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab1
{ 
    public class Node<T>
    { 
        public Node<T> Next { get; set; }
        public T Data { get; set; }

        public Node()
        {
            Next = null;
        }

        public Node(T data)
        {
            //Next = null;
            Data = data;
        }
    }
}
