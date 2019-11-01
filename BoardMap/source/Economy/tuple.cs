using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Economy
{
    struct Tuple
    {
        public int ID { get; }
        public int Value { get; }

        // constructor
        public Tuple(int _id, int _value) {
            ID = _id;
            Value = _value;
        }
    }
}
