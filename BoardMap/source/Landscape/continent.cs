using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.LandscapeNS
{
    // each of the continents + oceans 
    // used to contain states rather than countries
    class Continent
    {
        // continent id
        public int ID { get; private set; }
        // continent name
        public string Name { get; private set; }
        // continent color
        public Color color { get; private set; }

        State[] states;

        public Continent(int _id, string _name, Color _color) {
            states = new State[0];
            ID = _id;
            Name = _name;
            color = _color;
        }
    }
}
