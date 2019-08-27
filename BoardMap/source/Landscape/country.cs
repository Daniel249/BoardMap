using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.LandscapeNS
{
    //
    class Country
    {
        // country id
        public int ID { get; private set; }
        // state name
        public string Name { get; private set; }
        // country color
        public Color color { get; private set; }

        // 3 letter tag
        public string Tag { get; private set; }

        // capital tile
        Tile capital;
        // probably City capital evtl !!

        // references to states
        public List<State> states { get; private set; }

        public void setColor(int r, int g, int b) {
            color = new Color(r, g, b);
        }

        // add state to states
        public void addState(State _state) {
            states.Add(_state);
        }

        // constructor
        public Country(string _name, string _tag) {
            Name = _name;
            Tag = _tag;
            states = new List<State>();
        }

    }
}
