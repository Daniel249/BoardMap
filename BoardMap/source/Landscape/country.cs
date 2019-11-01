using BoardMap.Economy;
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

        // population added at construction
        public int population { get; private set; }

        // capital tile
        Tile capital;
        // probably City capital evtl !!

        // references to states
        public List<State> states { get; private set; }

        // domestic marketplace
        public MarketPlace marketPlace { get; private set; }

        public void setColor(int r, int g, int b) {
            color = new Color(r, g, b);
        }

        // add state to states
        public void addState(State _state) {
            population = population + _state.population.Size;
            states.Add(_state);
        }

        // mein economy method
        public void runEconomy() {
            for(int i = 0; i < states.Count; i++) {
                states[i].population.makeOrder();
            }
            marketPlace.solveMarkets();
        }

        // constructor
        public Country(string _name, string _tag) {
            population = 0;
            Name = _name;
            Tag = _tag;
            states = new List<State>();
            marketPlace = new MarketPlace();
        }

    }
}
