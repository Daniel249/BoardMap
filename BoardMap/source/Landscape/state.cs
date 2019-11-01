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
    class State
    {
        // state id
        public int ID { get; private set; }
        // state name
        public string Name { get; private set; }
        // state color
        public Color color { get; private set; }

        // population size
        public Population population { get; private set; }

        // references to tiles
        public Tile[] tiles { get; private set; }
        // reference to country
        public Country country { get; private set; }
        // cities in state
        //Polis[]

        // init states in tiles
        public static void initStates(State[] _states) {
            foreach(State _state in _states) {
                for(int i = 0; i < _state.tiles.Length; i++) {
                    _state.tiles[i].setState(_state);
                }
            }
        }

        // state's firms
        List<Firm> stateEconomy;

        // constructor
        public State(int _id, int _manpower, Tile[] _tiles, string _name, Country _country, Color _color) {
            ID = _id;
            tiles = _tiles;
            color = _color;
            country = _country;
            Name = _name;
            stateEconomy = new List<Firm>();
            population = new Population(_manpower, 2000, 2000, this);
            country.addState(this);
        }
    }
}
