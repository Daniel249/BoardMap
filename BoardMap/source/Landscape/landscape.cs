using BoardMap.Externals;
using BoardMap.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.LandscapeNS
{
    class Landscape
    {
        // continents
        Continent[] continents;

        // countries
        Country[] countries;

        // states 
        State[] states;

        // tiles
        Tile[] definitions;
        public Dictionary<Color, Tile> tiles { get; private set; }


        // get tile from dictionary
        public Tile searchTile(Color _color) {
            // if found in dictionary return
            Tile tile;
            if (tiles.TryGetValue(_color, out tile)) {
                return tile;
            } else {
                return tiles.First().Value;
            }
            // else return
        }


        // check tile with most textures
        public Tile getmaxTextures() {
            
            // get random tile
            Tile currentTile = tiles.First().Value;// = new Tile();
            // to return actual max
            foreach (Tile _tile in tiles.Values) {
                if (currentTile.textures.Count < _tile.textures.Count) {
                    currentTile = _tile;
                }
            }
            return currentTile;
        }

        // constructor
        public Landscape(Texture2D _texture) {
            // init loader with map texture
            TileLoader tloader = new TileLoader(_texture);
            // init DefinitionsReader
            DefReader dreader = new DefReader();

            // run loader and set tiles
            tiles = tloader.processMap(40);
            // read file and set definitions
            definitions = dreader.processFile(tiles);
        }
        // create landscale without loading tiles
        // init stuff to 0
        public Landscape() {
            tiles = new Dictionary<Color, Tile>();
            states = new State[0];
            countries = new Country[0];
            continents = new Continent[0];
        }
    }
}
