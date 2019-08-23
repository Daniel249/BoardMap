using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Landscape
{
    //
    class Country
    {
        // country id
        public int countryID { get; private set; }
        // state name
        public string Name { get; private set; }
        // country color
        public Color color { get; private set; }

        // capital tile
        Tile capital;
        // probably City capital evtl !!

        // references to states
        State[] states;
    }
}
