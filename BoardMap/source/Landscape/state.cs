using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Landscape
{
    //
    class State
    {
        // state id
        public int stateID { get; private set; }
        // state name
        public string Name { get; private set; }
        // state color
        public Color color { get; private set; }
        


        // references to tiles
        Tile[] tiles;
        // reference to country
        Country country;
        // cities in state
        // tiles array doesnt contain cities !!

    }
}
