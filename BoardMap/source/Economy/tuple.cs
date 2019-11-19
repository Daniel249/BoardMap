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
        public double Value { get; }

        // constructor
        public Tuple(int _id, double _value) {
            ID = _id;
            Value = _value;
        }
    }
}
