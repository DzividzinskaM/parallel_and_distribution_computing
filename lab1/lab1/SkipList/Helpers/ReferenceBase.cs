using System;
using System.Collections.Generic;
using System.Text;

namespace lab1.SkipList.Helpers
{
    public class ReferenceBase<T>
    {
        public readonly T Value;
        public readonly AtomicBool Marked;

        public ReferenceBase(T value, bool marked)
        {
            this.Value = value;
            this.Marked = new AtomicBool(marked);
        }
    }
}
