using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Externals
{
    abstract class Reader
    {
        // path to file
        readonly string path;

        // main method
        protected abstract bool processFile();

        // returns rows array
        protected abstract string[] readFile();

        public Reader(string _path)
        {
            path = _path;
        }
    }
}
